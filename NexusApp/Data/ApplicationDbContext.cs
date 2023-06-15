using Microsoft.EntityFrameworkCore;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Employee.Models;
using NexusApp.Areas.Financial.Models;
using NexusApp.Areas.RetailShop.Models;
using NexusApp.Areas.ServiceConnection.Models;
using NexusApp.Areas.Storage.Models;
using NexusApp.Areas.Survey.Models;

namespace NexusApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RetailShopModel>(p =>
            {
                p.ToTable("retailshop").HasKey(k => k.RetailShopId);
            });
            modelBuilder.Entity<EmployeeModel>(p =>
            {
                p.ToTable("employee").HasKey(k => k.EmployeeId);
                p.HasOne(b => b.RetailShop)
                .WithMany(s => s.Employees)
                .HasConstraintName("fk_retail_employee")
                .HasForeignKey(m => m.RetaishopRefId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<ServiceConnectionModel>(p =>
            {
                p.ToTable("serviceconnection").HasKey(k => k.ServiceConnectionId);
            });
            modelBuilder.Entity<SubServiceConnectionModel>(p =>
            {
                p.ToTable("subserviceconnection").HasKey(k => k.SubServiceConnectionId);
                p.HasOne(b => b.ServiceConnections)
             .WithMany(s => s.SubServiceConnectionModels)
              .HasConstraintName("fk_serviceconnection_subserviceconnection")
             .HasForeignKey(m => m.ServiceConnectionRefId)
             .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<ServiceModel>(p =>
            {
                p.ToTable("service").HasKey(k => k.ServiceId);
                p.HasOne(b => b.SubServiceConnections)
             .WithMany(s => s.ServiceModels)
              .HasConstraintName("fk_subserviceconnection_service")
             .HasForeignKey(m => m.SubServiceConnectionRefId)
             .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<CustomerModel>(p =>
            {
                p.ToTable("customer").HasKey(k => k.CustomerId);
                p.HasOne(b => b.Services)
                    .WithMany(s => s.Customers)
                     .HasConstraintName("fk_service-customer")
                     .HasForeignKey(k => k.ServiceRefId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<SurveyModel>(p =>
            {
                p.ToTable("survey").HasKey(k => k.SurveyId);
                p.HasOne(b => b.Employee)
                .WithMany(s => s.Surveys)
                .HasConstraintName("fk_survey_employee")
                .HasForeignKey(m => m.EmployeeRefId)
                .OnDelete(DeleteBehavior.Restrict);
                p.HasOne(b => b.Customer)
                .WithMany(s => s.Surveys)
                .HasConstraintName("fk_survey_Customer")
                .HasForeignKey(m => m.CustomerRefId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<StorageModel>(p =>
            {
                p.ToTable("storage").HasKey(k => k.StorageId);
                p.HasOne(b => b.Employee)
                .WithMany(s => s.Storages)
                .HasConstraintName("storage_Employee")
                .HasForeignKey(m => m.EmployeeRefId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<EquipmentModel>(p =>
            {
                p.ToTable("equipment").HasKey(k => k.EquipmentId);
                p.HasOne(b => b.Storage)
                .WithMany(s => s.Equipments)
                .HasConstraintName("fk_storage_equipment")
                .HasForeignKey(m => m.StorageRefId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<VendorModel>(p =>
            {
                p.ToTable("vendor").HasKey(k => k.VendorId);
            });
            modelBuilder.Entity<Vendor_Equipment>(p =>
            {
                p.ToTable("vendor_equipment").HasKey(k => k.Id);
                p.HasOne(b => b.Vendor)
                .WithMany(s => s.Vendor_Equipment)
                .HasConstraintName("fk_vendor_equipment_vendor")
                .HasForeignKey(k => k.VendorRefId).OnDelete(DeleteBehavior.Restrict);
                p.HasOne(b => b.Equipment)
                .WithMany(s => s.Vendor_Equipment)
                .HasConstraintName("fk_vendor_equipment_equipment")
                .HasForeignKey(k => k.EquipmentRefId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<InstallModel>(p =>
            {
                p.ToTable("install").HasKey(k => k.InstallId);
                p.HasOne(b => b.Surveys)
                    .WithOne(s => s.Installs)
                     .HasConstraintName("fk_survey_install")
                     .HasForeignKey<InstallModel>(k => k.SurveyRefId).OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<AccountModel>(p =>
            {
                p.ToTable("accounts").HasKey(k => k.AccountId);
                p.HasOne(b => b.Customer)
                .WithOne(s => s.Accounts)
                .HasForeignKey<AccountModel>(k => k.CustomerRefId)
                .HasConstraintName("fk_accounts_customer") // Provide a different name for the foreign key constraint
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<GuaranteeModel>(p =>
            {
                p.ToTable("guarantees").HasKey(k => k.GuaranteeId);
                p.HasOne(b => b.surveyModel)
                .WithOne(s => s.Guarantees)
                .HasConstraintName("fk_survey_guarantee")
                .HasForeignKey<GuaranteeModel>(k => k.SurveyRefId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<ConnectionModel>(p =>
            {
                p.ToTable("connection").HasKey(k => k.ConnectionID);
                p.HasOne(b => b.Services)
                .WithMany(s => s.Connections)
                .HasConstraintName("fk_Connection")
                .HasForeignKey(k => k.ServiceRefId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<OrderDetailModel>(p =>
            {
                p.ToTable("OrderDetail").HasKey(k => k.OrderDetailId);
                p.HasOne(b => b.Equipments)
                .WithMany(s => s.OrderDetails)
                .HasConstraintName("fk_oderDetail_equipment")
                .HasForeignKey(k => k.EquipmentRefId).OnDelete(DeleteBehavior.Restrict);
                p.HasOne(b => b.Connections)
                .WithMany(s => s.OrderDetails)
                .HasConstraintName("fk_orderDetail_connection")
                .HasForeignKey(k => k.ConnectionRefId).OnDelete(DeleteBehavior.Restrict);
                p.HasOne(b => b.Employees)
                .WithMany(s => s.OrderDetails)
                .HasConstraintName("FK_OrderDetail_Employee")
                .HasForeignKey(k => k.EmployeeRefId).OnDelete(DeleteBehavior.Restrict);
                p.HasOne(b => b.Customers)
                .WithMany(s => s.OrderDetails)
                .HasConstraintName("FK_OrderDetail_Customer")
                .HasForeignKey(k => k.CustomerRefId).OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<PaymentModel>(p =>
            {
                p.ToTable("payments").HasKey(k => k.PaymentId);
                p.HasOne(b => b.OrderDetails)
                .WithMany(s => s.Payments)
                .HasConstraintName("fk_payment_orderDetail")
                .HasForeignKey(k => k.OrderDetailRefId).OnDelete(DeleteBehavior.Restrict);
                p.HasOne(b => b.Guarantees)
                .WithMany(s => s.Payments)
                .HasConstraintName("fk_payment_guarantees")
                .HasForeignKey(k => k.GuaranteeRefId).OnDelete(DeleteBehavior.Restrict);
                p.HasOne(b => b.Accounts)
                .WithMany(s => s.Payments)
                .HasConstraintName("fk_payment_account")
                .HasForeignKey(k => k.AccountRefId).OnDelete(DeleteBehavior.Restrict);


            });


        }
        public DbSet<RetailShopModel> RetailShop { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<CustomerModel> customerModels { get; set; }
        public DbSet<SurveyModel> surveyModels { get; set; }
        public DbSet<StorageModel> storageModels { get; set; }
        public DbSet<EquipmentModel> EquipmentModels { get; set; }
        public DbSet<VendorModel> vendorModels { get; set; }
        public DbSet<Vendor_Equipment> Vendor_Equipments { get; set; }
        public DbSet<InstallModel> installModels { get; set; }
        public DbSet<AccountModel> accountModels { get; set; }
        public DbSet<GuaranteeModel> GuaranteeModels { get; set; }
        public DbSet<ConnectionModel> connectionModels { get; set; }
        public DbSet<OrderDetailModel> orderDetailModels { get; set; }
        public DbSet<PaymentModel> PaymentModels { get; set; }
        public DbSet<ServiceConnectionModel> serviceConnectionModels { get; set; }
        public DbSet<ServiceModel> serviceModels { get; set; }
        public DbSet<SubServiceConnectionModel> subServiceConnectionModels { get; set; }

    }
}
