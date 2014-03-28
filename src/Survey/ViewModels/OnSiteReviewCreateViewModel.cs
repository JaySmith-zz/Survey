using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Survey.Core.Domain;

namespace Survey.ViewModels
{
    public class OnSiteReviewCreateViewModel
    {
        public IEnumerable<SelectListItem> AvailableList { get; set; }
        public IEnumerable<SelectListItem> SelectedList { get; set; }

        public string[] AvailableValues { get; set; }
        [Required(ErrorMessage = "* At least one questionnaire has to be selected.")]
        public string[] SelectedValues { get; set; }

        [Required(ErrorMessage = "* A name for the On-site Review is required.")]
        [DataType(DataType.Text)]
        public string ReviewName { get; set; }

        [DisplayName("Review Date")]
        [DisplayFormat(NullDisplayText = "", ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "* A valid date for the On-site Review is required.")]
        [DataType(DataType.Date)]
        public DateTime? ReviewDate { get; set; }

        public OnSiteReviewCreateViewModel()
        {
            this.SelectedList = new MultiSelectList(new List<Questionnaire>() { });
        }
    }
}