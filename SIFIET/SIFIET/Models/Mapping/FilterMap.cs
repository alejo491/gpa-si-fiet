using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class FilterMap : EntityTypeConfiguration<Filter>
    {
        public FilterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FieldName)
                .IsRequired();

            this.Property(t => t.Operator)
                .IsRequired();

            this.Property(t => t.Value)
                .IsRequired();

            this.Property(t => t.LogicOperator)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Filters");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdItemData).HasColumnName("IdItemData");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.Operator).HasColumnName("Operator");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.LogicOperator).HasColumnName("LogicOperator");
            this.Property(t => t.FilterNumber).HasColumnName("FilterNumber");

            // Relationships
            this.HasRequired(t => t.ItemData)
                .WithMany(t => t.Filters)
                .HasForeignKey(d => d.IdItemData);

        }
    }
}
