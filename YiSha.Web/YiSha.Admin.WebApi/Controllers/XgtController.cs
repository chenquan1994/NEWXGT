using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class XgtController : ControllerBase
    {
      
      
        private readonly IConfiguration _configuration;
        private readonly MiaContext _context;

        public XgtController(IConfiguration configuration, MiaContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]

        //获取每日XGT价格
        public dynamic GetXGTmoney()
        {
            var list = _context.cq_xgt.OrderByDescending(t => t.datetime).ToList();
            return new { msg = "获取成功", list = list, state = "success" };
        }

        //获取官方收款地址

        [HttpGet]
        [AllowAnonymous]
        public dynamic GetmoneyDizhi()
        {
            var list = _context.cq_money_dizhi.ToList();
            return new { msg = "获取成功", list = list, state = "success" };
        }


        [HttpPost]
        public dynamic huazhuan(XgtEntity xgt)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var userlist = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            if (userlist.dh_state == 1)
            {
                return new { msg = "兑换失败,系统入金,请联系管理员开通", state = "error" };
            }



            var xgtmoney = _context.cq_xgt.OrderByDescending(t => t.datetime).FirstOrDefault();

            decimal usdt = (decimal)xgt.money * (decimal)xgtmoney.money;

            var money = _context.cq_money.Where(t => t.User_id == long.Parse(user_id)).FirstOrDefault();

            if (money.xgt < (decimal)xgt.money)
            {
                return new { msg = "兑换失败,余额不足", state = "error" };

            }

            money.usdt += usdt;
            money.xgt -= (decimal)xgt.money;

            _context.SaveChanges();

            return new { msg = "兑换成功", state = "success" };


        }

    }
}
