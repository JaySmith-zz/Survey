using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionnairesCompletedBySurveyId : Query<Questionnaire>
    {
        public QuestionnairesCompletedBySurveyId(int surveyId)
        {
            ContextQuery = c => c.AsQueryable<Questionnaire>()
                .Where(x => x.SurveyId == surveyId)
                .Where(x => x.Status == QuestionnaireStatus.Complete);
        }
    }
}