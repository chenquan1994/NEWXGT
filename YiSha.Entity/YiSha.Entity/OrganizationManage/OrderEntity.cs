using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 20:25
    /// 描 述：5000U订单实体类
    /// </summary>
    [Table("Cq_order")]
    public class OrderEntity : BaseEntity
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
        public decimal? money { get; set; }
        public decimal? yuanjia { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? state { get; set; }
        public int? sf_state { get; set; }

        public int? sd_rj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? type { get; set; }

        [NotMapped]
        public string orderId { get; set; }
        [NotMapped]
        public string txHash { get; set; }
        [NotMapped]
        public string amount { get; set; }
 
    }
}
