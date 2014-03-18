using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class FilesPostMap : EntityTypeConfiguration<FilesPost>
    {
        public FilesPostMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdPost, t.IdFile });

            // Properties
            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("FilesPosts");
            this.Property(t => t.IdPost).HasColumnName("IdPost");
            this.Property(t => t.IdFile).HasColumnName("IdFile");
            this.Property(t => t.Main).HasColumnName("Main");
            this.Property(t => t.Type).HasColumnName("Type");

            // Relationships
            this.HasRequired(t => t.File)
                .WithMany(t => t.FilesPosts)
                .HasForeignKey(d => d.IdFile);
            this.HasRequired(t => t.Post)
                .WithMany(t => t.FilesPosts)
                .HasForeignKey(d => d.IdPost);

        }
    }
}
