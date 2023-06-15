using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NexusApp.Areas.Storage.Models;
using NexusApp.Areas.Survey.Models;
using NexusApp.Data;
using System.Globalization;
using static NexusApp.ModelDTOs.DashboardViewModel;

namespace NexusApp.Areas.Financial.Reposetory.Survey
{
    public class Surveyimplement : ISurveyReposetory
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public Surveyimplement(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor)
        {
           context = _context;
           httpContextAccessor = _httpContextAccessor;

        }
        public class SurveyException : Exception
        {
            public SurveyException(string message) : base(message) { }
        }
        public async Task<List<SurveyModel>> GetAllSurvey()
        {
            var suy = await context.surveyModels.Include(s => s.Employee).Include(a => a.Customer).ToListAsync();
            if (suy !=null)
            {
                return suy;
            }
            else
            {
                throw new SurveyException("Can not find any Survey ");
            }
        }
        public async Task<SurveyModel> GetSurveyById(int id)
        {
            var suy = await context.surveyModels.FindAsync(id);
            if (suy != null)
            {
                return suy;
            }
            else
            {
                throw new SurveyException("Can not get Survey by this ID");
            }
        }

        public async Task UpdateSurvey(SurveyModel surveyform)
        {           
            var survey = await context.surveyModels.Include(c =>c.Customer)
                .ThenInclude(cs=>cs.Services)
                .ThenInclude(sb=>sb.SubServiceConnections)
                .ThenInclude(sc=>sc.ServiceConnections)
                .SingleOrDefaultAsync(a=>a.SurveyId == surveyform.SurveyId);
            if (survey != null)
            {
                survey.CustomerRefId = surveyform.CustomerRefId;
                survey.Descriptiontest = surveyform.Descriptiontest;
                survey.EmployeeRefId = surveyform.EmployeeRefId;
                survey.IsEquipment = surveyform.IsEquipment;
                survey.IsSupportInternet = surveyform.IsSupportInternet;
                survey.ServeyName = surveyform.ServeyName;
                survey.Status = surveyform.Status;
                survey.UpdatedDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                context.surveyModels.Update(survey);
                var result = await context.SaveChangesAsync();
                if (result > 0  && survey.Status == "Completed" && survey.IsSupportInternet == true)
                {
                    var existingModel = context.GuaranteeModels.FirstOrDefault(g => g.SurveyRefId == survey.SurveyId);

                    if (existingModel == null)
                    {
                        var newModel = new GuaranteeModel();
                        newModel.IsDeposit = false;
                        newModel.SendMail = false;
                        newModel.Amount = survey.Customer.Services.SubServiceConnections.ServiceConnections.Deposit;
                        newModel.Note = "";
                        newModel.DepositedDate = DateTime.Now;
                        newModel.CreatedDate = DateTime.Now;
                        newModel.SurveyRefId = survey.SurveyId;
                        newModel.surveyModel = survey;
                        context.GuaranteeModels.Add(newModel);
                    }
                    else
                    {
                        existingModel.IsDeposit = false;
                        existingModel.SendMail = false;
                        existingModel.Amount = survey.Customer.Services.SubServiceConnections.ServiceConnections.Deposit;
                        existingModel.Note = "";
                        existingModel.DepositedDate = DateTime.Now;
                        existingModel.CreatedDate = DateTime.Now;
                        existingModel.surveyModel = survey;
                        context.GuaranteeModels.Update(existingModel);
                    }
                    await context.SaveChangesAsync();
                    var transaction = new TransactionViewModel
                    {
                        Time = DateTime.Now,
                        Description = "Survey : " + surveyform.ServeyName + "Update Status",
                        Type = "NewSale"
                    };
                    var transactionsJson = httpContextAccessor.HttpContext.Session.GetString("Transactions");
                    var transactions = string.IsNullOrEmpty(transactionsJson) ?
                        new List<TransactionViewModel>() :
                        JsonConvert.DeserializeObject<List<TransactionViewModel>>(transactionsJson);
                    transactions.Add(transaction);
                    httpContextAccessor.HttpContext.Session.SetString("Transactions", JsonConvert.SerializeObject(transactions));
                } 
               if( result > 0 && survey.Status != "Completed")
               {
                  var existingModel = context.GuaranteeModels.FirstOrDefault(g => g.SurveyRefId == survey.SurveyId);
                  if (existingModel != null)
                  {
                        context.GuaranteeModels.Remove(existingModel);
                        await context.SaveChangesAsync();
                  }                                            
                }
            }
            else
            {
                throw new SurveyException("Can not Update Survey with this ID");
            }
        }

        public async Task DeleteSurvey(int id)
        {
            var suy = await context.surveyModels.Include(e => e.Employee).Include(i => i.Installs).SingleOrDefaultAsync(c=>c.SurveyId == id);

            if (suy != null)
            {
                var installs = await context.installModels.Where(i => i.SurveyRefId == suy.SurveyId).ToListAsync();
                if(installs.Count > 0)
                {
                    context.installModels.RemoveRange(installs);
                }  
                var guarantees = await context.GuaranteeModels.Where(g => g.SurveyRefId == suy.SurveyId).ToListAsync();
                if(guarantees.Count > 0)
                {
                   context.GuaranteeModels.RemoveRange(guarantees);
                }          
                context.surveyModels.Remove(suy);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new SurveyException("Can not Delete Survey with this ID");
            }
        }
    }
}
