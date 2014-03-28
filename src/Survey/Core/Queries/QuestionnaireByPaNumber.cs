using System.Linq;
using Highway.Data;
using Survey.Core.Domain;

namespace Survey.Core.Queries
{
    public class QuestionnaireByPaNumber : Query<Questionnaire>
    {
        public QuestionnaireByPaNumber(string paNumber)
        {
            ContextQuery = c => c.AsQueryable<Questionnaire>().Where(x => x.PersonnelAreaNumber == paNumber);
        }
    }
}