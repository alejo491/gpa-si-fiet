using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class UsersStepMap : EntityTypeConfiguration<UsersStep>
    {
        public UsersStepMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.IdSteps });

            // Properties
            this.Property(t => t.Ok)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UsersSteps");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IdSteps).HasColumnName("IdSteps");
            this.Property(t => t.Ok).HasColumnName("Ok");

            // Relationships
            this.HasRequired(t => t.Step)
                .WithMany(t => t.UsersSteps)
                .HasForeignKey(d => d.IdSteps);
            this.HasRequired(t => t.User)
                .WithMany(t => t.UsersSteps)
                .HasForeignKey(d => d.UserId);

        }
    }
}
