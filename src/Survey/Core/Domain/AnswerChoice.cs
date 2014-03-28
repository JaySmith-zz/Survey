namespace Survey.Core.Domain
{
    public class AnswerChoice : ModelBase
    {
        public int AnswerChoiceId { get; set; }
        public int QuestionId { get; set; }
        public string DisplayText { get; set; }
        public virtual Question Question { get; set; }
    }
}