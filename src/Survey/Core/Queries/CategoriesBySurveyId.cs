using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class CategoriesBySurveyId : Query<Category>
    {
        public CategoriesBySurveyId(int surveyId)
        {
            ContextQuery = c => c.AsQueryable<Category>().Where(x => x.SurveyId == surveyId).OrderBy(x => x.DisplayOrder);
        }
    }
}