using System;
using System.Collections.Generic;

namespace SIFIET.Models
{
    public partial class FreeFieldsValue
    {
        public System.Guid Id { get; set; }
        public System.Guid IdFreeField { get; set; }
        public string Value { get; set; }
        public virtual FreeField FreeField { get; set; }
    }
}
