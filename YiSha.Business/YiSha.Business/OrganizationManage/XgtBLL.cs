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
    /// 日 期：2025-08-22 07:57
    /// 描 述：XGT动态表业务类
    /// </summary>
    public class XgtBLL
    {
        private XgtService xgtService = new XgtService();

        #region 获取数据
        public async Task<TData<List<XgtEntity>>> GetList(XgtListParam param)
        {
            TData<List<XgtEntity>> obj = new TData<List<XgtEntity>>();
            obj.Data = await xgtService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<XgtEntity>>> GetPageList(XgtListParam param, Pagination pagination)
        {
            TData<List<XgtEntity>> obj = new TData<List<XgtEntity>>();
            obj.Data = await xgtService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<XgtEntity>> GetEntity(long id)
        {
            TData<XgtEntity> obj = new TData<XgtEntity>();
            obj.Data = await xgtService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(XgtEntity entity)
        {
            TData<string> obj = new TData<string>();
            await xgtService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await xgtService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
