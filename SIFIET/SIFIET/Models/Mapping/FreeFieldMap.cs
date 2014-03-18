using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class FreeFieldMap : EntityTypeConfiguration<FreeField>
    {
        public FreeFieldMap()
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

            this.Property(t => t.FieldDisplay)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("FreeFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TableName).HasColumnName("TableName");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.FieldDisplay).HasColumnName("FieldDisplay");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.FielOrder).HasColumnName("FielOrder");
        }
    }
}
