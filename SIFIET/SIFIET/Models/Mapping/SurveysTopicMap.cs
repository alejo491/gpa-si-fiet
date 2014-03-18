using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class SurveysTopicMap : EntityTypeConfiguration<SurveysTopic>
    {
        public SurveysTopicMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdSurveys, t.IdTopic });

            // Properties
            // Table & Column Mappings
            this.ToTable("SurveysTopics");
            this.Property(t => t.IdSurveys).HasColumnName("IdSurveys");
            this.Property(t => t.IdTopic).HasColumnName("IdTopic");
            this.Property(t => t.TopicNumber).HasColumnName("TopicNumber");

            // Relationships
            this.HasRequired(t => t.Survey)
                .WithMany(t => t.SurveysTopics)
                .HasForeignKey(d => d.IdSurveys);
            this.HasRequired(t => t.Topic)
                .WithMany(t => t.SurveysTopics)
                .HasForeignKey(d => d.IdTopic);

        }
    }
}
