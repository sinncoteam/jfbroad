using JFB.Business.Domain.Info;
using JFB.Business.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JFB.Wx.Models
{
    public class GameDto
    {
        public IList<QuestionInfo> qList { get; set; }
    }
}