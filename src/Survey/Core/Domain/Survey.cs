using System.Collections.Generic;

namespace Survey.Core.Domain
{
    public class Survey : ModelBase
    {
        public Survey()
        {
            Categories = new List<Category>();
            Questionnaires = new List<Questionnaire>();
        }

        public int SurveyId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public bool IsDefaultSurvey { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Questionnaire> Questionnaires { get; set; }
    }

    public class SurveyStatus
    {
        public const string Active = "Active";
        public const string InActive = "InActive";
    }
}