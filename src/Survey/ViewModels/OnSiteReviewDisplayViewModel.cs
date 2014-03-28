using System.Collections.Generic;
using System.Web.Mvc;
using Survey.Core.Domain;

namespace Survey.ViewModels
{
    public class OnSiteReviewDisplayViewModel
    {
        public IEnumerable<SelectListItem> Items { get; set; }
        public OnSiteReview OnSiteReview { get; set; }

        public OnSiteReviewDisplayViewModel(OnSiteReview onSiteReview)
        {
            OnSiteReview = onSiteReview;
        }
    }
}