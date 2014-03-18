using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class GroupOptionMap : EntityTypeConfiguration<GroupOption>
    {
        public GroupOptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FieldName)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("GroupOptions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdItemData).HasColumnName("IdItemData");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.GruopOrder).HasColumnName("GruopOrder");

            // Relationships
            this.HasRequired(t => t.ItemData)
                .WithMany(t => t.GroupOptions)
                .HasForeignKey(d => d.IdItemData);

        }
    }
}
