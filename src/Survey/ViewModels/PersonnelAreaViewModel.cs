using System;
using System.ComponentModel;

namespace Survey.ViewModels
{
    public class PersonnelAreaViewModel
    {
        [DisplayName(@"Number")]
        public string Id { get; set; }

        [DisplayName(@"Name")]
        public string Name { get; set; }

        [DisplayName(@"Owner (domain\username)")]
        public string Owner { get; set; }

        [DisplayName(@"Modified")]
        public DateTime Modified { get; set; }

        [DisplayName(@"Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName(@"Created")]
        public DateTime Created { get; set; }

        [DisplayName(@"Created By")]
        public string CreatedBy { get; set; }

        [DisplayName(@"Create Questionnaire for Personnel Area")]
        public bool AutoCreateQuestionnaire { get; set; }
    }
}