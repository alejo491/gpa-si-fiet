using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ThesisMap : EntityTypeConfiguration<Thesis>
    {
        public ThesisMap()
        {
            // Primary Key
            this.HasKey(t => t.IdStudies);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Thesis");
            this.Property(t => t.IdStudies).HasColumnName("IdStudies");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Description).HasColumnName("Description");

            // Relationships
            this.HasRequired(t => t.Study)
                .WithOptional(t => t.Thesis);

        }
    }
}
