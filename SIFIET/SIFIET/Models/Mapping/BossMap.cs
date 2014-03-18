using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class BossMap : EntityTypeConfiguration<Boss>
    {
        public BossMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Bosses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
        }
    }
}
