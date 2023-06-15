using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusApp.Migrations
{
    /// <inheritdoc />
    public partial class update_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "retailshop",
                columns: table => new
                {
                    RetailShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retailshop", x => x.RetailShopId);
                });

            migrationBuilder.CreateTable(
                name: "serviceconnection",
                columns: table => new
                {
                    ServiceConnectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serviceconnection", x => x.ServiceConnectionId);
                });

            migrationBuilder.CreateTable(
                name: "vendor",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendor", x => x.VendorId);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetaishopRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "fk_retail_employee",
                        column: x => x.RetaishopRefId,
                        principalTable: "retailshop",
                        principalColumn: "RetailShopId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "subserviceconnection",
                columns: table => new
                {
                    SubServiceConnectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceConnectionRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subserviceconnection", x => x.SubServiceConnectionId);
                    table.ForeignKey(
                        name: "fk_serviceconnection_subserviceconnection",
                        column: x => x.ServiceConnectionRefId,
                        principalTable: "serviceconnection",
                        principalColumn: "ServiceConnectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "storage",
                columns: table => new
                {
                    StorageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storage", x => x.StorageId);
                    table.ForeignKey(
                        name: "storage_Employee",
                        column: x => x.EmployeeRefId,
                        principalTable: "employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "service",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServicePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubServiceConnectionRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service", x => x.ServiceId);
                    table.ForeignKey(
                        name: "fk_subserviceconnection_service",
                        column: x => x.SubServiceConnectionRefId,
                        principalTable: "subserviceconnection",
                        principalColumn: "SubServiceConnectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serial = table.Column<int>(type: "int", nullable: false),
                    IsSupportLine = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportInternet = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StorageRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "fk_storage_equipment",
                        column: x => x.StorageRefId,
                        principalTable: "storage",
                        principalColumn: "StorageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "connection",
                columns: table => new
                {
                    ConnectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connection", x => x.ConnectionID);
                    table.ForeignKey(
                        name: "fk_Connection",
                        column: x => x.ServiceRefId,
                        principalTable: "service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "fk_service-customer",
                        column: x => x.ServiceRefId,
                        principalTable: "service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vendor_equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    VendorRefId = table.Column<int>(type: "int", nullable: false),
                    EquipmentRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendor_equipment", x => x.Id);
                    table.ForeignKey(
                        name: "fk_vendor_equipment_equipment",
                        column: x => x.EquipmentRefId,
                        principalTable: "equipment",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_vendor_equipment_vendor",
                        column: x => x.VendorRefId,
                        principalTable: "vendor",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "fk_accounts_customer",
                        column: x => x.CustomerRefId,
                        principalTable: "customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderSeri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEquipment = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EquipmentRefId = table.Column<int>(type: "int", nullable: true),
                    ConnectionRefId = table.Column<int>(type: "int", nullable: false),
                    EmployeeRefId = table.Column<int>(type: "int", nullable: false),
                    CustomerRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Customer",
                        column: x => x.CustomerRefId,
                        principalTable: "customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Employee",
                        column: x => x.EmployeeRefId,
                        principalTable: "employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_oderDetail_equipment",
                        column: x => x.EquipmentRefId,
                        principalTable: "equipment",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_orderDetail_connection",
                        column: x => x.ConnectionRefId,
                        principalTable: "connection",
                        principalColumn: "ConnectionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "survey",
                columns: table => new
                {
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServeyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEquipment = table.Column<bool>(type: "bit", nullable: false),
                    IsSupportInternet = table.Column<bool>(type: "bit", nullable: false),
                    SurveyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descriptiontest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeRefId = table.Column<int>(type: "int", nullable: false),
                    CustomerRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey", x => x.SurveyId);
                    table.ForeignKey(
                        name: "fk_survey_Customer",
                        column: x => x.CustomerRefId,
                        principalTable: "customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_survey_employee",
                        column: x => x.EmployeeRefId,
                        principalTable: "employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "guarantees",
                columns: table => new
                {
                    GuaranteeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeposit = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SendMail = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SurveyRefId = table.Column<int>(type: "int", nullable: false),
                    CustomerModelCustomerId = table.Column<int>(type: "int", nullable: true),
                    EmployeeModelEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_guarantees", x => x.GuaranteeId);
                    table.ForeignKey(
                        name: "FK_guarantees_customer_CustomerModelCustomerId",
                        column: x => x.CustomerModelCustomerId,
                        principalTable: "customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_guarantees_employee_EmployeeModelEmployeeId",
                        column: x => x.EmployeeModelEmployeeId,
                        principalTable: "employee",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "fk_survey_guarantee",
                        column: x => x.SurveyRefId,
                        principalTable: "survey",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "install",
                columns: table => new
                {
                    InstallId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstalledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SurveyRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_install", x => x.InstallId);
                    table.ForeignKey(
                        name: "fk_survey_install",
                        column: x => x.SurveyRefId,
                        principalTable: "survey",
                        principalColumn: "SurveyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SendMail = table.Column<bool>(type: "bit", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderDetailRefId = table.Column<int>(type: "int", nullable: false),
                    GuaranteeRefId = table.Column<int>(type: "int", nullable: false),
                    AccountRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "fk_payment_account",
                        column: x => x.AccountRefId,
                        principalTable: "accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payment_guarantees",
                        column: x => x.GuaranteeRefId,
                        principalTable: "guarantees",
                        principalColumn: "GuaranteeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payment_orderDetail",
                        column: x => x.OrderDetailRefId,
                        principalTable: "OrderDetail",
                        principalColumn: "OrderDetailId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_CustomerRefId",
                table: "accounts",
                column: "CustomerRefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_connection_ServiceRefId",
                table: "connection",
                column: "ServiceRefId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_ServiceRefId",
                table: "customer",
                column: "ServiceRefId");

            migrationBuilder.CreateIndex(
                name: "IX_employee_RetaishopRefId",
                table: "employee",
                column: "RetaishopRefId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_StorageRefId",
                table: "equipment",
                column: "StorageRefId");

            migrationBuilder.CreateIndex(
                name: "IX_guarantees_CustomerModelCustomerId",
                table: "guarantees",
                column: "CustomerModelCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_guarantees_EmployeeModelEmployeeId",
                table: "guarantees",
                column: "EmployeeModelEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_guarantees_SurveyRefId",
                table: "guarantees",
                column: "SurveyRefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_install_SurveyRefId",
                table: "install",
                column: "SurveyRefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ConnectionRefId",
                table: "OrderDetail",
                column: "ConnectionRefId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_CustomerRefId",
                table: "OrderDetail",
                column: "CustomerRefId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_EmployeeRefId",
                table: "OrderDetail",
                column: "EmployeeRefId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_EquipmentRefId",
                table: "OrderDetail",
                column: "EquipmentRefId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_AccountRefId",
                table: "payments",
                column: "AccountRefId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_GuaranteeRefId",
                table: "payments",
                column: "GuaranteeRefId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_OrderDetailRefId",
                table: "payments",
                column: "OrderDetailRefId");

            migrationBuilder.CreateIndex(
                name: "IX_service_SubServiceConnectionRefId",
                table: "service",
                column: "SubServiceConnectionRefId");

            migrationBuilder.CreateIndex(
                name: "IX_storage_EmployeeRefId",
                table: "storage",
                column: "EmployeeRefId");

            migrationBuilder.CreateIndex(
                name: "IX_subserviceconnection_ServiceConnectionRefId",
                table: "subserviceconnection",
                column: "ServiceConnectionRefId");

            migrationBuilder.CreateIndex(
                name: "IX_survey_CustomerRefId",
                table: "survey",
                column: "CustomerRefId");

            migrationBuilder.CreateIndex(
                name: "IX_survey_EmployeeRefId",
                table: "survey",
                column: "EmployeeRefId");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_equipment_EquipmentRefId",
                table: "vendor_equipment",
                column: "EquipmentRefId");

            migrationBuilder.CreateIndex(
                name: "IX_vendor_equipment_VendorRefId",
                table: "vendor_equipment",
                column: "VendorRefId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "install");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "vendor_equipment");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "guarantees");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "vendor");

            migrationBuilder.DropTable(
                name: "survey");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "connection");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "storage");

            migrationBuilder.DropTable(
                name: "service");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "subserviceconnection");

            migrationBuilder.DropTable(
                name: "retailshop");

            migrationBuilder.DropTable(
                name: "serviceconnection");
        }
    }
}
