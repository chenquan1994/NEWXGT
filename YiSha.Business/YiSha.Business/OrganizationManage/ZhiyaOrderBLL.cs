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
    /// 描 述：质押订单业务类
    /// </summary>
    public class ZhiyaOrderBLL
    {
        private ZhiyaOrderService zhiyaOrderService = new ZhiyaOrderService();

        #region 获取数据
        public async Task<TData<List<ZhiyaOrderEntity>>> GetList(ZhiyaOrderListParam param)
        {
            TData<List<ZhiyaOrderEntity>> obj = new TData<List<ZhiyaOrderEntity>>();
            obj.Data = await zhiyaOrderService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZhiyaOrderEntity>>> GetPageList(ZhiyaOrderListParam param, Pagination pagination)
        {
            TData<List<ZhiyaOrderEntity>> obj = new TData<List<ZhiyaOrderEntity>>();
            obj.Data = await zhiyaOrderService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<ZhiyaOrderEntity>> GetEntity(long id)
        {
            TData<ZhiyaOrderEntity> obj = new TData<ZhiyaOrderEntity>();
            obj.Data = await zhiyaOrderService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(ZhiyaOrderEntity entity)
        {
            TData<string> obj = new TData<string>();
            await zhiyaOrderService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await zhiyaOrderService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
