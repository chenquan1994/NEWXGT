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
    /// 日 期：2025-08-23 12:01
    /// 描 述：提现业务类
    /// </summary>
    public class TxBLL
    {
        private TxService txService = new TxService();

        #region 获取数据
        public async Task<TData<List<TxEntity>>> GetList(TxListParam param)
        {
            TData<List<TxEntity>> obj = new TData<List<TxEntity>>();
            obj.Data = await txService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<TxEntity>>> GetPageList(TxListParam param, Pagination pagination)
        {
            TData<List<TxEntity>> obj = new TData<List<TxEntity>>();
            obj.Data = await txService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<TxEntity>> GetEntity(long id)
        {
            TData<TxEntity> obj = new TData<TxEntity>();
            obj.Data = await txService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(TxEntity entity)
        {
            TData<string> obj = new TData<string>();
            await txService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await txService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
