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
    /// 日 期：2025-08-22 00:22
    /// 描 述：业务类
    /// </summary>
    public class MoneyBLL
    {
        private MoneyService moneyService = new MoneyService();

        #region 获取数据
        public async Task<TData<List<MoneyEntity>>> GetList(MoneyListParam param)
        {
            TData<List<MoneyEntity>> obj = new TData<List<MoneyEntity>>();
            obj.Data = await moneyService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<MoneyEntity>>> GetPageList(MoneyListParam param, Pagination pagination)
        {
            TData<List<MoneyEntity>> obj = new TData<List<MoneyEntity>>();
            obj.Data = await moneyService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<MoneyEntity>> GetEntity(long id)
        {
            TData<MoneyEntity> obj = new TData<MoneyEntity>();
            obj.Data = await moneyService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(MoneyEntity entity)
        {
            TData<string> obj = new TData<string>();
            await moneyService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await moneyService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
