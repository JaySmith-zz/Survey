using System.Linq;
using Highway.Data;

namespace Survey.Core.Queries
{
    public class SurveyById : Scalar<Domain.Survey>
    {
        public SurveyById(int surveyId)
        {
            ContextQuery = c => c.AsQueryable<Domain.Survey>().Single(x => x.SurveyId == surveyId);
        }
    }
}