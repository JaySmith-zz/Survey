using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Survey.ViewModels
{
    public class HomeIndexViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public List<SelectListItem> StatusSelectListItems { get; set; }
        public string Owner { get; set; }
        public string QuestionnareName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public string PersonnelAreaNumber { get; set; }

        public string SurveyName { get; set; }

        public string UserId { get; set; }

        public int OwnerCount { get; set; }
    }
}