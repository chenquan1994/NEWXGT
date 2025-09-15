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
    /// 日 期：2025-08-23 15:02
    /// 描 述：矿机list控制器类
    /// </summary>
    [Area("OrganizationManage")]
    public class KuangListController :  BaseController
    {
        private KuangListBLL kuangListBLL = new KuangListBLL();

        #region 视图功能
        [AuthorizeFilter("organization:kuanglist:view")]
        public ActionResult KuangListIndex()
        {
            return View();
        }

        public ActionResult KuangListForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:kuanglist:search")]
        public async Task<ActionResult> GetListJson(KuangListListParam param)
        {
            TData<List<KuangListEntity>> obj = await kuangListBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:kuanglist:search")]
        public async Task<ActionResult> GetPageListJson(KuangListListParam param, Pagination pagination)
        {
            TData<List<KuangListEntity>> obj = await kuangListBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<KuangListEntity> obj = await kuangListBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:kuanglist:add,organization:kuanglist:edit")]
        public async Task<ActionResult> SaveFormJson(KuangListEntity entity)
        {
            TData<string> obj = await kuangListBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:kuanglist:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await kuangListBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
