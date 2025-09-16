using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum;
using YiSha.IdGenerator;
using YiSha.Model;
using YiSha.Model.Result.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]

    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MiaContext _context;

        public AdminController(IConfiguration configuration, MiaContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        //获取用户列表已经余额

        [HttpGet]
        public dynamic Getuserlist([FromQuery] int yema, [FromQuery] int yezhi)
        {
            //社区结点

            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();
            var user = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            if (user.dizhi != "0x1595b935dae18bf57992d95dba81c2fd5e3cdbcc")
            {
                return new { msg = "没有权限", state = "error" };

            }


            var shequjiedian = from a in _context.cq_user
                               join b in _context.cq_money on a.Id equals b.User_id

                               select new
                               {
                                   tuandui=_context.cq_user.Where(t=>t.f_id.ToString().Contains(b.User_id.ToString())).ToList().Count(),
                                   ID = a.Id.ToString(),
                                   zhitui=a.zhitui,
                                   jibie=a.jibie,
                                   b.usdk,
                                   b.usdt,
                                   b.xgt,
                               };

            return new { msg = "获取成功", list = shequjiedian.Skip((yema - 1) * yezhi).Take(yezhi).ToList(), state = "success" };

        }


        [HttpGet]

        public dynamic test()
        {
            return Ok();
        }

        [HttpGet]


        //手动入金
        public dynamic rujin([FromQuery] string dizhi, [FromQuery] string sum)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();
            var user2 = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            //if (user2.dizhi != "0x1595b935dae18bf57992d95dba81c2fd5e3cdbcc")
            //{
            //    return new { msg = "没有权限", state = "error" };

            //}

            var user = _context.cq_user.Where(t => t.dizhi == dizhi).FirstOrDefault();

            user.hz_state = 1;
            user.dh_state = 1;
            user.tx_state = 1;
            user.zy_state = 1;




            OrderEntity order = new OrderEntity();

            decimal money = decimal.Parse(sum);

            order.yuanjia = decimal.Parse(sum);
            double dazhe = double.Parse(sum) * 0.3;
            double shiji = (double.Parse(sum)) - dazhe;




            double usdk = (double)money * 0.3;
            var usermoney = _context.cq_money.Where(t => t.User_id == user.Id).FirstOrDefault();


            order.money = decimal.Parse(shiji.ToString());
            order.state = 0;
            order.type = 2;
            order.Id = IdGeneratorHelper.Instance.GetId();
            order.user_id = user.Id;
            order.sf_state = 0;
            order.datetime = System.DateTime.Now;
            order.sd_rj = 1;

    



            double meishifangmoney = 0;
            if (order.yuanjia >= 100 && order.yuanjia <= 499)
            {
                meishifangmoney = (double)(order.yuanjia * 2);
            }


            else if (order.yuanjia >= 500 && order.yuanjia < 1000)
            {
                meishifangmoney = (double)(double.Parse(order.yuanjia.ToString()) * 2.5);
            }


            else if (order.yuanjia >= 1000 && order.yuanjia < 3000)
            {
                meishifangmoney = (double)(double.Parse(order.yuanjia.ToString()) * 3);
            }

            else if (order.yuanjia >= 3000 && order.yuanjia < 5000)
            {
                meishifangmoney = (double)(double.Parse(order.yuanjia.ToString()) * 3.5);
            }


            else
            {
                meishifangmoney = (double)(double.Parse(order.yuanjia.ToString()) * 4);
            }



            LicaiOrderEntity licai = new LicaiOrderEntity();
            licai.Id = IdGeneratorHelper.Instance.GetId();
            licai.state = 1;
            licai.datetime = DateTime.Now;
            licai.sf_money = 0;
            licai.wsf_money = (decimal?)meishifangmoney;
            licai.User_id = user.Id;
            licai.sf_datetime = DateTime.Now;
            licai.money = order.yuanjia;




        
            _context.Add(licai);
            _context.SaveChanges();
            string ddid = order.Id.ToString();
            return new { msg = "订单创建成功", order = order.Id.ToString(), dazhemoney = shiji, state = "success" };










        }

        [HttpGet]
        public dynamic dengji()
        {
            var list = _context.cq_user.Where(t=>t.jibie==0).ToList();

            foreach (var item in list)
            {

                var user = _context.cq_user.Where(t => t.zt_id == item.Id).ToList();

                List<double> values = new List<double>();

                for (int i = 0; i < user.Count; i++)
                {
                    var jine = from a in _context.cq_order
                               join b in _context.cq_user on a.user_id equals b.Id

                               where b.f_id.Contains(user[i].Id.ToString())

                               select new
                               {
                                   a.yuanjia
                               };

                    var licai = _context.cq_licai_order.Where(t => t.User_id == user[i].Id).Sum(t => t.money);

                    values.Add((double)jine.Sum(t => t.yuanjia) + double.Parse(licai.ToString()));
                }


                var sortedAndRemoved = values.OrderByDescending(n => n).Skip(1).ToList();


                double yeji = 0;


                foreach (var item2 in sortedAndRemoved)
                {
                    yeji += item2;

                }

                int dengji = 0;

                if (yeji >= 3000 && yeji < 10000)
                {
                    dengji = 1;
                }

                if (yeji >= 10000 && yeji < 30000)
                {
                    dengji = 2;
                }

                if (yeji >= 30000 && yeji < 80000)
                {
                    dengji = 3;
                }

                if (yeji >= 80000 && yeji < 200000)
                {
                    dengji = 4;
                }

                if (yeji >= 200000 && yeji < 600000)
                {
                    dengji = 5;
                }

                if (yeji >= 600000 && yeji < 1000000)
                {
                    dengji = 6;
                }

                if (yeji >= 1000000 && yeji <= 1000000000)
                {
                    dengji = 7;
                }


                int dj = dengji;

                if (item.jibie < dj)
                {
                    item.jibie = dj;

                    _context.SaveChanges();


                }






            }


            return Ok();


        }



        //提现表
        [HttpGet]
        public dynamic txlist([FromQuery] int state, [FromQuery] int yema, [FromQuery] int yezhi)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var user2 = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

           
            var sql = from a in _context.cq_tx
                      join b in _context.cq_user on a.user_id equals b.Id

                      where a.state==state
                      select new
                      {
                          Id=a.Id.ToString(),
                          a.money,
                          a.datetime,
                          a.state,
                          b.dizhi
                          ,b.biaoshi
                      };

            var list = sql.OrderByDescending(t => t.datetime).Skip((yema - 1) * yezhi) // 跳过前面的记录
                       .Take(yezhi);


            return new { msg = "获取成功", list = list.ToList(), state = "success" };

        }


        //申请提现

        [HttpPost]

        public async Task<dynamic> Cltx(TxEntity tx)
        {
            var list = _context.cq_tx.Where(t => t.Id == tx.Id && t.state == 0).ToList();


            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var user2 = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

          

            string msg = "";
            string state = "";

            if (list.Count > 0)
            {

                foreach (var item in list)
                {

                    if (item.state == 0)
                    {
                        var dizhi = _context.cq_user.Where(t => t.Id == item.user_id).FirstOrDefault();



                        var data = new
                        {
                            to_address = dizhi.dizhi,
                            amount = item.money.ToString()

                        };
                        string json = JsonConvert.SerializeObject(data); // 使用Jso

                        var url = "https://qqq.usxgt.com/api/v1/transfer/usdt";
                        var client = new HttpClient();

                        client.DefaultRequestHeaders.Add("X-API-Key", "your_api_key_1_here");
                        client.DefaultRequestHeaders.Accept.Add(
                  new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));


                        string body = await response.Content.ReadAsStringAsync();

                        var peron = JsonConvert.DeserializeObject<Person>(body);




                        if (peron.success == "true")
                        {
                            item.state = 1;

                            state = "success";
                            msg = "处理成功";

                            _context.SaveChanges();



                        }
                        else
                        {
                            state = "error";
                            msg = "处理失败";

                        }



                    }
                    else
                    {
                        state = "error";
                        msg = "已经处理过";
                    }


                }
                return new { msg =msg, state = state };

            }

            else
            {
                return new { msg = "已处理", state = "error" };

            }
        }

     



        [HttpGet]
        public dynamic updatestate([FromQuery]string dizhi)
        {
            var user = _context.cq_user.Where(t => t.dizhi == dizhi).FirstOrDefault();

            user.sf_state = 1;
            user.zy_state = 1;
            user.hz_state = 1;
            user.tx_state = 1;

            _context.SaveChanges();

            return Ok();


        }
    }


 
        


        class Person
        {
            public string success { get; set; }
            public string tx_hash { get; set; }
            public string message { get; set; }
            public string money { get; set; }
        }
    }

