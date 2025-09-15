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
    /// 日 期：2025-08-22 22:41
    /// 描 述：临时支付回调参数业务类
    /// </summary>
    public class LinshiZhifuBLL
    {
        private LinshiZhifuService linshiZhifuService = new LinshiZhifuService();

        #region 获取数据
        public async Task<TData<List<LinshiZhifuEntity>>> GetList(LinshiZhifuListParam param)
        {
            TData<List<LinshiZhifuEntity>> obj = new TData<List<LinshiZhifuEntity>>();
            obj.Data = await linshiZhifuService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<LinshiZhifuEntity>>> GetPageList(LinshiZhifuListParam param, Pagination pagination)
        {
            TData<List<LinshiZhifuEntity>> obj = new TData<List<LinshiZhifuEntity>>();
            obj.Data = await linshiZhifuService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<LinshiZhifuEntity>> GetEntity(long id)
        {
            TData<LinshiZhifuEntity> obj = new TData<LinshiZhifuEntity>();
            obj.Data = await linshiZhifuService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(LinshiZhifuEntity entity)
        {
            TData<string> obj = new TData<string>();
            await linshiZhifuService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await linshiZhifuService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
