using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ExemplarMap : EntityTypeConfiguration<Exemplar>
    {
        public ExemplarMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Exemplars");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdSurveys).HasColumnName("IdSurveys");
            this.Property(t => t.ExemplarNumber).HasColumnName("ExemplarNumber");
            this.Property(t => t.IdSurveyed).HasColumnName("IdSurveyed");

            // Relationships
            this.HasRequired(t => t.Survey)
                .WithMany(t => t.Exemplars)
                .HasForeignKey(d => d.IdSurveys);
            this.HasRequired(t => t.Surveyed)
                .WithMany(t => t.Exemplars)
                .HasForeignKey(d => d.IdSurveyed);

        }
    }
}
