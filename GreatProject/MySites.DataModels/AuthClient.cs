using Framework.Core.Models;
using MySites.Common.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySites.DataModels
{
    [Table("AuthClient")]
    [Serializable]
    public class AuthClient : BaseModel
    {
        /// <summary>
        /// Name of client
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Sha256
        /// </summary>
        public string Secret { get; set; }

    }
}
