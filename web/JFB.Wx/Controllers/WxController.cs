﻿using JFB.Business.Domain.Info;
using JFB.Business.Domain.Service;
using JFB.Wx.Component;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JFB.Wx.Controllers
{
    public class WxController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            string code = Request.QueryString["code"];
            string state = Request.QueryString["state"];
            string s = Request.QueryString["s"];
            if (!IsLogin)
            {
                OAuthAccessTokenResult result = OAuthApi.GetAccessToken(tenPayV3Info.AppId, tenPayV3Info.AppSecret, code);
                if (result.errcode == Senparc.Weixin.ReturnCode.请求成功)
                {
                    string openid = result.openid;
                    UserService x_userService = new UserService();
                    var item = x_userService.Get(a => a.OpenId == openid).FirstOrDefault();
                    if (item != null)
                    {
                        if (item.IsValid == 1)
                        {
                            Authentication.Instance.SetAuth(item, true);
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                    else  //获取用户的信息
                    {
                        OAuthUserInfo wxUser = OAuthApi.GetUserInfo(result.access_token, code);
                        if (wxUser != null)
                        {
                            UserInfo uInfo = new UserInfo()
                            {
                                IsValid = 1,
                                CreateTime = DateTime.Now,
                                //Lastlogintime = DateTime.Now,
                                NickName = wxUser.nickname,
                                OpenId = wxUser.openid,
                                HeadImage = wxUser.headimgurl
                            };
                            uInfo.ID = Convert.ToInt32(x_userService.Insert(uInfo));
                            Authentication.Instance.SetAuth(uInfo, true);
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                }
            }
            string url = "/";
            if (!string.IsNullOrEmpty(s))
            {
                url = s;
            }
            return Redirect(url);
        }
    }
}
