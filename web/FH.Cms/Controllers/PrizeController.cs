using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JFB.Utils;
using ViData;
using JFB.Systems.Domain.Service;
using JFB.Systems.Domain.Info;

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
                TableName = "t_d_operator op",
                Fileds = "*",
                SortFields = "id desc"
            };
            if (!string.IsNullOrEmpty(adata.sSearch))
            {
                pi.Conditions = "  op.username like '%'+@key+'%' ";
                pi.Parameters.Add("key", adata.sSearch);
            }
            OperatorService x_opService = new OperatorService();
            var list = x_opService.GetPaging(pi);
            JTableResult<OperatorInfo> ar = new JTableResult<OperatorInfo>()
            {
                sEcho = adata.sEcho,
                iTotalRecords = pi.RecordCount,
                iTotalDisplayRecords = pi.RecordCount,
                aaData = list
            };
            return Json(ar);
        }

    }
}
