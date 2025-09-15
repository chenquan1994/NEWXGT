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
    /// 日 期：2025-09-06 11:47
    /// 描 述：白名单控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class BmdController :  BaseController
    {
        private BmdBLL bmdBLL = new BmdBLL();

        #region 视图功能
        [AuthorizeFilter("organization:bmd:view")]
        public ActionResult BmdIndex()
        {
            return View();
        }

        public ActionResult BmdForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:bmd:search")]
        public async Task<ActionResult> GetListJson(BmdListParam param)
        {
            TData<List<BmdEntity>> obj = await bmdBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:bmd:search")]
        public async Task<ActionResult> GetPageListJson(BmdListParam param, Pagination pagination)
        {
            TData<List<BmdEntity>> obj = await bmdBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<BmdEntity> obj = await bmdBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:bmd:add,organization:bmd:edit")]
        public async Task<ActionResult> SaveFormJson(BmdEntity entity)
        {
            TData<string> obj = await bmdBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:bmd:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await bmdBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
