using System.Collections.Generic;
using Survey.Core.Domain;

namespace Survey.Models
{
    public class JsonQuestionResponse
    {
        public int QuestionnaireId { get; set; }
        public int QuestionId { get; set; }
        public ICollection<Response> Responses { get; set; }
    }
}