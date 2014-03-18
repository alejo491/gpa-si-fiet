using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Abstract)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.Content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Posts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdUser).HasColumnName("IdUser");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Abstract).HasColumnName("Abstract");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.PublicationDate).HasColumnName("PublicationDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Autorized).HasColumnName("Autorized");
            this.Property(t => t.Main).HasColumnName("Main");
            this.Property(t => t.Estate).HasColumnName("Estate");
            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Posts)
                .HasForeignKey(d => d.IdUser);

        }
    }
}
