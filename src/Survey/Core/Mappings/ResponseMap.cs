using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class ResponseMap : EntityTypeConfiguration<Response>
    {
        public ResponseMap()
        {
            // Primary Key
            this.HasKey(t => t.ResponseId);

            // Properties
            this.Property(t => t.IsSelected)
                .HasMaxLength(5);

            this.Property(t => t.Value)
                .HasMaxLength(100);

            this.Property(t => t.Comment)
                .HasMaxLength(150);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RESPONSE");
            this.Property(t => t.ResponseId).HasColumnName("RESPONSE_ID");
            this.Property(t => t.QuestionnaireId).HasColumnName("QUESTIONNAIRE_ID");
            this.Property(t => t.QuestionId).HasColumnName("QUESTION_ID");
            this.Property(t => t.IsSelected).HasColumnName("RESPONSE_IND");
            this.Property(t => t.Value).HasColumnName("RESPONSE_VALUE");
            this.Property(t => t.Comment).HasColumnName("RESPONSE_COMMENT");
            this.Property(t => t.Created).HasColumnName("CREATE_DATE");
            this.Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");
            this.Property(t => t.Modified).HasColumnName("MOD_DATE");
            this.Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");

            // Relationships
            this.HasRequired(t => t.Question)
                .WithMany(t => t.Responses)
                .HasForeignKey(d => d.QuestionId);
            this.HasRequired(t => t.Questionnaire)
                .WithMany(t => t.Responses)
                .HasForeignKey(d => d.QuestionnaireId);
        }
    }
}