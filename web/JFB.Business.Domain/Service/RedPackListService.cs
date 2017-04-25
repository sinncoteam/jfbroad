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
        static object locker = new object();
        /// <summary>
        /// 用户抽奖红包逻辑
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RedPackListInfo CheckInRedPack(int userId)
        {
            RedPackListInfo info = new RedPackListInfo();
            List<RedPackCheck> clist = new List<RedPackCheck>();
            RedPackService x_rpService = new RedPackService();
            var list = x_rpService.Get(a => a.IsValid == 1);
            int total = 0;
            foreach(var item in list)
            {
                int nextv = total + item.GetPercent;
                RedPackCheck rc = new RedPackCheck()
                {
                    RedPackId = item.ID,
                     MinV = total,
                      MaxV = nextv - 1
                };
                clist.Add(rc);
                total = nextv;
            }
            Random rnd = new Random(DateTime.Now.GetHashCode());
            int v = rnd.Next(1, total);
            var checkitem = clist.Where(a => v >= a.MinV && v <= a.MaxV).FirstOrDefault();
            if( checkitem != null)
            {
                lock (locker)
                {
                    var sitem = this.Get(a => a.UserId == userId).FirstOrDefault();
                    if (sitem == null)
                    {
                        var nowitem = x_rpService.Get(a => a.ID == checkitem.RedPackId && a.IsValid == 1).FirstOrDefault();
                        if (nowitem != null)
                        {
                            if (nowitem.RbCount > 0)
                            {
                                x_rpService.Update(() => new RedPack() { RbCount = nowitem.RbCount - 1 }, a => a.ID == nowitem.ID);
                                RedPackListInfo rpinfo = new RedPackListInfo()
                                {
                                    PackId = nowitem.ID,
                                    GetTime = DateTime.Now,
                                    PackMoney = nowitem.RbMoney,
                                    UserId = userId,
                                    PackStatus = 0
                                };
                                this.Insert(rpinfo);
                                info.PackId = nowitem.ID;
                                info.RbName = nowitem.RbName;
                                info.PackMoney = nowitem.RbMoney;
                                return info;
                            }
                            else
                            {
                                info.PackId = 0;
                                info.RbName = "红包已发完，请留意后续活动";
                            }
                        }
                    }
                    else
                    {
                        info.PackId = -1;
                        info.RbName = "您已领取过红包";
                    }
                }
            }
            else
            {
                info.PackId = 0;
                info.RbName = "很遗憾，本次未能抽中红包";
            }           
            
            return info;
        }
    }
}
