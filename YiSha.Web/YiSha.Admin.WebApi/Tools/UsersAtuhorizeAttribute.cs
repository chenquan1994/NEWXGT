using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
 
using YiSha.Business.OrganizationManage;
using YiSha.Model;
 
 
using YiSha.Service.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.Admin.WebApi.Tools
{
    public class UsersAtuhorizeAttribute : IAuthorizationFilter
    {
        private cqUserBLL userBLL = new cqUserBLL();
        private UserService User_serverice = new UserService();

        private readonly MiaContext _context;

        public UsersAtuhorizeAttribute(MiaContext context)
        {
            _context = context;
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            StringValues authorizationHeader = context.HttpContext.Request.Headers["Authorization"]; //获取请求头header携带的token值
            if (!StringValues.IsNullOrEmpty(authorizationHeader))
            {
                string[] parts = authorizationHeader[0].Split(' ');

                if (parts.Length == 2 && parts[0] == "Bearer")
                {
                    // 提取 JWT token
                    var jwtTokenHandler = new JwtSecurityTokenHandler();

                    try
                    {
                        var jwtToken = jwtTokenHandler.ReadJwtToken(parts[1]);

                        // 获取用户 ID 或其他需要的信息
                        var userId = jwtToken.Claims.First(x => x.Type == "UId").Value;

                        var user = _context.cq_user.Where(t => t.Id == Int64.Parse(userId)).FirstOrDefault();

                        if (user == null)
                        {
                            context.Result = new JsonResult(
                   new TData
                   {
                       Tag = 0,
                       Message = "非法登录",
                       Description = "非法登录"
                   });
                        }

                      

                        // 将用户信息存入上下文对象，供后续操作使用

                    }
                    catch (System.Exception ex)
                    {
                        context.Result = new JsonResult(
                  new TData
                  {
                      Tag = 0,
                      Message = "非法登录",
                      Description = "非法登录"
                  });
                    }
                }
            }



        }
    }
}