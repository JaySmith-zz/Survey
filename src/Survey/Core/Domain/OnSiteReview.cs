using System;
using System.Collections.Generic;

namespace Survey.Core.Domain
{
    public class OnSiteReview : ModelBase
    {
        public OnSiteReview()
        {
            Comments = new List<OnSiteReviewComment>();
            Questionnaires = new List<Questionnaire>();
        }

        public int OnsiteReviewid { get; set; }
        public string Name { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Status { get; set; }
        public virtual ICollection<OnSiteReviewComment> Comments { get; set; }
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
    }

    public class OnSiteReviewStatus
    {
        public const string Open = "Open";
        public const string Closed = "Closed";
        public const string Pending = "Pending";
    }
}