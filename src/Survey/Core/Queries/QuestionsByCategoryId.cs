using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionsByCategoryId : Query<Question>
    {
        public QuestionsByCategoryId(int categoryId, bool isKeyQuestion)
        {
            if (isKeyQuestion) // Filter on IsKeyQuestion
                ContextQuery = c => c.AsQueryable<Question>()
                         .Where(x => x.CategoryId == categoryId)
                         .Where(x => x.IsKeyQuestion == isKeyQuestion).OrderBy(x => x.DisplayOrder);
            else
                ContextQuery = c => c.AsQueryable<Question>()
                         .Where(x => x.CategoryId == categoryId).OrderBy(x => x.DisplayOrder);
        }
    }
}