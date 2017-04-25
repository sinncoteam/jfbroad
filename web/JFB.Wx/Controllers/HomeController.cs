using JFB.Business.Domain.Service;
using JFB.Utils;
using JFB.Wx.Component;
using JFB.Wx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JFB.Wx.Controllers
{
    //[AuthLogin]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game()
        {
            GameDto dto = new GameDto();
            QuestionService x_qService = new QuestionService();
            dto.qList = x_qService.GetRandomList(1);
            return View(dto);
        }

        public JsonResult GameRed()
        {
            AjaxMsgResult result = new AjaxMsgResult();

            RedPackListService x_rplService = new RedPackListService();
            var info = x_rplService.CheckInRedPack(0);
            if( info.PackId > 0)
            {
                result.Success = true;
                result.Source = info;
            }
            else
            {
                result.Success = false;
                result.Msg = info.RbName;
            }

            return Json(result);
        }
    }
}
