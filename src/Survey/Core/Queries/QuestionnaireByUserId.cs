using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionnaireByUserId : Query<Questionnaire>
    {
        public QuestionnaireByUserId(string userId)
        {
            ContextQuery = c => c.AsQueryable<Questionnaire>().Where(x => x.User.Id == userId);
        }
    }
}