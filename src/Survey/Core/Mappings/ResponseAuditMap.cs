using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class ResponseAuditMap : EntityTypeConfiguration<ResponseAudit>
    {
        public ResponseAuditMap()
        {
            // Primary Key
            this.HasKey(t => t.ResponseAuditId);

            // Properties
            this.Property(t => t.OldResponseValue)
                .HasMaxLength(100);

            this.Property(t => t.NewResponseValue)
                .HasMaxLength(100);

            this.Property(t => t.OldIsSelectedValue)
                .HasMaxLength(5);

            this.Property(t => t.NewIsSelectedValue)
                .HasMaxLength(5);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RESPONSE_AUDIT");
            this.Property(t => t.ResponseAuditId).HasColumnName("RESPONSE_AUDIT_ID");
            this.Property(t => t.ResponseId).HasColumnName("RESPONSE_ID");
            this.Property(t => t.OldResponseValue).HasColumnName("OLD_RESPONSE_VALUE");
            this.Property(t => t.NewResponseValue).HasColumnName("NEW_RESPONSE_VALUE");
            this.Property(t => t.OldIsSelectedValue).HasColumnName("OLD_RESPONSE_IND");
            this.Property(t => t.NewIsSelectedValue).HasColumnName("NEW_RESPONSE_IND");
            this.Property(t => t.Created).HasColumnName("CREATE_DATE");
            this.Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");

            // Relationships
            this.HasRequired(t => t.Response)
                .WithMany(t => t.ResponseAudits)
                .HasForeignKey(d => d.ResponseId);

        }
    }
}