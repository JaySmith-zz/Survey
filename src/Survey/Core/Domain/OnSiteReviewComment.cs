namespace Survey.Core.Domain
{
    public class OnSiteReviewComment : ModelBase
    {
        public int OnSiteReviewCommentId { get; set; }
        public int OnSiteReviewId { get; set; }
        public int QuestionId { get; set; }
        public string Comment { get; set; }
        public virtual OnSiteReview OnSiteReview { get; set; }
        public virtual Question Question { get; set; }
    }
}