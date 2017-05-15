using PHRMSAdmin.DALayer;
using PHRMSAdmin.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHRMSAdmin.Controllers
{
    public class CommonController : Controller
    {
        public static List<Tasks> GetUserMenuList(long UserId, bool IsSuperAdmin)
        {

            // UserName oUser = GetUserId(GetUserName(UserName), Type);

            PHRMSDBContext oDB = new PHRMSDBContext();

            List<Tasks> oMenuList = new List<Tasks>();
            try
            {
                if (!IsSuperAdmin)
                {


                    oMenuList = (from r in oDB.Role
                                 join
                                 ur in oDB.UserRoleMapping on r.RoleId equals ur.RoleId
                                 join
                                 rt in oDB.RoleTaskMapping on ur.RoleId equals rt.RoleId
                                 join
                                    t in oDB.Tasks on rt.TaskId equals t.TaskId
                                 // from TaskList in oDB.Task.
                                 where t.IsActive == true && ur.AdminUserId == UserId

                                 select t).OrderBy(m => m.HeadingPosition).ThenBy(m => m.HeadingName).ThenBy(m => m.TaskPosition).ToList();



                    oMenuList = oMenuList.Distinct().ToList();
                }
                else
                {
                    return oDB.Tasks.Where(s => s.IsActive == true && s.TaskId!=8 &&s.TaskId!=10).OrderBy(m => m.HeadingPosition).ThenBy(m => m.HeadingName).ThenBy(m => m.TaskPosition).ToList();
                }
            }
            catch (Exception ex)
            {

                //    Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Common");
            }
            oDB.Dispose();
            oDB = null;
            return oMenuList;
        }
    }
}