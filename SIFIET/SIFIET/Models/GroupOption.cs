namespace SIFIET.Models
{
    /// <summary>
    /// Clase que representa el agrupamiento de un item.
    /// </summary>
    public partial class GroupOption
    {
        /// <summary>
        /// Codigo del grupo
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Codigo del item de datos
        /// </summary>
        public System.Guid IdItemData { get; set; }

        /// <summary>
        /// Nombre del campo del grup
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Nunero del grupo
        /// </summary>
        public decimal GruopOrder { get; set; }

        /// <summary>
        /// Objeto de tipo item de datos.
        /// </summary>
        public virtual ItemData ItemData { get; set; }

    }
}
