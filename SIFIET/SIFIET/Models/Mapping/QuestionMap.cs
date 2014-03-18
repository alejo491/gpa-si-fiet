using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Sentence)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Questions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdTopic).HasColumnName("IdTopic");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Sentence).HasColumnName("Sentence");
            this.Property(t => t.QuestionNumber).HasColumnName("QuestionNumber");
            this.Property(t => t.IsObligatory).HasColumnName("IsObligatory");

            // Relationships
            this.HasRequired(t => t.Topic)
                .WithMany(t => t.Questions)
                .HasForeignKey(d => d.IdTopic);

        }
    }
}
