using System.Collections.Generic;
using System.Web.Mvc;
using Survey.Core.Domain;

namespace Survey.ViewModels
{
    public class OnSiteReviewIndexViewModel
    {
        public IEnumerable<SelectListItem> Items { get; set; }
        public List<OnSiteReviewRow> OnSiteReviews { get; set; }

        public OnSiteReviewIndexViewModel()
        {
            OnSiteReviews = new List<OnSiteReviewRow>();
        }

        public class OnSiteReviewRow : OnSiteReview
        {
            public OnSiteReviewRow(OnSiteReview onSiteReview)
            {
                base.OnsiteReviewid = onSiteReview.OnsiteReviewid;
                base.Name = onSiteReview.Name;
                base.ReviewDate = onSiteReview.ReviewDate;
                base.Status = onSiteReview.Status;
                base.Questionnaires = onSiteReview.Questionnaires;
            }

            public string ExportLink { get { return "Excel"; } }
            public string QuestionnaireList { get { return BuildList(); } }

            private string BuildList()
            {
                return "AM01<br>ABCD<br>EFGH";
            }
        }
    }
}