using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ReportMap : EntityTypeConfiguration<Report>
    {
        public ReportMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Reports");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdUser).HasColumnName("IdUser");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ReportDate).HasColumnName("ReportDate");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Reports)
                .HasForeignKey(d => d.IdUser);

        }
    }
}
