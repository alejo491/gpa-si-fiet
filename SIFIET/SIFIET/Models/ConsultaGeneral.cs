using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Representa la vista de la base de datos
    /// </summary>
    public partial class ConsultaGeneral
    {
        /// <summary>
        /// Telefono del usuario
        /// </summary>
        public string Telefono { get; set; }
        /// <summary>
        /// Nombre del usuario
        /// </summary>
        public string Nombres { get; set; }
        /// <summary>
        /// Apellidos del usuario
        /// </summary>
        public string Apellidos { get; set; }

        /// <summary>
        /// Direccion del usuario
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Celular del usuario
        /// </summary>
        public string Celular { get; set; }

        /// <summary>
        /// Fecha de nacimiento del usuario
        /// </summary>
        public Nullable<System.DateTime> FechaNacimiento { get; set; }

        /// <summary>
        /// Genero del usuario
        /// </summary>
        public string Genero { get; set; }

        /// <summary>
        /// Estado civil del usuario
        /// </summary>
        public string EstadoCivil { get; set; }

        /// <summary>
        /// Titulo del usuario
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Fecha de inicio de los estudios del usuario
        /// </summary>
        public System.DateTime FechaInicioEstudio { get; set; }

        /// <summary>
        /// /// <summary>
        /// Fecha de Fin de los estudios del usuario
        /// </summary>
        /// </summary>
        public System.DateTime FechaFinEstudio { get; set; }

        /// <summary>
        /// Electiva del estudiante
        /// </summary>
        public string NombreElectiva { get; set; }

        /// <summary>
        /// Titulo de la tesis
        /// </summary>
        public string TituloTesis { get; set; }

        /// <summary>
        /// Descripcion de la tesis
        /// </summary>
        public string DescripcionTesis { get; set; }

        /// <summary>
        /// Nombre de la institucion
        /// </summary>
        public string NombreInstitucion { get; set; }

        /// <summary>
        /// Cargo
        /// </summary>
        public string Cargo { get; set; }

        /// <summary>
        /// Fecha de inicio de la experiencia
        /// </summary>
        public System.DateTime FechaInicioExperiencia { get; set; }

        /// <summary>
        /// Fecha de fin de la experiencia
        /// </summary>
        public System.DateTime FechaFinExperiencia { get; set; }

        /// <summary>
        /// Descripcion de la experiencia
        /// </summary>
        public string DescripcionExperiencia { get; set; }

        /// <summary>
        /// Fecha de inicio de experiencia del jefe
        /// </summary>
        public System.DateTime FechaInicioExperienciaJefe { get; set; }

        /// <summary>
        /// Fecha de fin de experiencia del jefe
        /// </summary>
        public System.DateTime FechaFinExperienciaJefe { get; set; }

        /// <summary>
        /// Nombre del fefe
        /// </summary>
        public string NombreJeve { get; set; }

        /// <summary>
        /// Email del feje
        /// </summary>
        public string EmailJefe { get; set; }

        /// <summary>
        /// Telefono del jefe
        /// </summary>
        public string TelefonoJefe { get; set; }
        
        /// <summary>
        /// Nombre del compañia
        /// </summary>
        public string NombreCompania { get; set; }
        
        /// <summary>
        /// Ciudad de la compañia
        /// </summary>
        public string Ciudad { get; set; }
        
        /// <summary>
        /// Sector de la compañia
        /// </summary>
        public string Sector { get; set; }
        
        /// <summary>
        /// Tipo de la compañia
        /// </summary>
        public string Tipo { get; set; }
        
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public string NombreDeUsuario { get; set; }
        
        /// <summary>
        /// Rol del usuario
        /// </summary>
        public string Rol { get; set; }
        
        /// <summary>
        /// Descripcion del rol
        /// </summary>
        public string DescripcionRol { get; set; }
        
        /// <summary>
        /// Email del usuario
        /// </summary>
        public string EmailUsuario { get; set; }
    }
}
