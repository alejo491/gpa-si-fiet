using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class CompanyMap : EntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Sector)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Companies");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Sector).HasColumnName("Sector");
        }
    }
}
