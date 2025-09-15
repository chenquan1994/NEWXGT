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
    /// 日 期：2025-08-22 00:35
    /// 描 述：用户表控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class cqUserController :  BaseController
    {
        private cqUserBLL cqUserBLL = new cqUserBLL();

        #region 视图功能
        [AuthorizeFilter("organization:cquser:view")]
        public ActionResult cqUserIndex()
        {
            return View();
        }

        public ActionResult cqUserForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:cquser:search")]
        public async Task<ActionResult> GetListJson(cqUserListParam param)
        {
            TData<List<cqUserEntity>> obj = await cqUserBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:cquser:search")]
        public async Task<ActionResult> GetPageListJson(cqUserListParam param, Pagination pagination)
        {
            TData<List<cqUserEntity>> obj = await cqUserBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<cqUserEntity> obj = await cqUserBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:cquser:add,organization:cquser:edit")]
        public async Task<ActionResult> SaveFormJson(cqUserEntity entity)
        {
            TData<string> obj = await cqUserBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:cquser:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await cqUserBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
