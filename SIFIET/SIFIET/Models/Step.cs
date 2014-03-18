using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    /// <summary>
    /// Pasos del Wizard
    /// </summary>
    public partial class Step
    {
        /// <summary>
        /// Constructor de la Clase Step
        /// </summary>
        public Step()
        {
            this.UsersSteps = new List<UsersStep>();
        }

        /// <summary>
        /// Codigo del Paso
        /// </summary>
        public System.Guid Id { get; set; }
        
        /// <summary>
        /// Numero de Paso
        /// </summary>
        public decimal SOrder { get; set; }
        
        /// <summary>
        /// Direccion del paso que se va alMACENAR
        /// </summary>
        public string SPath { get; set; }
        
        
        /// <summary>
        /// Coleccion de Objetos dela relacion Step y UserSteps
        /// </summary>
        public virtual ICollection<UsersStep> UsersSteps { get; set; }
    }
}
