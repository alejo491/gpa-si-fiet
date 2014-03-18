using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ExperienceMap : EntityTypeConfiguration<Experience>
    {
        public ExperienceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Charge)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Description)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Experiences");
            this.Property(t => t.IdUser).HasColumnName("IdUser");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Charge).HasColumnName("Charge");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IdCompanie).HasColumnName("IdCompanie");

            // Relationships
            this.HasRequired(t => t.Company)
                .WithMany(t => t.Experiences)
                .HasForeignKey(d => d.IdCompanie);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Experiences)
                .HasForeignKey(d => d.IdUser);

        }
    }
}
