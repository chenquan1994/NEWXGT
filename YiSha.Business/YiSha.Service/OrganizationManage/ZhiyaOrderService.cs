using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;

namespace YiSha.Service.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 20:25
    /// 描 述：质押订单服务类
    /// </summary>
    public class ZhiyaOrderService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<ZhiyaOrderEntity>> GetList(ZhiyaOrderListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<ZhiyaOrderEntity>> GetPageList(ZhiyaOrderListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<ZhiyaOrderEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<ZhiyaOrderEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(ZhiyaOrderEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<ZhiyaOrderEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<ZhiyaOrderEntity, bool>> ListFilter(ZhiyaOrderListParam param)
        {
            var expression = LinqExtensions.True<ZhiyaOrderEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
