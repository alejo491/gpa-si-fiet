using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class MethodMap : EntityTypeConfiguration<Method>
    {
        public MethodMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Methods");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.IdController).HasColumnName("IdController");

            // Relationships
            this.HasRequired(t => t.SecureController)
                .WithMany(t => t.Methods)
                .HasForeignKey(d => d.IdController);

        }
    }
}
