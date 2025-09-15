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
    /// 日 期：2025-09-06 11:47
    /// 描 述：白名单业务类
    /// </summary>
    public class BmdBLL
    {
        private BmdService bmdService = new BmdService();

        #region 获取数据
        public async Task<TData<List<BmdEntity>>> GetList(BmdListParam param)
        {
            TData<List<BmdEntity>> obj = new TData<List<BmdEntity>>();
            obj.Data = await bmdService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<BmdEntity>>> GetPageList(BmdListParam param, Pagination pagination)
        {
            TData<List<BmdEntity>> obj = new TData<List<BmdEntity>>();
            obj.Data = await bmdService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<BmdEntity>> GetEntity(long id)
        {
            TData<BmdEntity> obj = new TData<BmdEntity>();
            obj.Data = await bmdService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(BmdEntity entity)
        {
            TData<string> obj = new TData<string>();
            await bmdService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await bmdService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
