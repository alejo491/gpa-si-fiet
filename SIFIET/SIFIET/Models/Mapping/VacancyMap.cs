using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class VacancyMap : EntityTypeConfiguration<Vacancy>
    {
        public VacancyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Charge)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DayTrip)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ProfessionalProfile)
                .IsRequired();

            this.Property(t => t.VacanciesNumber)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Vacancies");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Charge).HasColumnName("Charge");
            this.Property(t => t.DayTrip).HasColumnName("DayTrip");
            this.Property(t => t.HoursNumber).HasColumnName("HoursNumber");
            this.Property(t => t.ProfessionalProfile).HasColumnName("ProfessionalProfile");
            this.Property(t => t.PublicationDate).HasColumnName("PublicationDate");
            this.Property(t => t.ClosingDate).HasColumnName("ClosingDate");
            this.Property(t => t.VacanciesNumber).HasColumnName("VacanciesNumber");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IdUser).HasColumnName("IdUser");
            this.Property(t => t.IdCompanie).HasColumnName("IdCompanie");

            // Relationships
            this.HasRequired(t => t.Company)
                .WithMany(t => t.Vacancies)
                .HasForeignKey(d => d.IdCompanie);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Vacancies)
                .HasForeignKey(d => d.IdUser);

        }
    }
}
