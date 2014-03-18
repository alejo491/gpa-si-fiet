using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ExperiencesBossMap : EntityTypeConfiguration<ExperiencesBoss>
    {
        public ExperiencesBossMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ExperiencesBosses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdBoss).HasColumnName("IdBoss");
            this.Property(t => t.IdExperiences).HasColumnName("IdExperiences");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");

            // Relationships
            this.HasRequired(t => t.Boss)
                .WithMany(t => t.ExperiencesBosses)
                .HasForeignKey(d => d.IdBoss);
            this.HasRequired(t => t.Experience)
                .WithMany(t => t.ExperiencesBosses)
                .HasForeignKey(d => d.IdExperiences);

        }
    }
}
