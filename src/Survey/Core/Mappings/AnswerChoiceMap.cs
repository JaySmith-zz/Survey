using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class AnswerChoiceMap : EntityTypeConfiguration<AnswerChoice>
    {
        public AnswerChoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.AnswerChoiceId);

            // Properties
            this.Property(t => t.DisplayText)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ANSWER_LOOKUP");
            this.Property(t => t.AnswerChoiceId).HasColumnName("ANSWER_LOOKUP_ID");
            this.Property(t => t.QuestionId).HasColumnName("QUESTION_ID");
            this.Property(t => t.DisplayText).HasColumnName("ANSWER_LOOKUP_TEXT");
            this.Property(t => t.Created).HasColumnName("CREATE_DATE");
            this.Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");
            this.Property(t => t.Modified).HasColumnName("MOD_DATE");
            this.Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");

            // Relationships
            this.HasRequired(t => t.Question)
                .WithMany(t => t.AnswerChoices)
                .HasForeignKey(d => d.QuestionId);
        }
    }
}