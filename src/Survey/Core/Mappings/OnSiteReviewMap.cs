using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class OnSiteReviewMap : EntityTypeConfiguration<OnSiteReview>
    {
        public OnSiteReviewMap()
        {
            // Primary Key
            this.HasKey(t => t.OnsiteReviewid);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ONSITE_REVIEW");
            this.Property(t => t.OnsiteReviewid).HasColumnName("ONSITE_REVIEW_ID");
            this.Property(t => t.Name).HasColumnName("ONSITE_REVIEW_NAME");
            this.Property(t => t.ReviewDate).HasColumnName("ONSITE_REVIEW_DATE");
            this.Property(t => t.Status).HasColumnName("ONSITE_REVIEW_STATUS");
            this.Property(t => t.Created).HasColumnName("CREATE_DATE");
            this.Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");
            this.Property(t => t.Modified).HasColumnName("MOD_DATE");
            this.Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");
        }
    }
}