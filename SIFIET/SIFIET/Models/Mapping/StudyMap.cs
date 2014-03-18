using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class StudyMap : EntityTypeConfiguration<Study>
    {
        public StudyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Grade)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Studies");
            this.Property(t => t.IdSchool).HasColumnName("IdSchool");
            this.Property(t => t.IdUser).HasColumnName("IdUser");
            this.Property(t => t.Grade).HasColumnName("Grade");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Id).HasColumnName("Id");

            // Relationships
            this.HasRequired(t => t.School)
                .WithMany(t => t.Studies)
                .HasForeignKey(d => d.IdSchool);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Studies)
                .HasForeignKey(d => d.IdUser);

        }
    }
}
