using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    public partial class FreeField
    {
        public FreeField()
        {
            this.FreeFieldsValues = new List<FreeFieldsValue>();
        }

        public System.Guid Id { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldDisplay { get; set; }
        public string Type { get; set; }
        public decimal FielOrder { get; set; }
        public virtual ICollection<FreeFieldsValue> FreeFieldsValues { get; set; }
    }
}
