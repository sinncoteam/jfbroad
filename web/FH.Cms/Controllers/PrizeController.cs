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

    }
}
