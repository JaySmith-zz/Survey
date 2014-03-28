using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionnaireByOwnerAndStatus : Query<Questionnaire>
    {
        public QuestionnaireByOwnerAndStatus(string userId, string status)
        {
            ContextQuery = c => c.AsQueryable<Questionnaire>()
                                 .Where(x => x.User.Id == userId && x.Status != status);
        }
    }
}