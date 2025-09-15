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
    /// 日 期：2025-08-22 00:22
    /// 描 述：控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class MoneyController :  BaseController
    {
        private MoneyBLL moneyBLL = new MoneyBLL();

        #region 视图功能
        [AuthorizeFilter("organization:money:view")]
        public ActionResult MoneyIndex()
        {
            return View();
        }

        public ActionResult MoneyForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:money:search")]
        public async Task<ActionResult> GetListJson(MoneyListParam param)
        {
            TData<List<MoneyEntity>> obj = await moneyBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:money:search")]
        public async Task<ActionResult> GetPageListJson(MoneyListParam param, Pagination pagination)
        {
            TData<List<MoneyEntity>> obj = await moneyBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<MoneyEntity> obj = await moneyBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:money:add,organization:money:edit")]
        public async Task<ActionResult> SaveFormJson(MoneyEntity entity)
        {
            TData<string> obj = await moneyBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:money:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await moneyBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
