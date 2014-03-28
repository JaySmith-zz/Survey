using System;

namespace Survey.Core.Domain
{
    public class ResponseAudit
    {

        public ResponseAudit()
        {
            Created = DateTime.Now;
        }

        public int ResponseAuditId { get; set; }
        public int ResponseId { get; set; }
        public string OldResponseValue { get; set; }
        public string NewResponseValue { get; set; }
        public string OldIsSelectedValue { get; set; }
        public string NewIsSelectedValue { get; set; }
        public virtual Response Response { get; set; }

        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
    }
}