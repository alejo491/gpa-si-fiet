using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class SecureControllerMap : EntityTypeConfiguration<SecureController>
    {
        public SecureControllerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SecureControllers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
