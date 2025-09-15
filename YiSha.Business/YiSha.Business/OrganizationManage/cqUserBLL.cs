using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;

namespace YiSha.Business.OrganizationManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 00:35
    /// 描 述：用户表业务类
    /// </summary>
    public class cqUserBLL
    {
        private cqUserService cqUserService = new cqUserService();

        #region 获取数据
        public async Task<TData<List<cqUserEntity>>> GetList(cqUserListParam param)
        {
            TData<List<cqUserEntity>> obj = new TData<List<cqUserEntity>>();
            obj.Data = await cqUserService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<cqUserEntity>>> GetPageList(cqUserListParam param, Pagination pagination)
        {
            TData<List<cqUserEntity>> obj = new TData<List<cqUserEntity>>();
            obj.Data = await cqUserService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<cqUserEntity>> GetEntity(long id)
        {
            TData<cqUserEntity> obj = new TData<cqUserEntity>();
            obj.Data = await cqUserService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(cqUserEntity entity)
        {
            TData<string> obj = new TData<string>();
            await cqUserService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await cqUserService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
