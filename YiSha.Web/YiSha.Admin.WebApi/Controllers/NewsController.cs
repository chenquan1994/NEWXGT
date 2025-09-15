using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private NewsBLL newsBLL = new NewsBLL();

        private readonly IConfiguration _configuration;
        private readonly MiaContext _context;

        public NewsController(IConfiguration configuration, MiaContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        #region 获取数据
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<NewsEntity>>> GetPageList([FromQuery] NewsListParam param, [FromQuery] Pagination pagination)
        {
            TData<List<NewsEntity>> obj = await newsBLL.GetPageList(param, pagination);
            return obj;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<NewsEntity>>> GetPageContentList([FromQuery]NewsListParam param, [FromQuery]Pagination pagination)
        {
            TData<List<NewsEntity>> obj = await newsBLL.GetPageContentList(param, pagination);
            return obj;
        }

        /// <summary>
        /// 获取文章内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<NewsEntity>> GetForm([FromQuery]long id)
        {
            TData<NewsEntity> obj = await newsBLL.GetEntity(id);
            return obj;
        }
        #endregion

        [HttpGet]

        public dynamic GetNewsList()
        {
            var list = _context.cq_news.Where(t => t.NewsType == 1).ToList();

            return new { msg = "获取成功", list = list, state = "success" };
        }

        [HttpGet]

        public dynamic GetNewsId([FromQuery] string id)
        {
            var list = _context.cq_news.Where(t => t.Id == long.Parse(id)).ToList();

            return new { msg = "获取成功", list = list, state = "success" };

        }


        #region 提交数据
        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<string>> SaveForm([FromBody]NewsEntity entity)
        {
            TData<string> obj = await newsBLL.SaveForm(entity);
            return obj;
        }

        [HttpPost]
        public async Task<TData<string>> SaveViewTimes([FromBody]IdParam param)
        {
            TData<string> obj = null;
            TData<NewsEntity> objNews = await newsBLL.GetEntity(param.Id.Value);
            NewsEntity newsEntity = new NewsEntity();
            if (objNews.Data != null)
            {
                newsEntity.Id = param.Id.Value;
                newsEntity.ViewTimes = objNews.Data.ViewTimes;
                newsEntity.ViewTimes++;
                obj = await newsBLL.SaveForm(newsEntity);
            }
            else
            {
                obj = new TData<string>();
                obj.Message = "文章不存在";
            }
            return obj;
        }
        #endregion
    }
}