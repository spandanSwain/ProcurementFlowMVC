using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ProcurementFlow.Data;
using ProcurementFlow.Models.ConfigItems;
using ProcurementFlow.Models.PurchaseOrder;

namespace ProcurementFlow.DataAccess
{
    public class PurchaseOrder
    {
        private readonly ProcurementFlowContext _context;
        private readonly IMemoryCache _cache;
        private readonly EmailConfig _config;
        public PurchaseOrder(ProcurementFlowContext context, IMemoryCache cache, IOptions<EmailConfig> config)
        {
            _context = context;
            _cache = cache;
            _config = config.Value;
        }
        
        public async Task<PurchaseOrderViewModel> GetPendingRequisitionsAsync(int? page)
        {
            var data = await _context.Set<PendingRequisitions>()
                       .FromSqlInterpolated($"EXECUTE SP_SELECT_PENDING_REQUISITIONS").AsNoTracking().ToListAsync();
            var model = new PurchaseOrderViewModel
            {
                PendingRequisitions = PaginatedList<PendingRequisitions>.CreateFromList(data, page ?? 1, 5)
            };
            return model;
        }

        public async Task<int> PerformApprovalActionAsync(PurchaseOrderViewModel model)
        {
            try
            {
                var data = await _context.Set<ApprovalItemReceipt>()
                          .FromSqlInterpolated($"EXECUTE SP_APPROVE_REQUISITION @V_ID={model.SelectedId}, @V_USER={model.User}")
                          .AsNoTracking().ToListAsync();
                int resp = SendApprovalEmail(data[0]);
                return (resp == 1 ? 1 : 0);
            }
            catch (Exception ex) { Console.WriteLine($"Exception at DataAccess/PurchaseOrder.cs/PerformApprovalActionAsync() -> {ex}"); return 0; }
        }

        public async Task<int> PerformRejectionActionAsync(PurchaseOrderViewModel model)
        {
            try
            {
                var data = await _context.Set<RejectionItemReceipt>().FromSqlInterpolated
                          ($"EXECUTE SP_REJECT_REQUISITION @V_ID={model.SelectedId}, @V_USER={model.User}, @V_COMMENT={model.Comment}")
                          .AsNoTracking().ToListAsync();
                int resp = SendRejectionEmail(data[0]);
                return (resp == 1 ? 1 : 0);
            }
            catch (Exception ex) { Console.WriteLine($"Exception at DataAccess/PurchaseOrder.cs/PerformRejectionActionAsync() -> {ex}"); return 0; }
        }

