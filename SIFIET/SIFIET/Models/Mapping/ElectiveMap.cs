using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ElectiveMap : EntityTypeConfiguration<Elective>
    {
        public ElectiveMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Electives");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");

            // Relationships
            this.HasMany(t => t.Studies)
                .WithMany(t => t.Electives)
                .Map(m =>
                    {
                        m.ToTable("ElectivesStudies");
                        m.MapLeftKey("IdElective");
                        m.MapRightKey("IdStudies");
                    });


        }
    }
}
