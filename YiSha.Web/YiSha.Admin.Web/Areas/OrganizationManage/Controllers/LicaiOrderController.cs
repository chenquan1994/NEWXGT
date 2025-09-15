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
    /// 日 期：2025-08-22 20:25
    /// 描 述：理财订单控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class LicaiOrderController :  BaseController
    {
        private LicaiOrderBLL licaiOrderBLL = new LicaiOrderBLL();

        #region 视图功能
        [AuthorizeFilter("organization:licaiorder:view")]
        public ActionResult LicaiOrderIndex()
        {
            return View();
        }

        public ActionResult LicaiOrderForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:licaiorder:search")]
        public async Task<ActionResult> GetListJson(LicaiOrderListParam param)
        {
            TData<List<LicaiOrderEntity>> obj = await licaiOrderBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:licaiorder:search")]
        public async Task<ActionResult> GetPageListJson(LicaiOrderListParam param, Pagination pagination)
        {
            TData<List<LicaiOrderEntity>> obj = await licaiOrderBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<LicaiOrderEntity> obj = await licaiOrderBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:licaiorder:add,organization:licaiorder:edit")]
        public async Task<ActionResult> SaveFormJson(LicaiOrderEntity entity)
        {
            TData<string> obj = await licaiOrderBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:licaiorder:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await licaiOrderBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