        private int SendRejectionEmail(RejectionItemReceipt data)
        {
            var Port = Convert.ToInt32(_config.Port); var MailServer = _config.MailServer;
            var MailFrom = _config.MailFrom; var SmtpAddress = _config.SmtpAddress;
            var Password = _config.Password; var MailTo = data.Requestor!;
            var ccList = (data.IsSameUser == "True" ? new List<string>() : new List<string> { data.RejectedBy! });
            string mailSubject = $"Purchase Order Rejection – Item \"{data.ItemName}\""; string htmlBody = BuildRejectionHtml(data);
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    foreach (string cc in ccList) if (!string.IsNullOrWhiteSpace(cc)) mail.CC.Add(cc.Trim());
                    mail.From = new MailAddress(MailFrom!); mail.Subject = mailSubject;
                    mail.Body = htmlBody; mail.IsBodyHtml = true; mail.To.Add(MailTo!.Trim());

                    using (SmtpClient smtp = new SmtpClient(SmtpAddress, Port))
                    {
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(MailFrom, Password);
                        smtp.Send(mail);
                        Console.WriteLine("Email sent successfully.");
                        return 1;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Exception at DataAccess/PurchaseOrder.cs/SendApprovalEmail() -> {ex}"); return 0; }
        }

        private int SendApprovalEmail(ApprovalItemReceipt data)
        {
            var Port = Convert.ToInt32(_config.Port); var MailServer = _config.MailServer;
            var MailFrom = _config.MailFrom; var SmtpAddress = _config.SmtpAddress;
            var Password = _config.Password; var MailTo = _config.MailTo;
            var ccList = (data.IsSameUser == "True" ? new List<string> { data.Approver! } : new List<string> { data.Approver!, data.Requestor! });
            string mailSubject = $"Purchase Order Approval – Item \"{data.ItemName}\" Ready for Supply"; string htmlBody = BuildReceiptHtml(data);
            try
            {
                using(MailMessage mail =  new MailMessage())
                {
                    foreach (string cc in ccList) if (!string.IsNullOrWhiteSpace(cc)) mail.CC.Add(cc.Trim());
                    mail.From = new MailAddress(MailFrom!); mail.Subject = mailSubject;
                    mail.Body = htmlBody; mail.IsBodyHtml = true; mail.To.Add(MailTo!.Trim());

                    using (SmtpClient smtp = new SmtpClient(SmtpAddress, Port))
                    {
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(MailFrom, Password);
                        smtp.Send(mail);
                        Console.WriteLine("Email sent successfully.");
                        return 1;
                    }
                }
            }
            catch(Exception ex) { Console.WriteLine($"Exception at DataAccess/PurchaseOrder.cs/SendApprovalEmail() -> {ex}"); return 0; }
        }

        private string BuildRejectionHtml(RejectionItemReceipt data)
        {
            string Safe(object o) => o?.ToString() ?? "-";

            return $@"
                <table width='100%' cellpadding='0' cellspacing='0' style='font-family:Arial, sans-serif; border-collapse:collapse; background:#1b0a0a; color:#ff5c5c;'>
                    <tr>
                        <td style='padding:20px; background:#1b0a0a; border-bottom:2px solid #ff2a2a; text-align:center;'>
                            <h2 style='margin:0; font-size:22px; color:#ff5c5c;'>Rejection Receipt</h2>
                            <p style='margin:5px 0 0; color:#ff9999;'>
                                Your requisition was rejected by <span style='color:#ff5c5c;'>{Safe(data.RejectedByName!)}</span> (<span style='color:#ff5c5c;'>{Safe(data.RejectedByRole!)}</span>) with comments: """"<span style='color:#ff5c5c;'>{Safe(data.Comments!)}</span>"""" on <span style='color:#ff5c5c;'>{data.RejectedOn?.ToString("dd-MM-yyyy")}</span>.
                            </p>
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:20px;'>
                            <table width='100%' cellpadding='8' cellspacing='0' style='border:1px solid #ff2a2a; border-radius:6px; background:#000000;'>
                                <tr>
                                    <td colspan='2' style='background:#1b0a0a; font-weight:bold; border-bottom:1px solid #ff2a2a; color:#ff5c5c;'>Item Details</td>
                                </tr>
                                <tr><td style='width:200px; font-weight:bold; color:#ff5c5c;'>Name</td><td>{Safe(data.ItemName!)}</td></tr>
                                <tr><td style='font-weight:bold; color:#ff5c5c;'>Description</td><td>{Safe(data.ItemDescription!)}</td></tr>
                                <tr><td style='font-weight:bold; color:#ff5c5c;'>Quantity</td><td>{data.Quantity}</td></tr>
                                <tr><td style='font-weight:bold; color:#ff5c5c;'>Price</td><td>₹ {data.Price:N2}</td></tr>
                                <tr><td style='font-weight:bold; color:#ff5c5c;'>Status</td><td>{Safe(data.Status!)}</td></tr>
                                <tr><td style='font-weight:bold; color:#ff5c5c;'>Urgent</td><td>{Safe(data.Urgent!)}</td></tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td style='padding:20px;'>
                            <table width='100%' cellpadding='8' cellspacing='0' style='border:1px solid #ff2a2a; background:#000000; border-radius:6px;'>
                                <tr>
                                    <td colspan='2' style='background:#1b0a0a; font-weight:bold; border-bottom:1px solid #ff2a2a; color:#ff5c5c;'>Requester Info</td>
                                </tr>
                                <tr><td style='font-weight:bold; color:#ff5c5c;'>Requested By</td><td>{Safe(data.RequestorName!)}</td></tr>
                                <tr><td style='font-weight:bold; color:#ff5c5c;'>Role</td><td>{Safe(data.RequestorRole!)}</td></tr>
                                <tr><td style='font-weight:bold; color:#ff5c5c;'>Requested On</td><td>{data.RequestedDate?.ToString("dd-MMM-yyyy")}</td></tr>
                            </table>
                        </td>
                    </tr>
                </table>";
        }

        private string BuildReceiptHtml(ApprovalItemReceipt data)
        {
            string FormatDate(DateTime? dt) => dt?.ToString("dd-MMM-yyyy hh:mm tt") ?? "-";
            string Safe(object o) => o?.ToString() ?? "-";

            return $@"
            <table width='100%' cellpadding='0' cellspacing='0' style='font-family:Arial, sans-serif; border-collapse:collapse;'>
                <tr>
                    <td style='padding:20px; background:#f5f5f5; border-bottom:2px solid #ddd;'>
                        <h2 style='margin:0; font-size:22px;'>Purchase Receipt</h2>
                        <p style='margin:5px 0 0; color:#555;'>Approval Request Summary</p>
                    </td>
                </tr>

                <tr>
                    <td style='padding:20px;'>
                        <table width='100%' cellpadding='8' cellspacing='0' style='border:1px solid #ddd; background:#fff; border-radius:6px;'>
                            <tr>
                                <td colspan='2' style='background:#eee; font-weight:bold; border-bottom:1px solid #ddd;'>Supplier Details</td>
                            </tr>
                            <tr><td style='width:200px; font-weight:bold;'>Supplier Name</td><td>{Safe(data.SupplierName!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Supplier Email</td><td>{Safe(data.SupplierEmail!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Supplier Phone</td><td>{Safe(data.SupplierPhone!)}</td></tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td style='padding:20px;'>
                        <table width='100%' cellpadding='8' cellspacing='0' style='border:1px solid #ddd; background:#fff; border-radius:6px;'>
                            <tr>
                                <td colspan='2' style='background:#eee; font-weight:bold; border-bottom:1px solid #ddd;'>Item Details</td>
                            </tr>
                            <tr><td style='font-weight:bold;'>Name</td><td>{Safe(data.ItemName!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Description</td><td>{Safe(data.ItemDescription!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Quantity</td><td>{data.Quantity}</td></tr>
                            <tr><td style='font-weight:bold;'>UoM</td><td>{Safe(data.UOM!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Price</td><td>₹ {data.Price:N2}</td></tr>
                            <tr><td style='font-weight:bold;'>Original Price</td><td>₹ {data.OriginalPrice:N2}</td></tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td style='padding:20px;'>
                        <table width='100%' cellpadding='8' cellspacing='0' style='border:1px solid #ddd; background:#fff; border-radius:6px;'>
                            <tr>
                                <td colspan='2' style='background:#eee; font-weight:bold; border-bottom:1px solid #ddd;'>Requester Info</td>
                            </tr>
                            <tr><td style='font-weight:bold;'>Requested By</td><td>{Safe(data.RequestorName!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Requested Role</td><td>{Safe(data.RequestorRole!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Requested Date</td><td>{FormatDate(data.RequestedDate)}</td></tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td style='padding:20px;'>
                        <table width='100%' cellpadding='8' cellspacing='0' style='border:1px solid #ddd; background:#fff; border-radius:6px;'>
                            <tr>
                                <td colspan='2' style='background:#eee; font-weight:bold; border-bottom:1px solid #ddd;'>Approval Info</td>
                            </tr>
                            <tr><td style='font-weight:bold;'>Approved By</td><td>{Safe(data.ApproverName!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Approver Role</td><td>{Safe(data.ApproverRole!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Approval Date</td><td>{DateTime.Now:dd-MMM-yyyy hh:mm tt}</td></tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td style='padding:20px;'>
                        <table width='100%' cellpadding='8' cellspacing='0' style='border:1px solid #ddd; background:#fff; border-radius:6px;'>
                            <tr>
                                <td colspan='2' style='background:#eee; font-weight:bold; border-bottom:1px solid #ddd;'>Delivery Info</td>
                            </tr>
                            <tr><td style='font-weight:bold;'>Delivery Address</td><td>{Safe(data.DeliveryAddress!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Delivery Location</td><td>{Safe(data.DeliveryLocation!)}</td></tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td style='padding:20px;'>
                        <table width='100%' cellpadding='8' cellspacing='0' style='border:1px solid #ddd; background:#fff; border-radius:6px;'>
                            <tr>
                                <td colspan='2' style='background:#eee; font-weight:bold; border-bottom:1px solid #ddd;'>Other Details</td>
                            </tr>
                            <tr><td style='font-weight:bold;'>Urgent</td><td>{Safe(data.Urgent!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Category</td><td>{Safe(data.CategoryName!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Line Type</td><td>{Safe(data.LineTypeName!)}</td></tr>
                            <tr><td style='font-weight:bold;'>Status</td><td>{Safe(data.Status!)}</td></tr>
                        </table>
                    </td>
                </tr>

            </table>";
        }
    }
}
