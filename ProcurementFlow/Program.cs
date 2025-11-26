using Microsoft.EntityFrameworkCore;
using ProcurementFlow.Data;
using ProcurementFlow.Models.ConfigItems;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("sqlConn") ?? throw new InvalidOperationException("Connection string 'sqlConn' not found.");
builder.Services.AddDbContext<ProcurementFlowContext>(options => options.UseSqlServer(
    connectionString,
    sqlOptions => sqlOptions.CommandTimeout(200)
));

// Add services to the container.
builder.Services.AddScoped<ProcurementFlow.DataAccess.Login>();
builder.Services.AddScoped<ProcurementFlow.DataAccess.Items>();
builder.Services.AddScoped<ProcurementFlow.DataAccess.Statistics>();
builder.Services.AddScoped<ProcurementFlow.DataAccess.Requisition>();
builder.Services.AddScoped<ProcurementFlow.DataAccess.PurchaseOrder>();
builder.Services.AddScoped<ProcurementFlow.DataAccess.SummaryDashboard>();
builder.Services.AddScoped<ProcurementFlow.DataAccess.TransactionConsole>();

// Add Config Options
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // enable session

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
