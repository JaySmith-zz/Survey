using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class CategoryQuestionsByCategory : Query<Category>
    {
        public CategoryQuestionsByCategory(int surveyId, int categoryId)
        {
            ContextQuery = c => c.AsQueryable<Category>()
                                 .Where(x => x.Survey.SurveyId == surveyId)
                                 .Where(x => x.CategoryId == categoryId);
        }
    }
}