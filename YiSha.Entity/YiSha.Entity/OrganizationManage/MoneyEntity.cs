using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 00:22
    /// 描 述：实体类
    /// </summary>
    [Table("Cq_Money")]
    public class MoneyEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? User_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal usdt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal usdk { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal xgt { get; set; }
    }
}
