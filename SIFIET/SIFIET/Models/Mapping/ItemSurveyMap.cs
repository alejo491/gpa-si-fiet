using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ItemSurveyMap : EntityTypeConfiguration<ItemSurvey>
    {
        public ItemSurveyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Question)
                .IsRequired();

            this.Property(t => t.GraphicType)
                .IsRequired();

            this.Property(t => t.SQLQuey)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ItemSurveys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdReport).HasColumnName("IdReport");
            this.Property(t => t.Question).HasColumnName("Question");
            this.Property(t => t.GraphicType).HasColumnName("GraphicType");
            this.Property(t => t.ItemNumber).HasColumnName("ItemNumber");
            this.Property(t => t.SQLQuey).HasColumnName("SQLQuey");

            // Relationships
            this.HasRequired(t => t.Report)
                .WithMany(t => t.ItemSurveys)
                .HasForeignKey(d => d.IdReport);

        }
    }
}
