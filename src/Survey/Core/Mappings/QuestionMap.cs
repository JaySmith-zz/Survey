using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.QuestionId);

            // Properties
            this.Property(t => t.QuestionCode)
                .HasMaxLength(10);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DisplayText)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.InputType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DisplayLogic)
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("QUESTION");
            this.Property(t => t.QuestionId).HasColumnName("QUESTION_ID");
            this.Property(t => t.CategoryId).HasColumnName("CATEGORY_ID");
            this.Property(t => t.QuestionCode).HasColumnName("QUESTION_CODE");
            this.Property(t => t.Status).HasColumnName("QUESTION_STATUS");
            this.Property(t => t.DisplayText).HasColumnName("QUESTION_DISPLAY_TEXT");
            this.Property(t => t.InputType).HasColumnName("INPUT_TYPE");
            this.Property(t => t.IsKeyQuestion).HasColumnName("KEY_QUESTION_IND");
            this.Property(t => t.IsRequired).HasColumnName("REQUIRED_QUESTION_IND");
            this.Property(t => t.DisplayOrder).HasColumnName("QUESTION_DISPLAY_SEQ");
            this.Property(t => t.DisplayLogic).HasColumnName("DISPLAY_LOGIC");
            this.Property(t => t.Created).HasColumnName("CREATE_DATE");
            this.Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");
            this.Property(t => t.Modified).HasColumnName("MOD_DATE");
            this.Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");

            // Relationships
            this.HasRequired(t => t.Category)
                .WithMany(t => t.Questions)
                .HasForeignKey(d => d.CategoryId);
        }
    }
}