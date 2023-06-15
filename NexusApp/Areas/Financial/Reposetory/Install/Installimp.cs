using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NexusApp.Areas.Financial.Models;
using NexusApp.Areas.Survey.Models;
using NexusApp.Data;
using System;
using System.Net;
using System.Text;
using static NexusApp.ModelDTOs.DashboardViewModel;

namespace NexusApp.Areas.Financial.Reposetory.Install
{
    public class Installimp : InstallReposetory
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;

        public Installimp(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, IConfiguration _configuration)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor; 
            configuration = _configuration;
        }
        public class InstallException : Exception
        {
            public InstallException(string message) : base(message) { }
        }
        public async Task<List<InstallModel>> GetAllInstall()
        {
            var ins = await context.installModels.Include(X => X.Surveys).ThenInclude(a => a.Employee).ToListAsync();
            if (ins!=null)
            {
                return ins;
            }
            else
            {
                throw new InstallException("List Install is Empty");
            }
        }
        public async Task<InstallModel> GetInstallById(int id)
        {
            var ins = await context.installModels.FirstOrDefaultAsync(c=>c.InstallId == id);
            if (ins != null)
            {
                return ins;
            }
            else
            {
                throw new InstallException("Can not find Any Install with ID");
            }
        }

        public async Task UpdateInstall(InstallModel installform)
        {
            var insUp = await context.installModels.Include(s => s.Surveys)
                .ThenInclude(sc => sc.Customer).ThenInclude(scc=>scc.Services)
                .ThenInclude(scb=>scb.SubServiceConnections)
                .ThenInclude(scs=>scs.ServiceConnections)
                .Include(s => s.Surveys).ThenInclude(se=>se.Employee)
                .ThenInclude(er=>er.Storages).ThenInclude(r=>r.Equipments).SingleOrDefaultAsync(x => x.InstallId == installform.InstallId);
            if (insUp != null)
            {
                insUp.Status = installform.Status;
                insUp.CreatedDate = installform.CreatedDate;
                insUp.UpdatedDate = installform.UpdatedDate;
                insUp.FinishedDate = installform.FinishedDate;
                context.installModels.Update(insUp);
                var result =  await context.SaveChangesAsync();
                if (result > 0 && insUp.Status == "Completed" && insUp.Surveys.Customer != null)
                {
                    var connectModel = new ConnectionModel();
                    connectModel.Status = "false";
                    connectModel.ConnectionName = insUp.Surveys.Customer.Name + " - " + insUp.Surveys.Customer.Services.SubServiceConnections.ServiceConnections.Name;
                    connectModel.ConnectionType = insUp.Surveys.Customer.Services.SubServiceConnections.ServiceConnections.Name;
                    connectModel.ServiceRefId = insUp.Surveys.Customer.ServiceRefId;
                    connectModel.OrderDate = DateTime.Now;
                    connectModel.UpdatedDate = null;
                    connectModel.CreatedDate = DateTime.Now;
                    context.connectionModels.Add(connectModel);
                    var result2 = await context.SaveChangesAsync();
                    var transaction = new TransactionViewModel
                    {
                        Time = DateTime.Now,
                        Description = "ConnectionName : " + insUp.Surveys.Customer.Name + " - " + insUp.Surveys.Customer.Services.SubServiceConnections.ServiceConnections.Name + "Created",
                        Type = "NewArrival"
                    };
                    var transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                    var transactions = string.IsNullOrEmpty(transactionsJson) ?
                    new List<TransactionViewModel>() :
                    JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                    transactions.Add(transaction);

                    if (result2>0)
                        {
                        var existingoder = await context.orderDetailModels.FirstOrDefaultAsync(g => g.CustomerRefId == insUp.Surveys.CustomerRefId);
                    if (existingoder == null)
                    {

                        var odermodel = new OrderDetailModel();
                        int orderCount = await context.orderDetailModels.CountAsync();
                        string orderSeria = orderCount.ToString().PadLeft(11, '0');
                        odermodel.OrderSeri = insUp.Surveys.Customer.Services.SubServiceConnections.ServiceConnections.Name[0].ToString() + orderSeria;
                        odermodel.CreatedDate = DateTime.Now;
                         
                        odermodel.IsEquipment = insUp.Surveys.IsEquipment;
                        odermodel.Quantity = 0;
                        odermodel.EmployeeRefId = insUp.Surveys.EmployeeRefId;
                        odermodel.CustomerRefId = insUp.Surveys.CustomerRefId;
                        odermodel.ConnectionRefId = connectModel.ConnectionID;
                        if (insUp.Surveys.IsEquipment == true)
                        {
                            odermodel.EquipmentRefId = null;
                        }
                        else
                        {
                            var random = new Random();
                            var storageIndex = random.Next(insUp.Surveys.Employee.Storages.Count);
                            var storage = insUp.Surveys.Employee.Storages.ElementAt(storageIndex);
                            var equipmentIndex = random.Next(storage.Equipments.Count);
                            var equipment = storage.Equipments.ElementAt(equipmentIndex);
                            odermodel.EquipmentRefId = equipment.EquipmentId;
                            odermodel.Quantity = storage.Equipments.Count;
                        }
                        context.orderDetailModels.Add(odermodel);
                        await context.SaveChangesAsync();
                         transaction = new TransactionViewModel
                        {
                            Time = DateTime.Now,
                            Description = "New Order: " + insUp.Surveys.Customer.Services.SubServiceConnections.ServiceConnections.Name[0].ToString() + orderSeria + " Regiser",
                            Type = "PaymentReceived"
                        };
                        transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                        transactions = string.IsNullOrEmpty(transactionsJson) ?
                            new List<TransactionViewModel>() :
                            JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                        transactions.Add(transaction);
                        httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));
                    }
                    else
                    {
                        var odermodel = new OrderDetailModel();
                        odermodel.CreatedDate = DateTime.Now;
                        odermodel.IsEquipment = insUp.Surveys.IsEquipment;
                        odermodel.Quantity = 0;
                        odermodel.EmployeeRefId = insUp.Surveys.EmployeeRefId;
                        odermodel.CustomerRefId = insUp.Surveys.CustomerRefId;
                        odermodel.ConnectionRefId = connectModel.ConnectionID;
                        if (insUp.Surveys.IsEquipment == true)
                        {
                            odermodel.EquipmentRefId = null;
                        }
                        else
                        {
                            var random = new Random();
                            var storageIndex = random.Next(insUp.Surveys.Employee.Storages.Count);
                            var storage = insUp.Surveys.Employee.Storages.ElementAt(storageIndex);
                            var equipmentIndex = random.Next(storage.Equipments.Count);
                            var equipment = storage.Equipments.ElementAt(equipmentIndex);
                            odermodel.EquipmentRefId = equipment.EquipmentId;
                            odermodel.Quantity = storage.Equipments.Count;
                        }
                        context.orderDetailModels.Update(odermodel);
                        context.Entry(odermodel).Property(a => a.OrderSeri).IsModified = false;
                        await context.SaveChangesAsync();
                    }                   
                    }
                }
                if (result > 0 && insUp.Status != "Completed")
                {
                    var existingOder = await context.orderDetailModels.FirstOrDefaultAsync(g => g.CustomerRefId == insUp.Surveys.CustomerRefId);
                    if (existingOder != null)
                    {
                        context.orderDetailModels.Remove(existingOder);
                        await context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                throw new InstallException("Cant Not Update");
            }
        }
    }
}
