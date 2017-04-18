using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JFB.Business.Domain.Model
{
    public class User
    {
        public int ID { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string HeadImage { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsValid { get; set; }
    }
}
