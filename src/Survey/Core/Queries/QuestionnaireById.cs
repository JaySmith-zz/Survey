using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionnaireById : Scalar<Questionnaire>
    {
        public QuestionnaireById(int questionniareId)
        {
            ContextQuery = c => c.AsQueryable<Questionnaire>().Single(x => x.QuestionnaireId == questionniareId);
        }
    }
}