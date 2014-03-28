using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionByQuestionCode : Scalar<Question>
    {
        public QuestionByQuestionCode(string questionCode)
        {
            ContextQuery = c => c.AsQueryable<Question>().FirstOrDefault(x => x.QuestionCode == questionCode);
        }
    }
}