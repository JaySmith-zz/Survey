using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class OnSiteReviewCommentMap : EntityTypeConfiguration<OnSiteReviewComment>
    {
        public OnSiteReviewCommentMap()
        {
            // Primary Key
            HasKey(t => t.OnSiteReviewCommentId);

            // Properties
            Property(t => t.Comment)
                .IsRequired()
                .HasMaxLength(500);

            Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("ONSITE_QUESTION_COMMENT");
            Property(t => t.OnSiteReviewCommentId).HasColumnName("ONSITE_QUESTION_COMMENT_ID");
            Property(t => t.OnSiteReviewId).HasColumnName("ONSITE_REVIEW_ID");
            Property(t => t.QuestionId).HasColumnName("QUESTION_ID");
            Property(t => t.Comment).HasColumnName("COMMENT");
            Property(t => t.Created).HasColumnName("CREATE_DATE");
            Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");
            Property(t => t.Modified).HasColumnName("MOD_DATE");
            Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");

            // Relationships
            HasRequired(t => t.OnSiteReview)
                .WithMany(t => t.Comments)
                .HasForeignKey(d => d.OnSiteReviewId);
            HasRequired(t => t.Question)
                .WithMany(t => t.OnSiteReviewComments)
                .HasForeignKey(d => d.QuestionId);
        }
    }
}