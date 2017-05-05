using PHRMSAdmin.DALayer;
using PHRMSAdmin.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PHRMSAdmin.Models
{
    public class CommonModel
    {
        public static List<Tasks> GetTasks()
        {
            PHRMSDBContext oDB = new PHRMSDBContext();
            List<Tasks> oTasks = null;
            try
            {

                oTasks = oDB.Tasks.Where(s => s.IsActive == true).ToList();
            }
            catch (Exception ex)
            {

                //Common.CreateLog(Common.ExecptionMessage(ex), "Error", "Common");
            }
            oDB.Dispose();
            oDB = null;
            return oTasks;

        }
    }
}