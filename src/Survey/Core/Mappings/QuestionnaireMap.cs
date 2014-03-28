using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class QuestionnaireMap : EntityTypeConfiguration<Questionnaire>
    {
        public QuestionnaireMap()
        {
            // Primary Key
            this.HasKey(t => t.QuestionnaireId);

            // Properties
            this.Property(t => t.PersonnelAreaNumber)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("QUESTIONNAIRE");
            this.Property(t => t.QuestionnaireId).HasColumnName("QUESTIONNAIRE_ID");
            this.Property(t => t.SurveyId).HasColumnName("SURVEY_ID");
            this.Property(t => t.PersonnelAreaNumber).HasColumnName("PRSNL_AREA_NUM");
            this.Property(t => t.CurrentCategoryId).HasColumnName("CURRENT_CATEGORY_ID");
            this.Property(t => t.Status).HasColumnName("QUESTIONNAIRE_STATUS");
            this.Property(t => t.Created).HasColumnName("CREATE_DATE");
            this.Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");
            this.Property(t => t.Modified).HasColumnName("MOD_DATE");
            this.Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");
            this.Property(t => t.Started).HasColumnName("QUESTIONNAIRE_BEGIN_DATE");
            this.Property(t => t.StartedBy).HasColumnName("QUESTIONNAIRE_BEGIN_USERID");

            // Relationships
            this.HasMany(t => t.OnsiteReview)
                .WithMany(t => t.Questionnaires)
                .Map(m =>
                {
                    m.ToTable("ONSITE_REVIEW_QUESTIONNAIRE");
                    m.MapLeftKey("QUESTIONNAIRE_ID");
                    m.MapRightKey("ONSITE_REVIEW_ID");
                });

            this.HasOptional(t => t.CurrentCategory)
                .WithMany(t => t.Questionnaires)
                .HasForeignKey(d => d.CurrentCategoryId);

            this.HasRequired(t => t.User)
                .WithMany(t => t.Questionnaires)
                .HasForeignKey(d => d.PersonnelAreaNumber);

            this.HasRequired(t => t.Survey)
                .WithMany(t => t.Questionnaires)
                .HasForeignKey(d => d.SurveyId);
        }
    }
}