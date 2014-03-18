using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ExemplarsQuestionMap : EntityTypeConfiguration<ExemplarsQuestion>
    {
        public ExemplarsQuestionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdQuestion, t.IdExemplar });

            // Properties
            // Table & Column Mappings
            this.ToTable("ExemplarsQuestions");
            this.Property(t => t.IdQuestion).HasColumnName("IdQuestion");
            this.Property(t => t.IdExemplar).HasColumnName("IdExemplar");

            // Relationships
            this.HasRequired(t => t.Exemplar)
                .WithMany(t => t.ExemplarsQuestions)
                .HasForeignKey(d => d.IdExemplar);
            this.HasRequired(t => t.Question)
                .WithMany(t => t.ExemplarsQuestions)
                .HasForeignKey(d => d.IdQuestion);

        }
    }
}
