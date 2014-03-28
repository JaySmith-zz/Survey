using System.Collections.Generic;

namespace Survey.Core.Domain
{
    public class Question : ModelBase
    {
       public Question()
        {
            this.AnswerChoices = new List<AnswerChoice>();
            this.OnSiteReviewComments = new List<OnSiteReviewComment>();
            this.Responses = new List<Response>();
        }

        public int QuestionId { get; set; }
        public int CategoryId { get; set; }
        public string QuestionCode { get; set; }
        public string Status { get; set; }
        public string DisplayText { get; set; }
        public string InputType { get; set; }
        public bool IsKeyQuestion { get; set; }
        public bool IsRequired { get; set; }
        public int DisplayOrder { get; set; }
        public string DisplayLogic { get; set; }
        public virtual ICollection<AnswerChoice> AnswerChoices { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<OnSiteReviewComment> OnSiteReviewComments { get; set; }
        public virtual ICollection<Response> Responses { get; set; }

        public bool IsTextBased
        {
            get { return InputType == QuestionInputType.Date || InputType == QuestionInputType.Number || InputType == QuestionInputType.Text; }
        }

        public bool IsMultiSelect
        {
            get { return InputType == QuestionInputType.Checkbox || InputType == QuestionInputType.RadioButton; }
        }
    }

    public class QuestionStatus
    {
        public static string Active = "Active";
        public static string InActive = "InActive";
        public static string Disabled = "Disabled";
    }

    public class QuestionInputType
    {
        public const string Text = "Text";
        public const string Checkbox = "Checkbox";
        public const string RadioButton = "RadioButton";
        public const string Dropdown = "Dropdown";
        public const string Date = "Date";
        public const string Number = "Number";
    }
}