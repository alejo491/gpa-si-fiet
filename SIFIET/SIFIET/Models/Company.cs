using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa una compañía o empresa de la cual se requiere guardar alguna información
    /// </summary>
    public partial class Company
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Company()
        {
            this.Experiences = new List<Experience>();
            this.Vacancies = new List<Vacancy>();
        }

        /// <summary>
        /// Identificador único para cada compañía registrada
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Nombre de la compañía o empresa
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese un nombre para la compañía")]

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        /// <summary>
        /// Teléfono de la compañía
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese un teléfono para la compañía")]
        [Display(Name = "Teléfono")]
        [RegularExpression(@"[0-9]{7,10}", ErrorMessage = " No tiene el formato de Telefono")]
        public int Phone { get; set; }

        /// <summary>
        /// Dirección de alguna sede principal de la compañía
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese la dirección de la compañía")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        /// <summary>
        /// Correo electrónico para contactar con la compañía
        /// </summary>        
        [Required(ErrorMessage = "Por favor ingrese un e-mail de la compañía")]
        [Display(Name = "E-Mail")]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$", ErrorMessage = " No tiene el formato de un correo electrónico")]
        public string Email { get; set; }

        /// <summary>
        /// Ciudad donde se encuentra la compañía, o alguna sede
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese la ciudad donde se encuentra la compañía")]
        [Display(Name = "Ciudad")]
        [RegularExpression(@"[A-Za-zñÑáéíóúÁÉÍÓÚ\s]*", ErrorMessage = "El formato es incorrecto")]
        public string City { get; set; }

        /// <summary>
        /// Sector empresarial donde se ubica la compañía
        /// </summary>
        [Required(ErrorMessage = "Por favor ingrese el sector empresarial donde se encuentra la compañía")]
        [Display(Name = "Sector")]
        [RegularExpression(@"[A-Za-zñÑáéíóúÁÉÍÓÚ\s]*", ErrorMessage = "El formato es incorrecto")]
        public string Sector { get; set; }

        /// <summary>
        /// Colección que hace referencia a personas que han trabajado en la compañía
        /// </summary>
        [Display(Name = "Experiencias")]
        public virtual ICollection<Experience> Experiences { get; set; }

        /// <summary>
        /// Colección que hace referencia a vacantes u ofertas de empleo que se ofrecen en la compañia
        /// </summary>
        [Display(Name = "Vacantes")]
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
