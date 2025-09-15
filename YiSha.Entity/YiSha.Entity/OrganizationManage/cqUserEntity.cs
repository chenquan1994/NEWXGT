using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 00:35
    /// 描 述：用户表实体类
    /// </summary>
    [Table("Cq_user")]
    public class cqUserEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string dizhi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? datetitime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string f_id { get; set; }
        public decimal? yeji { get; set; }
        public int? zhitui { get; set; }

        public int jibie { get; set; }
        public int biaoshi { get; set; }
        public int dongjie { get; set; }
        public int tx_state { get; set; }
        public int dh_state { get; set; }
        public int hz_state { get; set; }
        public int sf_state { get; set; }
        public int zy_state { get; set; }
        public string yqm { get; set; }
        public string zjc { get; set; }
        public string zf_pass { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? zt_id { get; set; }
    }
}
