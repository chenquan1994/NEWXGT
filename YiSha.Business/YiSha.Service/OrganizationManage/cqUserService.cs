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
    /// 日 期：2025-08-22 00:35
    /// 描 述：用户表服务类
    /// </summary>
    public class cqUserService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<cqUserEntity>> GetList(cqUserListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<cqUserEntity>> GetPageList(cqUserListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<cqUserEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<cqUserEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(cqUserEntity entity)
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
            await this.BaseRepository().Delete<cqUserEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<cqUserEntity, bool>> ListFilter(cqUserListParam param)
        {
            var expression = LinqExtensions.True<cqUserEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
