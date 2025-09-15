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
    /// 日 期：2025-08-22 20:25
    /// 描 述：理财订单业务类
    /// </summary>
    public class LicaiOrderBLL
    {
        private LicaiOrderService licaiOrderService = new LicaiOrderService();

        #region 获取数据
        public async Task<TData<List<LicaiOrderEntity>>> GetList(LicaiOrderListParam param)
        {
            TData<List<LicaiOrderEntity>> obj = new TData<List<LicaiOrderEntity>>();
            obj.Data = await licaiOrderService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<LicaiOrderEntity>>> GetPageList(LicaiOrderListParam param, Pagination pagination)
        {
            TData<List<LicaiOrderEntity>> obj = new TData<List<LicaiOrderEntity>>();
            obj.Data = await licaiOrderService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<LicaiOrderEntity>> GetEntity(long id)
        {
            TData<LicaiOrderEntity> obj = new TData<LicaiOrderEntity>();
            obj.Data = await licaiOrderService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(LicaiOrderEntity entity)
        {
            TData<string> obj = new TData<string>();
            await licaiOrderService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await licaiOrderService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
