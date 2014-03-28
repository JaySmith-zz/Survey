using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionById : Scalar<Question>
    {
        public QuestionById(int questionId)
        {
            ContextQuery = c => c.AsQueryable<Question>().FirstOrDefault(x => x.QuestionId == questionId);
        }
    }
}