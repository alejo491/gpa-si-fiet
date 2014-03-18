using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhoneNumber)
                .HasMaxLength(20);

            this.Property(t => t.FirstNames)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastNames)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(50);

            this.Property(t => t.CellphoneNumber)
                .HasMaxLength(20);

            this.Property(t => t.Gender)
                .HasMaxLength(1);

            this.Property(t => t.MaritalStatus)
                .HasMaxLength(10);

            this.Property(t => t.States)
               .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.FirstNames).HasColumnName("FirstNames");
            this.Property(t => t.LastNames).HasColumnName("LastNames");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.CellphoneNumber).HasColumnName("CellphoneNumber");
            this.Property(t => t.BirthDate).HasColumnName("BirthDate");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.MaritalStatus).HasColumnName("MaritalStatus");
            this.Property(t => t.States).HasColumnName("States");

            // Relationships
            this.HasRequired(t => t.aspnet_Users)
                .WithOptional(t => t.User);

        }
    }
}
