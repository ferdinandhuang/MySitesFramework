using Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MySites.DataModels
{
    [Table("User")]
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Passwords { get; set; }
    }
}
