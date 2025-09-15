using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 10:31
    /// 描 述：收款地址实体类
    /// </summary>
    [Table("Cq_money_dizhi")]
    public class MoneyDizhiEntity : BaseEntity
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
        public string miyao { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? type { get; set; }
    }
}
