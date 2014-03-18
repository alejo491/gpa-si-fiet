using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace SIFIET.Models.Mapping
{
    public class ConsultaGeneralMap : EntityTypeConfiguration<ConsultaGeneral>
    {
        public ConsultaGeneralMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Nombres, t.Apellidos, t.Titulo, t.FechaInicioEstudio, t.FechaFinEstudio, t.NombreElectiva, t.TituloTesis, t.NombreInstitucion, t.Cargo, t.FechaInicioExperiencia, t.FechaFinExperiencia, t.DescripcionExperiencia, t.FechaInicioExperienciaJefe, t.FechaFinExperienciaJefe, t.NombreJeve, t.EmailJefe, t.NombreCompania, t.Ciudad, t.Sector, t.Tipo, t.NombreDeUsuario, t.Rol });

            // Properties
            this.Property(t => t.Telefono)
                .HasMaxLength(20);

            this.Property(t => t.Nombres)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Apellidos)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Direccion)
                .HasMaxLength(50);

            this.Property(t => t.Celular)
                .HasMaxLength(20);

            this.Property(t => t.Genero)
                .HasMaxLength(1);

            this.Property(t => t.EstadoCivil)
                .HasMaxLength(10);

            this.Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.NombreElectiva)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.TituloTesis)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NombreInstitucion)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Cargo)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.DescripcionExperiencia)
                .IsRequired();

            this.Property(t => t.NombreJeve)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EmailJefe)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.TelefonoJefe)
                .HasMaxLength(20);

            this.Property(t => t.NombreCompania)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Ciudad)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Sector)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Tipo)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.NombreDeUsuario)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Rol)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.DescripcionRol)
                .HasMaxLength(256);

            this.Property(t => t.EmailUsuario)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("ConsultaGeneral");
            this.Property(t => t.Telefono).HasColumnName("Telefono");
            this.Property(t => t.Nombres).HasColumnName("Nombres");
            this.Property(t => t.Apellidos).HasColumnName("Apellidos");
            this.Property(t => t.Direccion).HasColumnName("Direccion");
            this.Property(t => t.Celular).HasColumnName("Celular");
            this.Property(t => t.FechaNacimiento).HasColumnName("FechaNacimiento");
            this.Property(t => t.Genero).HasColumnName("Genero");
            this.Property(t => t.EstadoCivil).HasColumnName("EstadoCivil");
            this.Property(t => t.Titulo).HasColumnName("Titulo");
            this.Property(t => t.FechaInicioEstudio).HasColumnName("FechaInicioEstudio");
            this.Property(t => t.FechaFinEstudio).HasColumnName("FechaFinEstudio");
            this.Property(t => t.NombreElectiva).HasColumnName("NombreElectiva");
            this.Property(t => t.TituloTesis).HasColumnName("TituloTesis");
            this.Property(t => t.DescripcionTesis).HasColumnName("DescripcionTesis");
            this.Property(t => t.NombreInstitucion).HasColumnName("NombreInstitucion");
            this.Property(t => t.Cargo).HasColumnName("Cargo");
            this.Property(t => t.FechaInicioExperiencia).HasColumnName("FechaInicioExperiencia");
            this.Property(t => t.FechaFinExperiencia).HasColumnName("FechaFinExperiencia");
            this.Property(t => t.DescripcionExperiencia).HasColumnName("DescripcionExperiencia");
            this.Property(t => t.FechaInicioExperienciaJefe).HasColumnName("FechaInicioExperienciaJefe");
            this.Property(t => t.FechaFinExperienciaJefe).HasColumnName("FechaFinExperienciaJefe");
            this.Property(t => t.NombreJeve).HasColumnName("NombreJeve");
            this.Property(t => t.EmailJefe).HasColumnName("EmailJefe");
            this.Property(t => t.TelefonoJefe).HasColumnName("TelefonoJefe");
            this.Property(t => t.NombreCompania).HasColumnName("NombreCompania");
            this.Property(t => t.Ciudad).HasColumnName("Ciudad");
            this.Property(t => t.Sector).HasColumnName("Sector");
            this.Property(t => t.Tipo).HasColumnName("Tipo");
            this.Property(t => t.NombreDeUsuario).HasColumnName("NombreDeUsuario");
            this.Property(t => t.Rol).HasColumnName("Rol");
            this.Property(t => t.DescripcionRol).HasColumnName("DescripcionRol");
            this.Property(t => t.EmailUsuario).HasColumnName("EmailUsuario");
        }
    }
}
