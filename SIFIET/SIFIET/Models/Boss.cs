using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa una persona que ha ejercido como jefe de un usuario, en un cargo desempeñado durante un período de tiempo
    /// </summary>
    public partial class Boss
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Boss()
        {
            this.ExperiencesBosses = new List<ExperiencesBoss>();
        }

        /// <summary>
        /// Identificador único para cada jefe registrado
        /// </summary>

        public System.Guid Id { get; set; }

        /// <summary>
        /// Nombre completo del jefe
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese el nombre de este jefe")]
        [Display(Name = "Nombre")]
        [RegularExpression(@"[A-Za-zñÑáéíóúÁÉÍÓÚ\s]*", ErrorMessage = "El formato es incorrecto")]
        public string Name { get; set; }

        /// <summary>
        /// Correo electrónico de contacto del jefe
        /// </summary>      
        [Required(ErrorMessage = "Por favor el correo electrónico de contacto")]
        [Display(Name = "E-Mail")]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = " No tiene el formato de un correo electrónico")]
        public string Email { get; set; }

        /// <summary>
        /// Número telefónico de contacto del jefe
        /// </summary>  
        [Display(Name = "Teléfono")]
        [RegularExpression(@"[0-9]{7,10}", ErrorMessage = " No tiene el formato de Telefono")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Colección que indica las relaciones jefe-empleado que ha tenido un jefe
        /// </summary> 
        public virtual ICollection<ExperiencesBoss> ExperiencesBosses { get; set; }
    }
}
