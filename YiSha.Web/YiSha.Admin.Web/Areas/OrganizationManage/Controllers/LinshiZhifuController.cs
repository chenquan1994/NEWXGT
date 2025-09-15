using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Admin.Web.Controllers;
using YiSha.Entity.OrganizationManage;
using YiSha.Business.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2025-08-22 22:41
    /// 描 述：临时支付回调参数控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class LinshiZhifuController :  BaseController
    {
        private LinshiZhifuBLL linshiZhifuBLL = new LinshiZhifuBLL();

        #region 视图功能
        [AuthorizeFilter("organization:linshizhifu:view")]
        public ActionResult LinshiZhifuIndex()
        {
            return View();
        }

        public ActionResult LinshiZhifuForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:linshizhifu:search")]
        public async Task<ActionResult> GetListJson(LinshiZhifuListParam param)
        {
            TData<List<LinshiZhifuEntity>> obj = await linshiZhifuBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:linshizhifu:search")]
        public async Task<ActionResult> GetPageListJson(LinshiZhifuListParam param, Pagination pagination)
        {
            TData<List<LinshiZhifuEntity>> obj = await linshiZhifuBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<LinshiZhifuEntity> obj = await linshiZhifuBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:linshizhifu:add,organization:linshizhifu:edit")]
        public async Task<ActionResult> SaveFormJson(LinshiZhifuEntity entity)
        {
            TData<string> obj = await linshiZhifuBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:linshizhifu:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await linshiZhifuBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
