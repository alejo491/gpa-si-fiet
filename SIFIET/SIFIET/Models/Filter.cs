namespace SIFIET.Models
{
    /// <summary>
    /// Representa un filtro de item
    /// </summary>
    public partial class Filter
    {
        /// <summary>
        /// Codigo del filtro
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
        /// Operador sobre el campo
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Valor del filtro
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Operador logico
        /// </summary>
        public string LogicOperator { get; set; }

        /// <summary>
        /// Numero del filtro
        /// </summary>
        public decimal FilterNumber { get; set; }

        /// <summary>
        /// Objeto de tipo item
        /// </summary>
        public virtual ItemData ItemData { get; set; }
    }
}
