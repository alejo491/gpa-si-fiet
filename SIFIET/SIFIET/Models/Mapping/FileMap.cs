using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class FileMap : EntityTypeConfiguration<File>
    {
        public FileMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Path)
                .IsRequired()
                .HasMaxLength(600);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(600);

            this.Property(t => t.Size)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Files");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Size).HasColumnName("Size");
        }
    }
}
