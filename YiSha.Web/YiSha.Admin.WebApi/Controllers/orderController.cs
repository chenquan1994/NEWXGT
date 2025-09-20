using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YiSha.Entity.OrganizationManage;
using YiSha.IdGenerator;
using YiSha.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class orderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MiaContext _context;

        public orderController(IConfiguration configuration, MiaContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<dynamic> Addorder(OrderEntity order)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();


            var user = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            var bmd= _context.cq_bmd.Where(t => t.User_id == long.Parse(user_id)).ToList();

            if (bmd.Count() <= 0)
            {
                return new { msg = "请联系管理员开通权限", state = "error" };

            }



            var url = "https://qqq.usxgt.com/api/v1/balance/usdt/"+user.dizhi+"";
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("x-api-key", "your_api_key_1_here");
            var response = await client.GetAsync(url);


            string body = await response.Content.ReadAsStringAsync();

            var peron = JsonConvert.DeserializeObject<yue>(body);

            if (peron.success == "true")
            {
                if (peron.balance < 5000)
                {
                    return new { msg = "USDT余额不足",  state = "error" };

                }
                else
                {
                    order.state = 0;
                    order.type = 1;
                    order.sf_state = 0;
                    order.yuanjia = order.money;
                    order.Id = IdGeneratorHelper.Instance.GetId();
                    order.user_id = long.Parse(user_id);
                    order.datetime = System.DateTime.Now;
                    _context.Add(order);
                    _context.SaveChanges();
                    return new { msg = "订单创建成功", order = order.Id.ToString(), state = "success" };
                }
              
            }
            else
            {
                return new { msg = "系统错误", state = "error" };

            }

        }


  

        class yue
        {
            public string success { get; set; }
            public decimal balance { get; set; }

            public string address { get; set; }
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<string> NotifyPayment(OrderEntity order)
        {

            var orderlist = _context.cq_order.Where(t => t.Id == long.Parse(order.orderId) && t.money == decimal.Parse(order.amount) && t.state == 0).ToList();
            var orderlist2 = _context.cq_order.Where(t => t.Id == long.Parse(order.orderId) && t.money == decimal.Parse(order.amount) && t.state == 0).FirstOrDefault();

            var zijiuser = _context.cq_user.Where(t => t.Id == long.Parse(orderlist2.user_id.ToString())).FirstOrDefault();


            var shangji = _context.cq_user.Where(t => t.Id == zijiuser.zt_id).FirstOrDefault();





            if (orderlist.Count == 1)
            {

                var url = "https://qqq.usxgt.com/api/v1/transaction/" + order.txHash + "";
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add("x-api-key", "your_api_key_1_here");
                var response = await client.GetAsync(url);


                string body = await response.Content.ReadAsStringAsync();

                var peron = JsonConvert.DeserializeObject<Person>(body);

                if (peron.success == "true")
                {
                    var orderinfo = _context.cq_order.Where(t => t.Id == long.Parse(order.orderId)).FirstOrDefault();

                    var user = _context.cq_user.Where(t => t.Id == long.Parse(orderinfo.user_id.ToString())).FirstOrDefault();

                    if (peron.from_address.ToLower() != user.dizhi)
                    {
                        return "无效哈希值";

                    }
                    var shoukuandizhi = _context.cq_money_dizhi.FirstOrDefault();


                    if (peron.to_address!= shoukuandizhi.dizhi)
                    {
                        return "无效哈希值";

                    }

                    if (decimal.Parse(peron.usdt_amount) != orderinfo.money)
                    {
                        return "无效哈希值";

                    }

                    orderinfo.state = 1;


                    switch (orderinfo.type)
                    {
                        case 1:
                            KuangListEntity k = new KuangListEntity();
                            k.Id = IdGeneratorHelper.Instance.GetId();
                            k.state = 1;
                            k.datetime = DateTime.Now;
                            k.sf_money = 0;
                            k.sf_datetime = DateTime.Now;
                            k.wsf_money = orderinfo.money * 4;
                            k.user_id = orderinfo.user_id;
                            k.title = "社区节点包";
                            k.money = orderinfo.money;
                            _context.Add(k);
                            _context.SaveChanges();
                            break;


                        case 2:
                            LicaiOrderEntity licai = new LicaiOrderEntity();

                            var usermoney = _context.cq_money.Where(t => t.User_id == long.Parse(orderlist2.user_id.ToString())).FirstOrDefault();

                            double koudiaousdk = (double)orderlist2.yuanjia * 0.3;
                            double meishifangmoney = 0;
                            if (orderinfo.yuanjia >= 100 && orderinfo.yuanjia <= 499)
                            {
                                meishifangmoney = (double)(orderinfo.yuanjia * 2);
                            }


                            else if (orderinfo.yuanjia >= 500 && orderinfo.yuanjia < 1000)
                            {
                                meishifangmoney = (double)(double.Parse(orderinfo.yuanjia.ToString()) * 2.5);
                            }


                            else if (orderinfo.yuanjia >= 1000 && orderinfo.yuanjia < 3000)
                            {
                                meishifangmoney = (double)(double.Parse(orderinfo.yuanjia.ToString()) * 3);
                            }

                            else if (orderinfo.yuanjia >= 3000 && orderinfo.yuanjia < 5000)
                            {
                                meishifangmoney = (double)(double.Parse(orderinfo.yuanjia.ToString()) * 3.5);
                            }


                            else
                            {
                                meishifangmoney = (double)(double.Parse(orderinfo.yuanjia.ToString()) * 4);
                            }
                            usermoney.usdk -= (decimal)koudiaousdk;
                            licai.Id = IdGeneratorHelper.Instance.GetId();
                            licai.state = 1;
                            licai.datetime = DateTime.Now;
                            licai.sf_money = 0;
                            licai.wsf_money = (decimal?)meishifangmoney;
                            licai.User_id = orderinfo.user_id;
                            licai.sf_datetime = DateTime.Now;
                            licai.money = orderinfo.yuanjia;
                            shangji.yeji += orderinfo.yuanjia;




                            MxEntity mx = new MxEntity();
                            mx.Id = IdGeneratorHelper.Instance.GetId();
                            mx.pay_type = "USDK";
                            mx.type = 2;
                            mx.title = "投资理财消耗"+koudiaousdk;
                            mx.user_id = orderinfo.user_id;
                            mx.datetime = DateTime.Now;
                            _context.Add(mx);
                            _context.SaveChanges();


                            _context.Add(licai);
                            _context.SaveChanges();

                            dengji((long)zijiuser.zt_id);


                            break;




                    }

                 

                    _context.SaveChanges();
                    return "成功";

                }
                else
                {
                    return "交易失败";
                }



            }
            else
            {
                return "无效订单";
            }
        }


        public void dengji(long user_id)
        {
            var list = _context.cq_user.Where(t => t.Id == user_id).ToList();

            foreach (var item in list)
            {

                var user = _context.cq_user.Where(t => t.zt_id == item.Id).ToList();

                List<double> values = new List<double>();

                for (int i = 0; i < user.Count; i++)
                {
                    var jine = from a in _context.cq_order
                               join b in _context.cq_user on a.user_id equals b.Id

                               where b.f_id.Contains(user[i].Id.ToString()) && a.type == 2 && a.state == 1
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

            
        }



        class Person
        {
            public string success { get; set; }
            public string usdt_amount { get; set; }
            public string from_address { get; set; }
            public string to_address { get; set; }
        }


        [HttpPost]

        public async Task<dynamic> Addlicaiorder(OrderEntity order)
        {

            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var user=_context.cq_user.Where(t=>t.Id==long.Parse(user_id)).FirstOrDefault();
            var url = "https://qqq.usxgt.com/api/v1/balance/usdt/" + user.dizhi + "";


            var client = new HttpClient();

            decimal money = (decimal)order.money;

            order.yuanjia = order.money;
            double dazhe = (double)order.money * 0.3;
            double shiji = (double)order.money - dazhe;

            client.DefaultRequestHeaders.Add("x-api-key", "your_api_key_1_here");
            var response = await client.GetAsync(url);


            string body = await response.Content.ReadAsStringAsync();

            var peron = JsonConvert.DeserializeObject<yue>(body);

            if (peron.success == "true")
            {
                if (peron.balance < (decimal)shiji)
                {
                    return new { msg = "USDT余额不足", state = "error" };

                }
                else
                {
                 

                    double usdk = (double)money * 0.3;
                    var usermoney = _context.cq_money.Where(t => t.User_id == long.Parse(user_id)).FirstOrDefault();

                    if (usermoney.usdk < (decimal)usdk)
                    {
                        return new { msg = "USDK余额不足", state = "error" };
                    }


           

                  
                    order.money = decimal.Parse(shiji.ToString());
                    order.state = 0;
                    order.type = 2;
                    order.Id = IdGeneratorHelper.Instance.GetId();
                    order.user_id = long.Parse(user_id);
                    order.sf_state = 0;
                    order.datetime = System.DateTime.Now;
                     _context.Add(order);
                    _context.SaveChanges();
                    string ddid = order.Id.ToString();
                    return new { msg = "订单创建成功", order = order.Id.ToString(), dazhemoney = shiji, state = "success" };
                }
            }

            else
            {
                return new { msg = "系统错误", state = "error" };

            }

        }

        [HttpPost]
        public dynamic Addzhiya(ZhiyaOrderEntity order)
        {

            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var userlist = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            if (userlist.zy_state == 1)
            {
                return new { msg = "质押失败,请联系管理员开通", state = "error" };
            }

            if (order.day == 7)
            {
                var zhiya = _context.cq_zhiya_order.Where(t => t.user_id == long.Parse(user_id) && t.day == 7).ToList();
                if (zhiya.Count > 0)
                {
                    return new { msg = "质押失败,已体验过7天", state = "error" };
                }
            }


            if (order.day == 15)
            {
                var zhiya2 = _context.cq_zhiya_order.Where(t => t.user_id == long.Parse(user_id) && t.day == 15).ToList();
                if (zhiya2.Count > 0)
                {
                    return new { msg = "质押失败,已体验过15天", state = "error" };
                }

            }

          

            decimal money = (decimal)order.money;



            var usermoney = _context.cq_money.Where(t => t.User_id == long.Parse(user_id)).FirstOrDefault();

            if (usermoney.usdt < (decimal)money)
            {
                return new { msg = "USdt余额不足", state = "error" };
            }

            double beilv = 0;
            switch (int.Parse(order.day.ToString()))
            {
                case 7:
                    beilv = 0.01;
                    break;
                case 15:
                    beilv = 0.011;
                    break;
                case 30:
                    beilv = 0.013;
                    break;
                case 90:
                    beilv = 0.015;
                    break;
                case 180:
                    beilv = 0.018;
                    break;
                case 360:
                    beilv = 0.02;
                    break;


            }


            //释放金额
            double shifang = double.Parse(order.money.ToString()) * beilv * int.Parse(order.day.ToString());


            order.state = 1;
            order.Id = IdGeneratorHelper.Instance.GetId();
            order.user_id = long.Parse(user_id);

            order.sf_datetime = DateTime.Now;
            order.sf_money = 0;
            order.wsf_money = (decimal)shifang;
            order.datetime = System.DateTime.Now;
            usermoney.usdt -= (decimal)money;
            _context.Add(order);
            _context.SaveChanges();
            string ddid = order.Id.ToString();


            MxEntity mx = new MxEntity();
            mx.Id = IdGeneratorHelper.Instance.GetId();
            mx.pay_type = "USDT";
            mx.type = 2;
            mx.title = "质押" + order.money;
            mx.user_id = long.Parse(user_id);
            mx.datetime = DateTime.Now;
            _context.Add(mx);
            _context.SaveChanges();

            return new
            {
                msg = "订单创建成功",
                order = order.Id.ToString()


            };

        }



        [HttpGet]

        public dynamic suancuole()
        {

            DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime yesterdayStart = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day);
            DateTime yesterdayEnd = yesterdayStart.AddDays(1).AddTicks(-1); // 到昨天结束的时间

            var list = _context.cq_mx.Where(t => t.datetime >= yesterdayStart&&t.datetime<yesterdayEnd&&t.pay_type=="XGT"&&t.type==1).ToList();

           
            foreach(var item in list)
            {
                var money = _context.cq_money.Where(t => t.User_id == item.user_id).FirstOrDefault();

                double jiaqian = (double)item.money * 0.15;
                money.xgt += (decimal)jiaqian;
                var list2 = _context.cq_mx.Where(t => t.Id == item.Id).FirstOrDefault();
                list2.money += (decimal)jiaqian;
                _context.SaveChanges();



            }

            return Ok();
        }


 


        [HttpGet]
        //获取社区节点list

        public dynamic Getshequlist([FromQuery] int yema, [FromQuery] int yezhi)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();
            var list = _context.cq_kuang_order.Where(t => t.user_id == long.Parse(user_id)).OrderByDescending(t => t.datetime).ToList();
            var touzi = _context.cq_kuang_order.Where(t => t.user_id == long.Parse(user_id)).Sum(t => t.money);
            var fenyong = _context.cq_sf_mx.Where(t => t.user_id == long.Parse(user_id)&&t.type=="USDK").Sum(t => t.money);
            return new { msg = "获取成功", list = list.Skip((yema - 1) * yezhi).Take(yezhi).ToList(), state = "success",touzi=touzi,fenyong=fenyong };


        }



        [HttpGet]
        //获取社区节点list

        public dynamic Getlicailist([FromQuery] int yema, [FromQuery] int yezhi)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();
            var list = _context.cq_licai_order.Where(t => t.User_id == long.Parse(user_id)).OrderByDescending(t => t.datetime).ToList();
          
            var touzi = _context.cq_licai_order.Where(t => t.User_id == long.Parse(user_id)).Sum(t => t.money);
            var fenyong = _context.cq_sf_mx.Where(t => t.user_id == long.Parse(user_id) && t.type == "USDT").Sum(t => t.money);

            return new { msg = "获取成功", list = list.Skip((yema - 1) * yezhi).Take(yezhi).ToList(), state = "success",touzi = touzi, fenyong = fenyong };



        }



        [HttpGet]
        //获取社区节点list

        public dynamic Getzhiyalist([FromQuery] int yema, [FromQuery] int yezhi)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();
            var list = _context.cq_zhiya_order.Where(t => t.user_id == long.Parse(user_id)).OrderByDescending(t => t.datetime).ToList();

            var touzi = _context.cq_zhiya_order.Where(t => t.user_id == long.Parse(user_id)).Sum(t => t.money);

            var fenyong = _context.cq_sf_mx.Where(t => t.user_id == long.Parse(user_id) && t.type == "XGT").Sum(t => t.money);


            return new { msg = "获取成功", list = list.Skip((yema - 1) * yezhi).Take(yezhi).ToList(), state = "success", touzi = touzi, fenyong = fenyong };


        }
    }
}
