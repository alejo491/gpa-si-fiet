using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class AnswerMap : EntityTypeConfiguration<Answer>
    {
        public AnswerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Answers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdAnswer).HasColumnName("IdAnswer");
            this.Property(t => t.IdQuestion).HasColumnName("IdQuestion");
            this.Property(t => t.IdExemplar).HasColumnName("IdExemplar");
            this.Property(t => t.TextValue).HasColumnName("TextValue");

            // Relationships
            this.HasRequired(t => t.AnswerChoice)
                .WithMany(t => t.Answers)
                .HasForeignKey(d => d.IdAnswer);
            this.HasRequired(t => t.ExemplarsQuestion)
                .WithMany(t => t.Answers)
                .HasForeignKey(d => new { d.IdQuestion, d.IdExemplar });

        }
    }
}
