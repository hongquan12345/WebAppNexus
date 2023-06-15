using NexusApp.Areas.Survey.Models;

namespace NexusApp.Areas.Financial.Reposetory.Survey
{
    public interface ISurveyReposetory
    {
        Task<List<SurveyModel>> GetAllSurvey();
        Task<SurveyModel> GetSurveyById(int id);
        Task UpdateSurvey(SurveyModel surveyform);
        Task DeleteSurvey(int id);

    }
}
