using System;
using System.Collections.Generic;

namespace Entity.Models.DB
{
    public partial class Exchange
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
