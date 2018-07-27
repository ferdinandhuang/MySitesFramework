using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySites.DataModels
{
    [Table(Schema ="public")]
    public class Test
    {
        public string test { get; set; }
    }
}
