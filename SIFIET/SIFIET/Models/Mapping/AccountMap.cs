using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AccountType)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Accounts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdUser).HasColumnName("IdUser");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.AccountType).HasColumnName("AccountType");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Accounts)
                .HasForeignKey(d => d.IdUser);

        }
    }
}
