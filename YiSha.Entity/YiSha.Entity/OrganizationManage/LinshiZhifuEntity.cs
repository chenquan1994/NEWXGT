using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 22:41
    /// 描 述：临时支付回调参数实体类
    /// </summary>
    [Table("Cq_linshi_zhifu")]
    public class LinshiZhifuEntity : BaseEntity
    {
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
        public string haxi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? order_id { get; set; }
    }
}
