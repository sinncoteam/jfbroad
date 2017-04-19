using JFB.Business.Domain.Info;
using JFB.Business.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViData;

namespace JFB.Business.Domain.Service
{
    public class RedPackListService : Repository<RedPackListInfo, RedPackList>
    {
        public IList<RedPackListInfo> getAllUser()
        {
            string sql = "select u.nickname, u.openid, rp.rbname, rpl.* from t_d_redpack_list rpl inner join t_d_redpack rp on rp.ID = rpl.pack_id inner join t_d_user u on u.ID = rpl.user_id where rpl.packstatus = 0";
            return DataHelper.Fill<RedPackListInfo>(sql);
        }
    }
}
