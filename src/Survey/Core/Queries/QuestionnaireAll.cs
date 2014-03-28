using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionnaireAll : Query<Questionnaire>
    {
        public QuestionnaireAll()
        {
            ContextQuery = c => c.AsQueryable<Questionnaire>();
        }
    }
}