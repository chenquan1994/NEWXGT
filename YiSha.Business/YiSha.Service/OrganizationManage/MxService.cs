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
    /// 日 期：2025-08-23 12:22
    /// 描 述：明细服务类
    /// </summary>
    public class MxService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<MxEntity>> GetList(MxListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<MxEntity>> GetPageList(MxListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<MxEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<MxEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(MxEntity entity)
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
            await this.BaseRepository().Delete<MxEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<MxEntity, bool>> ListFilter(MxListParam param)
        {
            var expression = LinqExtensions.True<MxEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
