using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NexusApp.Areas.Survey.Models;
using NexusApp.Data;
using static NexusApp.ModelDTOs.DashboardViewModel;

namespace NexusApp.Areas.Financial.Reposetory.Guarantee
{
    public class GuaranteeImplement : IGuaranteeReposetory
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GuaranteeImplement(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
        }
        public class GuaranteeExceptio : Exception{
            public GuaranteeExceptio(string message) : base(message) { }
        }
        public async Task CreateGuarantee(GuaranteeModel guaranteeform)
        {
            if (guaranteeform != null)
            {
                var guaModel = new GuaranteeModel();
                guaModel.IsDeposit = guaranteeform.IsDeposit;
                guaModel.Amount = guaranteeform.Amount;
                guaModel.SendMail = false;
                guaModel.Note = guaranteeform.Note;
                guaModel.DepositedDate = DateTime.Now;
                guaModel.CreatedDate = DateTime.Now;   
                guaModel.UpdatedDate = guaranteeform.UpdatedDate;
                context.GuaranteeModels.Add(guaModel);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new GuaranteeExceptio("Can not Add  Guarantee ");
            }
        }

        public async Task<List<GuaranteeModel>> GetAllGuarantee()
        {  
            var gur = await context.GuaranteeModels
                .Include(S => S.surveyModel).ThenInclude(SC => SC.Customer)
                .Include(S => S.surveyModel).ThenInclude(SE =>SE.Employee).ToListAsync();
            if(gur != null)
            {
                return gur;
            }
            else
            {
                throw new GuaranteeExceptio("Can not Get List Guarantee ");
            }
        }

        public Task<GuaranteeModel> GetGuaranteeById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateGuarantee(GuaranteeModel guaranteeform)
        {
            var gua =  await context.GuaranteeModels
                .Include(s=>s.surveyModel).ThenInclude(si=>si.Installs)
                .Include(s => s.surveyModel).ThenInclude(e=>e.Employee)
                .Include(s => s.surveyModel).ThenInclude(c=>c.Customer)
                .Include(s => s.surveyModel).ThenInclude(i=>i.Installs)
                .SingleOrDefaultAsync(x=>x.GuaranteeId == guaranteeform.GuaranteeId);
            if(gua != null)
            {              
                gua.IsDeposit = guaranteeform.IsDeposit;
                gua.Note = guaranteeform.Note;
                gua.UpdatedDate = DateTime.Now;
                gua.Amount = guaranteeform.Amount;
                gua.SendMail = guaranteeform.SendMail;
                if(gua.IsDeposit ==true)
                {
                    gua.DepositedDate = DateTime.Now;
                }
                context.GuaranteeModels.Update(gua);
                var result = await context.SaveChangesAsync();
                var transaction = new TransactionViewModel
                {
                    Time = DateTime.Now,
                    Description = "Guarantee : " + guaranteeform.GuaranteeId + "Update Status",
                    Type = "NewSale"
                };
                var transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                var transactions = string.IsNullOrEmpty(transactionsJson) ?
                    new List<TransactionViewModel>() :
                    JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                transactions.Add(transaction);
                httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));
                if (result > 0 &&  gua.IsDeposit ==true &&  gua.surveyModel !=null)
                {
                    var existingModel = await context.installModels.FirstOrDefaultAsync(g => g.SurveyRefId == gua.surveyModel.SurveyId);
                    if (existingModel == null)
                    {
                        var model = new InstallModel();
                        model.CreatedDate = DateTime.Now;
                        model.InstalledDate = DateTime.Now;
                        model.FinishedDate = DateTime.Now.AddDays(2);
                        model.UpdatedDate = DateTime.Now;
                        model.Surveys = gua.surveyModel;
                        model.Status = "Pending";
                        model.SurveyRefId = gua.SurveyRefId;
                        context.installModels.Add(model);
                        await context.SaveChangesAsync();
                        transaction = new TransactionViewModel
                        {
                            Time = DateTime.Now,
                            Description = "InstallForm  of Customer  Name   : " + gua.surveyModel.Customer.Name + "Update Status",
                            Type = "PaymentMade"
                        };
                        transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                        transactions = string.IsNullOrEmpty(transactionsJson) ?
                            new List<TransactionViewModel>() :
                            JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                        transactions.Add(transaction);
                    }
                }
                if(result > 0 && gua.IsDeposit != true)
                {
                    if(gua.surveyModel.Installs !=null)
                    {
                        var InsexistingModel = context.installModels.Find(gua.surveyModel.Installs.InstallId);
                        if (InsexistingModel != null)
                        {
                            context.installModels.Remove(InsexistingModel);
                            await context.SaveChangesAsync();
                        }
                    }          
                }
            }
            else
            {
                throw new GuaranteeExceptio("Can not Update Guarantee with ID :  "+ guaranteeform.GuaranteeId);
            }
        }
    }
}
