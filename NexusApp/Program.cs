using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NexusApp.Areas.Employee.Repository;

    using Microsoft.EntityFrameworkCore;

using NexusApp.Areas.Financial.Reposetory.Account;
using NexusApp.Areas.Financial.Reposetory.Connect;
using NexusApp.Areas.Financial.Reposetory.Customer;
using NexusApp.Areas.Financial.Reposetory.Guarantee;
using NexusApp.Areas.Financial.Reposetory.Install;
using NexusApp.Areas.Financial.Reposetory.Order;
using NexusApp.Areas.Financial.Reposetory.Payment;
using NexusApp.Areas.Financial.Reposetory.Service;
using NexusApp.Areas.Financial.Reposetory.ServiceConnection;
using NexusApp.Areas.Financial.Reposetory.ServiceSub;
using NexusApp.Areas.Financial.Reposetory.Survey;
using NexusApp.Areas.RetailShop.Repository;

using NexusApp.Areas.Storage.Repository.Equipment;
using NexusApp.Areas.Storage.Repository.Storage;
using NexusApp.Areas.Storage.Repository.Vendor;
using NexusApp.Areas.Storage.Repository.VendorEquipment;

using NexusApp.Data;
using NexusApp.MailForm;
using NexusApp.Repository;
using NexusApp.Services;
using NNexusApp.Areas.Financial.Reposetory.ServiceConnection;
using NNexusApp.Areas.Financial.Reposetory.ServiceSub;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
// configure connection
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
    options.UseSqlServer(connection);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
// Add services to the container.

// Configure Repository
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<MailUtils>();

//eFinancial
builder.Services.AddScoped<IPaymentReposetory, PaymentImple>();
builder.Services.AddScoped<IServiceRepository, ServiceImp>();
builder.Services.AddScoped<ISubServiceRepository, SubServiceImp>();
builder.Services.AddScoped<IServiceConnectionRepository, ServiceConnectImp>();
builder.Services.AddScoped<ICustomerRepository, CustomerServiceImp>();
builder.Services.AddScoped<ISurveyReposetory, Surveyimplement>();
builder.Services.AddScoped<InstallReposetory, Installimp>();
builder.Services.AddScoped<IPaymentReposetory,PaymentImple>();
builder.Services.AddScoped<IGuaranteeReposetory,GuaranteeImplement>();
builder.Services.AddScoped<IOrderReposetory,OrderImplement>();
builder.Services.AddScoped<IConnectionReposetory, ConnectinImp>();
//Employee
builder.Services.AddScoped<IEmployeeRepository, EmployeeImp>();
//RetailShop
builder.Services.AddScoped<IRetailShopRepository, RetailShopImp>();
//Storage
builder.Services.AddScoped<IStorageRepository, StorageImp>();
//Vendor
builder.Services.AddScoped<IVendorRepository, VendorImp>();
//VendorEquipment
builder.Services.AddScoped<IVendorEquipmentRepository, VendorEquipmentImp>();
//Equipment
builder.Services.AddScoped<IEquipmentRepository, EquipmentImp>();
builder.Services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
builder.Services.AddScoped<IAccountReposetory, AccountImplent>();

builder.Services.AddTransient<EmailSender>();

// Configure Auto Mapper
builder.Services.AddAutoMapper(typeof(Program));
// Configure Session

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(5);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});
var app = builder.Build();
app.UseCors();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
     name: "Admin",
     pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
   );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
