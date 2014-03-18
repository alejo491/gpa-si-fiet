using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TableName)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.FieldName)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Items");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdReport).HasColumnName("IdReport");
            this.Property(t => t.TableName).HasColumnName("TableName");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.PageNumber).HasColumnName("PageNumber");

            // Relationships
            this.HasRequired(t => t.Report)
                .WithMany(t => t.Items)
                .HasForeignKey(d => d.IdReport);

        }
    }
}
