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
    /// 日 期：2025-08-23 16:07
    /// 描 述：释放明细控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class SfMxController :  BaseController
    {
        private SfMxBLL sfMxBLL = new SfMxBLL();

        #region 视图功能
        [AuthorizeFilter("organization:sfmx:view")]
        public ActionResult SfMxIndex()
        {
            return View();
        }

        public ActionResult SfMxForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:sfmx:search")]
        public async Task<ActionResult> GetListJson(SfMxListParam param)
        {
            TData<List<SfMxEntity>> obj = await sfMxBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:sfmx:search")]
        public async Task<ActionResult> GetPageListJson(SfMxListParam param, Pagination pagination)
        {
            TData<List<SfMxEntity>> obj = await sfMxBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<SfMxEntity> obj = await sfMxBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:sfmx:add,organization:sfmx:edit")]
        public async Task<ActionResult> SaveFormJson(SfMxEntity entity)
        {
            TData<string> obj = await sfMxBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:sfmx:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await sfMxBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
