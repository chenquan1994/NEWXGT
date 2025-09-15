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
    /// 日 期：2025-08-23 12:01
    /// 描 述：提现控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class TxController :  BaseController
    {
        private TxBLL txBLL = new TxBLL();

        #region 视图功能
        [AuthorizeFilter("organization:tx:view")]
        public ActionResult TxIndex()
        {
            return View();
        }

        public ActionResult TxForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:tx:search")]
        public async Task<ActionResult> GetListJson(TxListParam param)
        {
            TData<List<TxEntity>> obj = await txBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:tx:search")]
        public async Task<ActionResult> GetPageListJson(TxListParam param, Pagination pagination)
        {
            TData<List<TxEntity>> obj = await txBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<TxEntity> obj = await txBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:tx:add,organization:tx:edit")]
        public async Task<ActionResult> SaveFormJson(TxEntity entity)
        {
            TData<string> obj = await txBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:tx:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await txBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
