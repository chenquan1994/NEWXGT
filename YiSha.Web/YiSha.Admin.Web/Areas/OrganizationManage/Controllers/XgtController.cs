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
    /// 日 期：2025-08-22 07:57
    /// 描 述：XGT动态表控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class XgtController :  BaseController
    {
        private XgtBLL xgtBLL = new XgtBLL();

        #region 视图功能
        [AuthorizeFilter("organization:xgt:view")]
        public ActionResult XgtIndex()
        {
            return View();
        }

        public ActionResult XgtForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:xgt:search")]
        public async Task<ActionResult> GetListJson(XgtListParam param)
        {
            TData<List<XgtEntity>> obj = await xgtBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:xgt:search")]
        public async Task<ActionResult> GetPageListJson(XgtListParam param, Pagination pagination)
        {
            TData<List<XgtEntity>> obj = await xgtBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<XgtEntity> obj = await xgtBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:xgt:add,organization:xgt:edit")]
        public async Task<ActionResult> SaveFormJson(XgtEntity entity)
        {
            TData<string> obj = await xgtBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:xgt:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await xgtBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
