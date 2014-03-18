using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class aspnet_RolesMap : EntityTypeConfiguration<aspnet_Roles>
    {
        public aspnet_RolesMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleId);

            // Properties
            this.Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.LoweredRoleName)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Description)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("aspnet_Roles");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.LoweredRoleName).HasColumnName("LoweredRoleName");
            this.Property(t => t.Description).HasColumnName("Description");


            this.HasMany(t => t.Posts)
                .WithMany(t => t.aspnet_Roles)
                .Map(m =>
                {
                    m.ToTable("RolesPosts");
                    m.MapLeftKey("RoleId");
                    m.MapRightKey("IdPost");
                });

            // Relationships
            this.HasRequired(t => t.aspnet_Applications)
                .WithMany(t => t.aspnet_Roles)
                .HasForeignKey(d => d.ApplicationId);

        }
    }
}
