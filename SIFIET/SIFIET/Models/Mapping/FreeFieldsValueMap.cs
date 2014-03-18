using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class FreeFieldsValueMap : EntityTypeConfiguration<FreeFieldsValue>
    {
        public FreeFieldsValueMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Value)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("FreeFieldsValues");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdFreeField).HasColumnName("IdFreeField");
            this.Property(t => t.Value).HasColumnName("Value");

            // Relationships
            this.HasRequired(t => t.FreeField)
                .WithMany(t => t.FreeFieldsValues)
                .HasForeignKey(d => d.IdFreeField);

        }
    }
}
