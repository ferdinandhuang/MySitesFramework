using Framework.Core.Models;
using MySites.Common.Enum;
using StackExchange.Redis.Extensions.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MySites.DataModels
{
    [Table("ApiResource")]
    [Serializable]
    public class ApiResource : BaseModel
    {
        public string ApiName { get; set; }
        public string DisplayName { get; set; }
    }
}
