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
    /// 描 述：5000U订单控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class OrderController :  BaseController
    {
        private OrderBLL orderBLL = new OrderBLL();

        #region 视图功能
        [AuthorizeFilter("organization:order:view")]
        public ActionResult OrderIndex()
        {
            return View();
        }

        public ActionResult OrderForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:order:search")]
        public async Task<ActionResult> GetListJson(OrderListParam param)
        {
            TData<List<OrderEntity>> obj = await orderBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:order:search")]
        public async Task<ActionResult> GetPageListJson(OrderListParam param, Pagination pagination)
        {
            TData<List<OrderEntity>> obj = await orderBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<OrderEntity> obj = await orderBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:order:add,organization:order:edit")]
        public async Task<ActionResult> SaveFormJson(OrderEntity entity)
        {
            TData<string> obj = await orderBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:order:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await orderBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
