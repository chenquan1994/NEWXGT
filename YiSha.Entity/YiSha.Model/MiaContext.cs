using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Xml;
 
using YiSha.Entity.OrganizationManage;
 

namespace YiSha.Model
{
    public class MiaContext : DbContext
    {
        public MiaContext(DbContextOptions<MiaContext> options) : base(options)
        {


        }

        public virtual DbSet<cqUserEntity> cq_user { get; set; }
        public virtual DbSet<BmdEntity> cq_bmd { get; set; }
        public virtual DbSet<MoneyEntity> cq_money { get; set; }
        public virtual DbSet<XgtEntity> cq_xgt { get; set; }
        public virtual DbSet<NewsEntity> cq_news { get; set; }
        public virtual DbSet<MoneyDizhiEntity> cq_money_dizhi { get; set; }
        public virtual DbSet<OrderEntity> cq_order { get; set; }
        public virtual DbSet<LicaiOrderEntity> cq_licai_order { get; set; }
        public virtual DbSet<ZhiyaOrderEntity> cq_zhiya_order { get; set; }
        public virtual DbSet<MxEntity> cq_mx { get; set; }
        public virtual DbSet<TxEntity> cq_tx { get; set; }
        public virtual DbSet<KuangListEntity> cq_kuang_order { get; set; }
        public virtual DbSet<SfMxEntity> cq_sf_mx { get; set; }

    }
}

