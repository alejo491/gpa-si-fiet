using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Comments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdPost).HasColumnName("IdPost");
            this.Property(t => t.IdUser).HasColumnName("IdUser");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.CommentDate).HasColumnName("CommentDate");

            // Relationships
            this.HasRequired(t => t.Post)
                .WithMany(t => t.Comments)
                .HasForeignKey(d => d.IdPost);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Comments)
                .HasForeignKey(d => d.IdUser);

        }
    }
}
