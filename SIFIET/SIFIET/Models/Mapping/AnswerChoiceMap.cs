using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class AnswerChoiceMap : EntityTypeConfiguration<AnswerChoice>
    {
        public AnswerChoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Sentence)
                .IsRequired();

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("AnswerChoices");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdQuestion).HasColumnName("IdQuestion");
            this.Property(t => t.Sentence).HasColumnName("Sentence");
            this.Property(t => t.NumericValue).HasColumnName("NumericValue");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.AnswerNumber).HasColumnName("AnswerNumber");

            // Relationships
            this.HasRequired(t => t.Question)
                .WithMany(t => t.AnswerChoices)
                .HasForeignKey(d => d.IdQuestion);

        }
    }
}
