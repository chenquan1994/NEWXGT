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
    /// 日 期：2025-08-23 12:22
    /// 描 述：明细控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class MxController :  BaseController
    {
        private MxBLL mxBLL = new MxBLL();

        #region 视图功能
        [AuthorizeFilter("organization:mx:view")]
        public ActionResult MxIndex()
        {
            return View();
        }

        public ActionResult MxForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:mx:search")]
        public async Task<ActionResult> GetListJson(MxListParam param)
        {
            TData<List<MxEntity>> obj = await mxBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:mx:search")]
        public async Task<ActionResult> GetPageListJson(MxListParam param, Pagination pagination)
        {
            TData<List<MxEntity>> obj = await mxBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<MxEntity> obj = await mxBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:mx:add,organization:mx:edit")]
        public async Task<ActionResult> SaveFormJson(MxEntity entity)
        {
            TData<string> obj = await mxBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:mx:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await mxBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
