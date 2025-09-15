using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 20:25
    /// 描 述：质押订单实体类
    /// </summary>
    [Table("Cq_zhiya_order")]
    public class ZhiyaOrderEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? state { get; set; }
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
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? datetime { get; set; }


        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? sf_datetime { get; set; }

        public decimal? sf_money { get; set; }
        public decimal? wsf_money { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal? money { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? day { get; set; }
    }
}
