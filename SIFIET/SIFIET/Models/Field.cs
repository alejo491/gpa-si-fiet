

namespace SIFIET.Models
{

    /// <summary>
    /// Clase que representa los campos de un item de datos
    /// </summary>
    public partial class Field
    {
        /// <summary>
        /// Codigo del campo
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Codigo del item
        /// </summary>
        public System.Guid IdItemData { get; set; }

        /// <summary>
        /// Nombre del campo
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Operador del campo
        /// </summary>
        public string FieldOperation { get; set; }

        /// <summary>
        /// orden del campo
        /// </summary>
        public decimal FieldOrder { get; set; }

        /// <summary>
        /// Objeto Item
        /// </summary>
        public virtual ItemData ItemData { get; set; }
    }
}
