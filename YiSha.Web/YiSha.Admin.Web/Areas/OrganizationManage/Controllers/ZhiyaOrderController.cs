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
    /// 描 述：质押订单控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class ZhiyaOrderController :  BaseController
    {
        private ZhiyaOrderBLL zhiyaOrderBLL = new ZhiyaOrderBLL();

        #region 视图功能
        [AuthorizeFilter("organization:zhiyaorder:view")]
        public ActionResult ZhiyaOrderIndex()
        {
            return View();
        }

        public ActionResult ZhiyaOrderForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:zhiyaorder:search")]
        public async Task<ActionResult> GetListJson(ZhiyaOrderListParam param)
        {
            TData<List<ZhiyaOrderEntity>> obj = await zhiyaOrderBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:zhiyaorder:search")]
        public async Task<ActionResult> GetPageListJson(ZhiyaOrderListParam param, Pagination pagination)
        {
            TData<List<ZhiyaOrderEntity>> obj = await zhiyaOrderBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<ZhiyaOrderEntity> obj = await zhiyaOrderBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:zhiyaorder:add,organization:zhiyaorder:edit")]
        public async Task<ActionResult> SaveFormJson(ZhiyaOrderEntity entity)
        {
            TData<string> obj = await zhiyaOrderBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:zhiyaorder:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await zhiyaOrderBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
