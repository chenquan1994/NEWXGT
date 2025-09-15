using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 20:25
    /// 描 述：理财订单实体类
    /// </summary>
    [Table("Cq_licai_order")]
    public class LicaiOrderEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? state { get; set; }
        public int? sf_state { get; set; }
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
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? datetime { get; set; }


        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? sf_datetime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal? money { get; set; }
        public decimal? sf_money { get; set; }
        public decimal? wsf_money { get; set; }
    }
}
