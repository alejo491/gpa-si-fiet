using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa los roles que tendran los usuarios de la aplicacion, cada rol tendra ciertos permisos y restricciones
    /// </summary>
    public partial class aspnet_Roles
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public aspnet_Roles()
        {
            this.aspnet_UsersInRoles = new List<aspnet_UsersInRoles>();
            this.Posts = new List<Post>();
            this.RoleMethods = new List<RoleMethod>();
        }
        /// <summary>
        /// Identificador de la aplicacion
        /// </summary>
        public System.Guid ApplicationId { get; set; }
        /// <summary>
        /// Identificador del Rol
        /// </summary>
        public System.Guid RoleId { get; set; }
        /// <summary>
        /// Nombre del rol
        /// </summary>
        [Display(Name = "Nombre del Rol")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [MaxLength(50, ErrorMessage = "No puede tener más de 50 caracteres")]
        public string RoleName { get; set; }

       /// <summary>
       ///  nombre del rol en minusculas
       /// </summary>
        public string LoweredRoleName { get; set; }
        /// <summary>
        /// describe el rol, sus caracteristicas mas sobresalientes
        /// </summary>
        [Display(Name = "Descripciòn")]
        [Required(ErrorMessage = " ¡El campo es obligatorio!")]
        [MaxLength(50, ErrorMessage = "No puede tener más de 50 caracteres")]
        public string Description { get; set; }
        /// <summary>
        ///Aplicaciones a las que apllica ese rol
        /// </summary>
        public virtual aspnet_Applications aspnet_Applications { get; set; }
        /// <summary>
        /// Coleccionde usuarios a los que se les ha asignado el rol
        /// </summary>
        public virtual ICollection<aspnet_UsersInRoles> aspnet_UsersInRoles { get; set; }
        /// <summary>
        /// Coleccion de Post que ha hecho el usuario
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; }
        /// <summary>
        /// coleccion de metodos accesibles por este rol
        /// </summary>
        public virtual ICollection<RoleMethod> RoleMethods { get; set; }
    }
}
