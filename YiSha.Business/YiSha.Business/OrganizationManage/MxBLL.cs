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
    /// 日 期：2025-08-23 12:22
    /// 描 述：明细业务类
    /// </summary>
    public class MxBLL
    {
        private MxService mxService = new MxService();

        #region 获取数据
        public async Task<TData<List<MxEntity>>> GetList(MxListParam param)
        {
            TData<List<MxEntity>> obj = new TData<List<MxEntity>>();
            obj.Data = await mxService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<MxEntity>>> GetPageList(MxListParam param, Pagination pagination)
        {
            TData<List<MxEntity>> obj = new TData<List<MxEntity>>();
            obj.Data = await mxService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<MxEntity>> GetEntity(long id)
        {
            TData<MxEntity> obj = new TData<MxEntity>();
            obj.Data = await mxService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(MxEntity entity)
        {
            TData<string> obj = new TData<string>();
            await mxService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await mxService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
