using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            HasKey(t => t.CategoryId);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(10);

            Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("CATEGORY");
            Property(t => t.CategoryId).HasColumnName("CATEGORY_ID");
            Property(t => t.SurveyId).HasColumnName("SURVEY_ID");
            Property(t => t.Name).HasColumnName("CATEGORY_NAME");
            Property(t => t.Description).HasColumnName("CATEGORY_DESCR");
            Property(t => t.Status).HasColumnName("CATEGORY_STATUS");
            Property(t => t.DisplayOrder).HasColumnName("CATEGORY_DISPLAY_SEQ");
            Property(t => t.Created).HasColumnName("CREATE_DATE");
            Property(t => t.CreatedBy).HasColumnName("CREATE_USERD");
            Property(t => t.Modified).HasColumnName("MOD_DATE");
            Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");

            // Relationships
            HasRequired(t => t.Survey)
                .WithMany(t => t.Categories)
                .HasForeignKey(d => d.SurveyId);
        }
    }
}