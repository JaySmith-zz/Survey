using System.Data.Entity.ModelConfiguration;

namespace Survey.Core.Mappings
{
    public class SurveyMap : EntityTypeConfiguration<Domain.Survey>
    {
        public SurveyMap()
        {
            // Primary Key
            HasKey(t => t.SurveyId);

            // Properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

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
            ToTable("SURVEY");
            Property(t => t.SurveyId).HasColumnName("SURVEY_ID");
            Property(t => t.Name).HasColumnName("SURVEY_NAME");
            Property(t => t.Status).HasColumnName("SURVEY_STATUS");
            Property(t => t.IsDefaultSurvey).HasColumnName("DEFAULT_SURVEY_IND");
            Property(t => t.Created).HasColumnName("CREATE_DATE");
            Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");
            Property(t => t.Modified).HasColumnName("MOD_DATE");
            Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");
        }
    }
}