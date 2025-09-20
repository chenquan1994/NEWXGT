using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
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
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]

    public class UserController : ControllerBase
    {
        private UserBLL userBLL = new UserBLL();
        private readonly IConfiguration _configuration;
        private readonly MiaContext _context;

        private object locker = new object();


        public UserController(IConfiguration configuration, MiaContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        #region 获取数据       
        #endregion

        #region 提交数据
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<TData<OperatorInfo>> Login([FromQuery] string userName, [FromQuery] string password)
        {
            TData<OperatorInfo> obj = new TData<OperatorInfo>();
            TData<UserEntity> userObj = await userBLL.CheckLogin(userName, password, (int)PlatformEnum.WebApi);
            if (userObj.Tag == 1)
            {
                await new UserBLL().UpdateUser(userObj.Data);
                await Operator.Instance.AddCurrent(userObj.Data.ApiToken);
                obj.Data = await Operator.Instance.Current(userObj.Data.ApiToken);
            }
            obj.Tag = userObj.Tag;
            obj.Message = userObj.Message;
            return obj;
        }

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public TData LoginOff([FromQuery] string token)
        {
            var obj = new TData();
            Operator.Instance.RemoveCurrent(token);
            obj.Message = "登出成功";
            return obj;
        }


        public string CreateToken(string User_id, string user_name)
        {
            // 1. 定义需要使用到的Claims
            var claims = new[]
            {
            new Claim("UId", User_id),
            new Claim("name", user_name),

        };


            // 2. 从 appsettings.json 中读取SecretKey
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // 3. 选择加密算法
            var algorithm = SecurityAlgorithms.HmacSha256;

            // 4. 生成Credentials
            var signingCredentials = new SigningCredentials(secretKey, algorithm);

            // 5. 根据以上，生成token

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],    //Issuer
                _configuration["Jwt:Audience"],  //Audience
                claims,                          //Claims,
                DateTime.Now,                    //notBefore
                DateTime.Now.AddDays(7),     //expires
                signingCredentials           //Credentials
            );

            // 6. 将token变为string
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;
        }


        //登录

        [HttpPost]
        [AllowAnonymous]
        public dynamic Userlogin(cqUserEntity cqUser)
        {

            string token = "";
            var list = _context.cq_user.Where(t => t.dizhi == cqUser.dizhi).ToList();




            //已创建用户
            if (list.Count > 0)
            {
                var list2 = _context.cq_user.Where(t => t.dizhi == cqUser.dizhi).FirstOrDefault();
                token = CreateToken(list2.Id.ToString(), cqUser.dizhi);
            }
            else
            {
                var f_user = _context.cq_user.Where(t => t.yqm == cqUser.yqm).FirstOrDefault();


                if (f_user != null)
                {
                    string yamcode = "";
                    Random random = new Random();
                    yamcode = random.Next(100000, 999999).ToString();
                    cqUserEntity user = new cqUserEntity();
                    user.Id = IdGeneratorHelper.Instance.GetId();
                    user.dizhi = cqUser.dizhi;
                    user.f_id = f_user.f_id + "," + f_user.Id;
                    user.zt_id = f_user.Id;
                    user.zhitui = 0;
                    user.datetitime = DateTime.Now;
                    user.yqm = yamcode;
                    user.jibie = 0;
                    user.yeji = 0;

                    _context.Add(user);
                    _context.SaveChanges();

                    MoneyEntity money = new MoneyEntity();
                    money.usdt = 0;
                    money.xgt = 0;
                    money.usdk = 0;
                    money.Id = IdGeneratorHelper.Instance.GetId();
                    money.User_id = user.Id;
                    f_user.zhitui += 1;

                    _context.Add(money);
                    _context.SaveChanges();

                    token = CreateToken(user.Id.ToString(), cqUser.dizhi);

                }
                else
                {
                    return new { state = "error", msg = "邀请码错误" };
                }





            }

            return new { state = "success", msg = "成功", token = token };



        }



        //获取个人中心


        [HttpGet]
        [AllowAnonymous]
        public dynamic GetUserInfo()
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var td = _context.cq_user.Where(t => t.zt_id == long.Parse(user_id)).ToList();

            DateTime ks = DateTime.Today.Add(new TimeSpan(0, 0, 0));
            DateTime js = DateTime.Today.Add(new TimeSpan(23, 59, 59));


            //我的团队
            var tuandui = _context.cq_user.Where(t => t.f_id.Contains(user_id.ToString())).ToList();


            //今日新增团队
            var xzzhitui = _context.cq_user.Where(t => t.f_id.Contains(user_id.ToString()) && t.datetitime >= ks && t.datetitime < js).ToList();


            //社区结点

            var shequjiedian = from a in _context.cq_user
                               join b in _context.cq_kuang_order on a.Id equals b.user_id

                               where a.f_id.Contains(user_id)
                               select new
                               {
                                   a.Id
                               };



            //今日新增社区结点

            var jinrixinzen = from a in _context.cq_user
                              join b in _context.cq_kuang_order on a.Id equals b.Id

                              where a.f_id.Contains(user_id) && b.datetime >= ks && b.datetime < js
                              select new
                              {
                                  a.Id
                              };

            var jinriyeji = from a in _context.cq_user
                            join b in _context.cq_order on a.Id equals b.user_id
                            where a.f_id.Contains(user_id) && b.datetime >= ks && b.datetime < js && b.type == 2 && b.state == 1
                            select new
                            {
                                b.yuanjia,

                            };


            var jinrimoney = jinriyeji.Sum(t => t.yuanjia);

            //今日新增USDT

            var xinzengUSDT = _context.cq_mx.Where(t => t.pay_type == "USDT" &&t.title!="转入" && t.type == 1 && t.user_id == long.Parse(user_id.ToString()) && t.datetime >= ks && t.datetime < js).Sum(t => t.money);


            //今日新增XGT
            var xinzengXGT = _context.cq_mx.Where(t => t.pay_type == "XGT" && t.title != "转入" && t.type == 1 && t.user_id == long.Parse(user_id.ToString()) && t.datetime >= ks && t.datetime < js).Sum(t => t.money);


            var xgtjg = _context.cq_xgt.OrderByDescending(t => t.datetime).FirstOrDefault();


            //XGT转换USDT

            double zhuanhuanXGT = (double)(xinzengXGT * xgtjg.money);

            //今日收益

            decimal jinrixinzengmoney = (decimal)xinzengUSDT + (decimal)zhuanhuanXGT;


            //总资金

            var zijinmoney = _context.cq_money.Where(t => t.User_id == long.Parse(user_id)).FirstOrDefault();

            double zijixgt = (double)(xgtjg.money * zijinmoney.xgt);


            decimal zongzijin = (decimal)zijinmoney.usdt + (decimal)zijixgt;



            //团队业绩

            var user = _context.cq_user.Where(t => t.zt_id == long.Parse(user_id)).ToList();

            List<double> values = new List<double>();


            
                var jine = from a in _context.cq_order
                           join b in _context.cq_user on a.user_id equals b.Id
                           where b.f_id.Contains(user_id) && a.type == 2 && a.state == 1

                           select new
                           {
                               a.yuanjia
                           };

 


        

                values.Add((double)jine.Sum(t => t.yuanjia));
 


 

            double yeji = 0;


            foreach (var item2 in values)
            {
                yeji += item2;

            }
            var tuanduiyejimoney = yeji;


            var list = from a in _context.cq_user
                       join b in _context.cq_money on a.Id equals b.User_id

                       where a.Id == long.Parse(user_id)

                       select new
                       {
                           xiaoquyeji= dengji(long.Parse(user_id)),
                           yue = zongzijin,
                           tuandui = tuandui.Count,
                           xzzhitui = xzzhitui.Count,
                           shenqujinrixinzeng = jinrixinzen.ToList().Count,
                           xinzengyeji = jinrimoney,
                           jinrishouyi = jinrixinzengmoney,
                           shequjiedian = shequjiedian.ToList().Count,
                           tuanduiyeji = tuanduiyejimoney,
                           zt = td.Count,
                           Id = a.Id.ToString(),
                           zjc = a.zjc == null ? "false" : "true",
                           zfpass = a.zf_pass == null ? "false" : "true",
                           a.yeji,
                           a.dizhi,
                           a.jibie,
                           a.yqm,
                           b.usdk,
                           b.usdt,
                           b.xgt
                       };


            return new { msg = "获取成功", list = list, state = "success", tuandui = td.Count };

        }


        public decimal dengji(long user_id)
        {
            var list = _context.cq_user.Where(t => t.Id==user_id).ToList();

            double yeji = 0;
            foreach (var item in list)
            {

                var user = _context.cq_user.Where(t => t.zt_id == item.Id).ToList();

                List<double> values = new List<double>();

                for (int i = 0; i < user.Count; i++)
                {
                    var jine = from a in _context.cq_order
                               join b in _context.cq_user on a.user_id equals b.Id

                               where b.f_id.Contains(user[i].Id.ToString())&&a.type==2&&a.state==1

                               select new
                               {
                                   a.yuanjia
                               };

                    var licai = _context.cq_licai_order.Where(t => t.User_id == user[i].Id).Sum(t => t.money);

                    values.Add((double)jine.Sum(t => t.yuanjia) + double.Parse(licai.ToString()));
                }


                var sortedAndRemoved = values.OrderByDescending(n => n).Skip(1).ToList();




                foreach (var item2 in sortedAndRemoved)
                {
                    yeji += item2;

                }

              






            }


            return (decimal)yeji;


        }




        [HttpGet]

        public async Task<string> GetBNB()
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var user = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            var url = "http://127.0.0.1:8080/api/v1/balance/usdt/" + user.dizhi + "";
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("x-api-key", "your_api_key_1_here");
            var response = await client.GetAsync(url);


            string body = await response.Content.ReadAsStringAsync();

            var peron = JsonConvert.DeserializeObject<Person>(body);


            if (decimal.Parse(peron.balance) < 5000)
            {
                var data = new { msg = "USDT不足", state = "error" };


                string jsonString = JsonConvert.SerializeObject(data);

                return jsonString;

            }
            else
            {
                var data = new { msg = "余额通过", state = "success" };
                string jsonString = JsonConvert.SerializeObject(data);

                return jsonString;

            }
        }


        [HttpPost]

        //互转

        public dynamic MomeyHz([FromForm] string money, [FromForm] int type, [FromForm] string uid)
        {
            lock (locker)
            {
                var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

                var ziji_user = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

                if (ziji_user.hz_state == 1)
                {
                    return new { msg = "转账失败,请联系管理员开通", state = "error" };

                }


                var usermoney = _context.cq_money.Where(t => t.User_id == long.Parse(user_id)).FirstOrDefault();

                var df_user = _context.cq_user.Where(t => t.dizhi == uid).FirstOrDefault();

                if (df_user.Id.ToString() == user_id)
                {
                    return new { msg = "不能向自己转账", state = "error" };

                }


                decimal qb = 0;

                string paytitle = "";

                if (df_user == null)
                {
                    return new { msg = "账号错误", state = "error" };

                }
                else
                {
                    switch (type)
                    {
                        case 1:
                            qb = usermoney.usdt;
                            paytitle = "USDT";
                            break;

                        case 2:
                            qb = usermoney.usdk;
                            paytitle = "USDK";

                            break;

                        case 3:
                            qb = usermoney.xgt;
                            paytitle = "XGT";

                            break;

                    }


                    if (qb < decimal.Parse(money))
                    {
                        return new { msg = "余额不足", state = "error" };

                    }


                    //对方钱包
                    var dfmoney = _context.cq_money.Where(t => t.User_id == df_user.Id).FirstOrDefault();


                    switch (type)
                    {
                        case 1:
                            dfmoney.usdt += decimal.Parse(money);
                            usermoney.usdt -= decimal.Parse(money);


                            break;

                        case 2:
                            dfmoney.usdk += decimal.Parse(money);
                            usermoney.usdk -= decimal.Parse(money);



                            break;

                        case 3:
                            dfmoney.xgt += decimal.Parse(money);
                            usermoney.xgt -= decimal.Parse(money);



                            break;

                    }

                    MxEntity mx1 = new MxEntity();
                    mx1.money = decimal.Parse(money);
                    mx1.title = "转出";
                    mx1.pay_type = paytitle;
                    mx1.df_dizhi = uid;
                    mx1.type = 2;
                    mx1.user_id = long.Parse(user_id);
                    mx1.datetime = DateTime.Now;
                    mx1.Id = IdGeneratorHelper.Instance.GetId();


                    _context.Add(mx1);



                    MxEntity mx2 = new MxEntity();
                    mx2.money = decimal.Parse(money);
                    mx2.title = "转入";
                    mx2.df_dizhi = ziji_user.dizhi;
                    mx2.pay_type = paytitle;
                    mx2.type = 1;
                    mx2.user_id = df_user.Id;
                    mx2.datetime = DateTime.Now;
                    mx2.Id = IdGeneratorHelper.Instance.GetId();


                    _context.Add(mx2);

                    _context.SaveChanges();


                    return new { msg = "转账成功", state = "success" };
                }
            





            }
        }



        //获取直推 列表
        [HttpGet]
        public dynamic GetZTLIST([FromQuery] int yezhi, [FromQuery] int yema)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var list = from a in _context.cq_user

                       where a.zt_id == long.Parse(user_id)

                       select new
                       {
                           a.dizhi
                       };

            return new { msg = "获取成功", list = list.Skip((yema - 1) * yezhi).Take(yezhi).ToList(), state = "success" };


        }


        //申请提现
        [HttpPost]
        public dynamic Addtx(TxEntity tx)
        {
            lock (locker)
            {
                var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();
                var usermoney = _context.cq_money.Where(t => t.User_id == long.Parse(user_id)).FirstOrDefault();
                var xgt = _context.cq_xgt.OrderByDescending(t => t.datetime).FirstOrDefault();
                double sxf = (double)tx.money * 0.1;

                double xgtsxf = sxf / (double)xgt.money;


                var userlist = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

                if (userlist.tx_state == 1)
                {
                    return new { msg = "提现失败,请联系管理员开通", state = "error" };
                }


                if (tx.money < 10)
                {
                    return new { msg = "最低10起提", state = "error" };
                }


                if (usermoney.xgt < (decimal)xgtsxf)
                {
                    return new { msg = "XGT不足", state = "error" };

                }

                if (usermoney.usdt < (decimal)tx.money)
                {
                    return new { msg = "USDT不足", state = "error" };

                }


                usermoney.usdt -= (decimal)tx.money;

                usermoney.xgt -= (decimal)xgtsxf;
                MxEntity mx = new MxEntity();

                mx.money = tx.money;
                mx.datetime = DateTime.Now;
                mx.title = "提现";
                mx.user_id = long.Parse(user_id);
                mx.pay_type = "USDT";
                //1收入，2支出
                mx.type = 2;
                mx.Id = IdGeneratorHelper.Instance.GetId();



                _context.Add(mx);



                tx.Id = IdGeneratorHelper.Instance.GetId();
                tx.datetime = DateTime.Now;
                tx.user_id = long.Parse(user_id);
                tx.state = 0;


                _context.Add(tx);
                _context.SaveChanges();


                return new { msg = "提现成功", state = "success" };
            }
               



        }



        [HttpGet]

        public dynamic Gettxlist([FromQuery] int yezhi, [FromQuery] int yema)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var txlist = _context.cq_tx.Where(t => t.user_id == long.Parse(user_id)).ToList();

            return new { msg = "获取成功", list = txlist.Skip((yema - 1) * yezhi).Take(yezhi).ToList(), state = "success" };



        }

        [HttpGet]
        public dynamic mx([FromQuery] string pay_type, int type, [FromQuery] int yezhi, [FromQuery] int yema)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var mx = _context.cq_mx.Where(t => t.pay_type == pay_type && t.type == type && t.user_id == long.Parse(user_id)).OrderByDescending(t => t.datetime).ToList();

            return new { msg = "获取成功", list = mx.Skip((yema - 1) * yezhi).Take(yezhi).ToList(), state = "success" };


        }


        [HttpPost]

        public dynamic Updatezjc(cqUserEntity user)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var userlist = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            userlist.zjc = user.zjc;

            _context.SaveChanges();


            return new { msg = "修改助记词成功", state = "success" };




        }


        [HttpPost]

        public dynamic updatezfpass(cqUserEntity user)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var userlist = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            userlist.zf_pass = GetMd5Hash(user.zf_pass).ToString();

            _context.SaveChanges();


            return new { msg = "修改支付密码成功", state = "success" };




        }

        static string GetMd5Hash(string input)
        {
            // 创建一个MD5对象
            using (MD5 md5 = MD5.Create())
            {
                // 将字符串转换为字节数组并计算哈希值
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                // 将字节数组转换为十六进制字符串
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2")); // "x2" 表示每个字节用两位十六进制数表示
                }
                return sBuilder.ToString();
            }
        }


        [HttpGet]

        public dynamic Getzjc()
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var userlist = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();

            string[] xixi = userlist.zjc.Split(' ');

            xixi = xixi.OrderBy(x => Guid.NewGuid()).ToArray();

            List<string> zf = new List<string>();

            foreach (var x in xixi)
            {
                zf.Add(x);
            }
            return new { msg = "获取成功", state = "success", list = zf };


        }




        [HttpGet]

        public dynamic yzzjc([FromQuery] string zjc)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var userlist = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();


            if (zjc != userlist.zjc)
            {
                return new { msg = "验证失败", state = "error" };
            }
            else
            {
                return new { msg = "验证通过", state = "success" };

            }



        }



        [HttpGet]

        public dynamic yzzfpass([FromQuery] string zf_pass)
        {
            var user_id = User.Claims.Where(t => t.Type == "UId").Select(t => t.Value).FirstOrDefault();

            var userlist = _context.cq_user.Where(t => t.Id == long.Parse(user_id)).FirstOrDefault();


            if (GetMd5Hash(zf_pass).ToString() != userlist.zf_pass)
            {
                return new { msg = "验证失败", state = "error" };
            }
            else
            {
                return new { msg = "验证通过", state = "success" };

            }



        }


        [HttpPost]
        public dynamic Addbmd(BmdEntity bmd)
        {

            var user = _context.cq_user.Where(t => t.dizhi == bmd.dizhi).FirstOrDefault();

            if (user == null)
            {
                return new { state = "error", msg = "地址错误" };
            }

            bmd.datetime = DateTime.Now;
            bmd.User_id = user.Id;
            bmd.Id = IdGeneratorHelper.Instance.GetId();

            _context.Add(bmd);
            _context.SaveChanges();

            return new { msg = "添加成功", state = "success" };

        }

     

        class Person 
        {
            public string address { get; set; }
            public string balance { get; set; }
            public string balance_wei { get; set; }
            public string success { get; set; }
        }

        #endregion
    }
}