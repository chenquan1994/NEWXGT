using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-23 12:22
    /// 描 述：明细实体类
    /// </summary>
    [Table("Cq_mx")]
    public class MxEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string pay_type { get; set; }
        public string? df_dizhi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal? money { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? datetime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? user_id { get; set; }
    }
}
