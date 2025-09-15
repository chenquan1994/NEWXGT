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
    /// 日 期：2025-08-22 10:31
    /// 描 述：收款地址业务类
    /// </summary>
    public class MoneyDizhiBLL
    {
        private MoneyDizhiService moneyDizhiService = new MoneyDizhiService();

        #region 获取数据
        public async Task<TData<List<MoneyDizhiEntity>>> GetList(MoneyDizhiListParam param)
        {
            TData<List<MoneyDizhiEntity>> obj = new TData<List<MoneyDizhiEntity>>();
            obj.Data = await moneyDizhiService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<MoneyDizhiEntity>>> GetPageList(MoneyDizhiListParam param, Pagination pagination)
        {
            TData<List<MoneyDizhiEntity>> obj = new TData<List<MoneyDizhiEntity>>();
            obj.Data = await moneyDizhiService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<MoneyDizhiEntity>> GetEntity(long id)
        {
            TData<MoneyDizhiEntity> obj = new TData<MoneyDizhiEntity>();
            obj.Data = await moneyDizhiService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(MoneyDizhiEntity entity)
        {
            TData<string> obj = new TData<string>();
            await moneyDizhiService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await moneyDizhiService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
