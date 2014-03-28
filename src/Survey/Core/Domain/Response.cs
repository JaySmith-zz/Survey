using System.Collections.Generic;

namespace Survey.Core.Domain
{
    public class Response : ModelBase
    {
        public Response()
        {
            ResponseAudits = new List<ResponseAudit>();
        }

        public int ResponseId { get; set; }
        public int QuestionnaireId { get; set; }
        public int QuestionId { get; set; }
        public string IsSelected { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public virtual Question Question { get; set; }
        public virtual Questionnaire Questionnaire { get; set; }
        public virtual ICollection<ResponseAudit> ResponseAudits { get; set; }
    }
}