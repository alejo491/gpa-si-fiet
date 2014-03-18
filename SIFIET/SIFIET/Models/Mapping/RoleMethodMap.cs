using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class RoleMethodMap : EntityTypeConfiguration<RoleMethod>
    {
        public RoleMethodMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdRole, t.IdAction });

            // Properties
            // Table & Column Mappings
            this.ToTable("RoleMethods");
            this.Property(t => t.IdRole).HasColumnName("IdRole");
            this.Property(t => t.IdAction).HasColumnName("IdAction");

            // Relationships
            this.HasRequired(t => t.aspnet_Roles)
                .WithMany(t => t.RoleMethods)
                .HasForeignKey(d => d.IdRole);
            this.HasRequired(t => t.Method)
                .WithMany(t => t.RoleMethods)
                .HasForeignKey(d => d.IdAction);

        }
    }
}
