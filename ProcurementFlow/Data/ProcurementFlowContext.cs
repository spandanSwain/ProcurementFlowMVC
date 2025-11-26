using ProcurementFlow.Models.Login;
using Microsoft.EntityFrameworkCore;
using ProcurementFlow.Models.Common_Pages;
using ProcurementFlow.Models.Item_DropDown;
using ProcurementFlow.Models.Requisition;
using ProcurementFlow.Models.CatalogItems;
using ProcurementFlow.Models.TranscationConsole;
using ProcurementFlow.Models.PurchaseOrder;
using ProcurementFlow.Models.ConfigItems;
using ProcurementFlow.Models.SummaryDashboards;
using ProcurementFlow.Models.Statistics;

namespace ProcurementFlow.Data
{
    public class ProcurementFlowContext: DbContext
    {
        public ProcurementFlowContext (DbContextOptions<ProcurementFlowContext> options): base (options) { }
        public DbSet<ProcurementFlow.Models.Login.Users> Users { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Register Login SPs
            modelBuilder.Entity<LoginResult>().HasNoKey(); // AUTH
            modelBuilder.Entity<EmailConfig>().HasNoKey(); // Email Config
            // Register Catalog/ Non-Catalog Items ~ Items SPs
            modelBuilder.Entity<UOM>().HasNoKey(); // structure - DAPPER
            modelBuilder.Entity<ItemViewModel>().HasNoKey(); // ViewModel
            modelBuilder.Entity<LineTypes>().HasNoKey(); // structure - DAPPER
            modelBuilder.Entity<Categories>().HasNoKey(); // structure - DAPPER
            modelBuilder.Entity<CatalogItems>().HasNoKey(); // select catalog item list's structure
            modelBuilder.Entity<NonCatalogDisplayItems>().HasNoKey(); // select non-catalog item list's structure
            // Register Requisition SPs
            modelBuilder.Entity<RequisitionViewModel>().HasNoKey(); // ViewModel
            modelBuilder.Entity<ItemDDL>().HasNoKey(); // selects dropdown values - DAPPER
            modelBuilder.Entity<ItemDetails>().HasNoKey(); // selects value for API field population
            modelBuilder.Entity<RequisitionLineItems>().HasNoKey(); // selects value for API field population
            modelBuilder.Entity<RequisitionOverview>().HasNoKey(); // stores dashboard values for requisition - DAPPER
            modelBuilder.Entity<RequisitionListItems>().HasNoKey(); // stores the small requistion table items - DAPPER
            // Register Transaction Console SPs
            modelBuilder.Entity<TransactionConsoleViewModel>().HasNoKey(); // ViewModel
            modelBuilder.Entity<RequisitionTransactions>().HasNoKey(); // used to form the structure  for it's VM
            // Register Purchase Order SPs
            modelBuilder.Entity<PendingRequisitions>().HasNoKey(); // structure purchase orders
            modelBuilder.Entity<ApprovalItemReceipt>().HasNoKey(); // Approver Details for Mail DTO
            modelBuilder.Entity<RejectionItemReceipt>().HasNoKey(); // Rejector Details for Mail DTO
            // Register Summary Dashboard SPs
            modelBuilder.Entity<KPICards>().HasNoKey(); // KPI Cards DTO
            modelBuilder.Entity<CategorySpend>().HasNoKey(); // Spending per category DTO
            modelBuilder.Entity<SummaryDashboardViewModel>().HasNoKey(); // Summary Dashboard ViewModel
            modelBuilder.Entity<ManagerApproveRejectBarChart>().HasNoKey(); // Manager Stats DTO
            // Register Statistics SPs
            modelBuilder.Entity<TopManagers>().HasNoKey(); // Top Managers DTO
            modelBuilder.Entity<MonthlyTrend>().HasNoKey(); // Monthly Trend DTO
            modelBuilder.Entity<TotalKPIStats>().HasNoKey(); // Statistics KPI DTO
            modelBuilder.Entity<TopRequisitions>().HasNoKey(); // Top 3 Requisitions DTO
            modelBuilder.Entity<StatisticsViewModel>().HasNoKey(); // Statistics View Model
            modelBuilder.Entity<SpendPerCategory>().HasNoKey(); // Spending per category DTO
            modelBuilder.Entity<PurchaseOrderPipeline>().HasNoKey(); // Purchase Order Pipeline DTO
        }
    }
}
