using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JFB.Utils;
using ViData;
using JFB.Systems.Domain.Service;
using JFB.Systems.Domain.Info;
using JFB.Business.Domain.Service;
using JFB.Business.Domain.Model;
using JFB.Cms.Models;

namespace JFB.Cms.Controllers
{
    public class PrizeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult IndexList(JTableData adata)
        {
            PagingInfo pi = new PagingInfo()
            {
                BeginIndex = adata.iDisplayStart + 1,
                EndIndex = adata.iDisplayStart + adata.iDisplayLength,
                TableName = "t_d_redpack op",
                Fileds = "*",
                SortFields = "id desc"
            };
            //if (!string.IsNullOrEmpty(adata.sSearch))
            //{
            //    pi.Conditions = "  op.username like '%'+@key+'%' ";
            //    pi.Parameters.Add("key", adata.sSearch);
            //}
            RedPackService x_rpService = new RedPackService();
            var list = x_rpService.GetPaging(pi);

            JTableResult<RedPack> ar = new JTableResult<RedPack>()
            {
                sEcho = adata.sEcho,
                iTotalRecords = pi.RecordCount,
                iTotalDisplayRecords = pi.RecordCount,
                aaData = list
            };
            return Json(ar, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetStatus(string ids ,int type)
        {
            AjaxMsgResult result = new AjaxMsgResult();
            RedPackService x_rpService = new RedPackService();
            int i = x_rpService.ResetStatus(ids, type);
            if( i > 0)
            {
                result.Success = true;
                result.Msg = "重置状态成功";
            }
            else
            {
                result.Success = false;
                result.Msg = "重置状态处理失败";
            }
            return Json(result);
        }

        public ActionResult Edit(int? id)
        {
            RedPackEditDto dto = new RedPackEditDto();
            if (id > 0)
            {
                RedPackService x_rpService = new RedPackService();
                dto.RPInfo = x_rpService.GetById(id);

            }
            if (dto.RPInfo == null)
            {
                dto.RPInfo = new RedPack();
            }
            return View(dto);
        }

        public JsonResult EditSave(RedPack rp)
        {
            AjaxMsgResult result = new AjaxMsgResult();
            RedPackService x_opService = new RedPackService();
            if (rp.ID > 0)  //更新
            {
                int i = x_opService.Update(() => new RedPack() {
                     RbCount = rp.RbCount,
                      RbMoney = rp.RbMoney,
                       RbTotal = rp.RbTotal,
                        UpdateTime = DateTime.Now,
                         GetPercent = rp.GetPercent
                }, a => a.ID== rp.ID);
                if (i > 0)
                {
                    result.Success = true;
                }
                else
                {
                    result.Msg = "更新失败，没有找到该用户！";
                }
            }
            else
            {
                rp.IsValid = 1;
                rp.CreateTime = DateTime.Now;
                rp.UpdateTime = DateTime.Now;
                x_opService.Insert(rp);
                result.Success = true;
            }
            return Json(result);
        }
    }
}
