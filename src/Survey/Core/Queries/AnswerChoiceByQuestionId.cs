using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class AnswerChoiceByQuestionId : Query<AnswerChoice>
    {
        public AnswerChoiceByQuestionId(int questionId)
        {
            ContextQuery = c => c.AsQueryable<AnswerChoice>().Where(x => x.QuestionId == questionId);
        }
    }
}