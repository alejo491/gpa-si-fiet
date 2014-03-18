using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class FieldMap : EntityTypeConfiguration<Field>
    {
        public FieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FieldName)
                .IsRequired();

            this.Property(t => t.FieldOperation)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Fields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdItemData).HasColumnName("IdItemData");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.FieldOperation).HasColumnName("FieldOperation");
            this.Property(t => t.FieldOrder).HasColumnName("FieldOrder");

            // Relationships
            this.HasRequired(t => t.ItemData)
                .WithMany(t => t.Fields)
                .HasForeignKey(d => d.IdItemData);

        }
    }
}
