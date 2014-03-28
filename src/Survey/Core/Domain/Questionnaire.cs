using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey.Core.Domain
{
    public class Questionnaire : ModelBase
    {
        public Questionnaire()
        {
            Responses = new HashSet<Response>();
            OnsiteReview = new HashSet<OnSiteReview>();
        }

        public int QuestionnaireId { get; set; }
        public int SurveyId { get; set; }
        public string PersonnelAreaNumber { get; set; }
        public int? CurrentCategoryId { get; set; }
        public virtual Category CurrentCategory { get; set; }
        public string Status { get; set; }
        public virtual User User { get; set; }
        public virtual Survey Survey { get; set; }
        public virtual ICollection<Response> Responses { get; set; }
        public virtual ICollection<OnSiteReview> OnsiteReview { get; set; }
        public virtual DateTime? Started { get; set; }
        public virtual string StartedBy { get; set; }

        [NotMapped]
        public string QuestionniareName
        {
            get { return string.Format("{0}", Survey.Name); }
        }
    }

    public class QuestionnaireStatus
    {
        public const string NotStarted = "Not Started";
        public const string InProgress = "In Progress";
        public const string Complete = "Complete";
        public const string Updated = "Updated";
    }
}