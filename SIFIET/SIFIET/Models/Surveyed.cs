using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa a los encuestados
    /// </summary>
    public partial class Surveyed
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Surveyed()
        {
            this.Exemplars = new List<Exemplar>();
        }

        /// <summary>
        /// Codigo del encuestado
        /// </summary>
        public System.Guid Id { get; set; }
        
        /// <summary>
        /// Nombre del encuestado
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Email del encuestado
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Tipo de encuestado
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Coleccion de ejemplares
        /// </summary>
        public virtual ICollection<Exemplar> Exemplars { get; set; }
    }
}
