using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ItemDataMap : EntityTypeConfiguration<ItemData>
    {
        public ItemDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SQLQuey)
                .IsRequired();

            this.Property(t => t.GraphicType)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ItemDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdReport).HasColumnName("IdReport");
            this.Property(t => t.SQLQuey).HasColumnName("SQLQuey");
            this.Property(t => t.GraphicType).HasColumnName("GraphicType");
            this.Property(t => t.ItemNumber).HasColumnName("ItemNumber");
            this.Property(t => t.Sentence).HasColumnName("Sentence");

            // Relationships
            this.HasRequired(t => t.Report)
                .WithMany(t => t.ItemDatas)
                .HasForeignKey(d => d.IdReport);

        }
    }
}
