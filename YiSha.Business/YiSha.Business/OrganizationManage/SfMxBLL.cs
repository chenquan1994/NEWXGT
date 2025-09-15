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
    /// 日 期：2025-08-23 16:07
    /// 描 述：释放明细业务类
    /// </summary>
    public class SfMxBLL
    {
        private SfMxService sfMxService = new SfMxService();

        #region 获取数据
        public async Task<TData<List<SfMxEntity>>> GetList(SfMxListParam param)
        {
            TData<List<SfMxEntity>> obj = new TData<List<SfMxEntity>>();
            obj.Data = await sfMxService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<SfMxEntity>>> GetPageList(SfMxListParam param, Pagination pagination)
        {
            TData<List<SfMxEntity>> obj = new TData<List<SfMxEntity>>();
            obj.Data = await sfMxService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<SfMxEntity>> GetEntity(long id)
        {
            TData<SfMxEntity> obj = new TData<SfMxEntity>();
            obj.Data = await sfMxService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(SfMxEntity entity)
        {
            TData<string> obj = new TData<string>();
            await sfMxService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await sfMxService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
