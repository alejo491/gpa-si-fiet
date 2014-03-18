using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIFIET.Models
{   
    
    /// <summary>
    /// Clase que representa cada una de las experiencias laborales de un usuario
    /// </summary>
    public partial class Experience
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Experience()
        {
            this.ExperiencesBosses = new List<ExperiencesBoss>();
        }

        /// <summary>
        /// Identificador del usuario al que se asocia la experiencia laboral
        /// </summary>
        public System.Guid IdUser { get; set; }

        /// <summary>
        /// Identificador de la experiencia laboral
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Cargo que desempeñó la persona durante una experiencia laboral
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese el cargo desempeñado")]
        [Display(Name = "Cargo")]
        [RegularExpression(@"[A-Za-zñÑáéíóúÁÉÍÓÚ\s]*", ErrorMessage = "El formato es incorrecto")]
        public string Charge { get; set; }

        /// <summary>
        /// Fecha de inicio en un cargo dentro de una compañía
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese esta fecha")]
        [Display(Name = "Fecha de Inicio")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:d}")]
        public System.DateTime StartDate { get; set; }


        /// <summary>
        /// Fecha de finalización o retiro de un cargo dentro de una compañía
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese esta fecha")]
        [Display(Name = "Fecha de Finalización")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:d}")]
        public System.DateTime EndDate { get; set; }


        /// <summary>
        /// Descripción de las labores desempeñadas durante el tiempo laborado en un cargo específico
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese una descripción")]
        [Display(Name = "Descripción")]
        [RegularExpression(@"[(A-Za-zñÑáéíóúÁÉÍÓÚ\s,;:.""''“”0-9@°#$%/=¿?!¡~*|&)-_]*", ErrorMessage = "El formato es incorrecto")]
        public string Description { get; set; }


        /// <summary>
        /// Nombre de la compañía contratante
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese la companía donde laboró")]
        [Display(Name = "Compañía")]
        public System.Guid IdCompanie { get; set; }

        
        /// <summary>
        /// Objeto que referencia a la compañía contratante
        /// </summary>
        public virtual Company Company { get; set; }


        /// <summary>
        /// Colección de objetos que relacionan la experiencia laboral con el jefe que estuvo a cargo en la companía durante el período laborado en la misma
        /// </summary>
        public virtual ICollection<ExperiencesBoss> ExperiencesBosses { get; set; }

        /// <summary>
        /// Objeto que referencia al usuario del que se gestionan sus experiencias laborales
        /// </summary>
        public virtual User User { get; set; }
    }
}
