using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class TopicMap : EntityTypeConfiguration<Topic>
    {
        public TopicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Topics");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
