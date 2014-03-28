using System.Data.Entity.ModelConfiguration;
using Survey.Core.Domain;

namespace Survey.Core.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModifiedBy)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.LastLogon).HasColumnName("LastLogon");
            this.Property(t => t.Created).HasColumnName("CREATE_DATE");
            this.Property(t => t.CreatedBy).HasColumnName("CREATE_USERID");
            this.Property(t => t.Modified).HasColumnName("MOD_DATE");
            this.Property(t => t.ModifiedBy).HasColumnName("MOD_USERID");
        }
    }
}