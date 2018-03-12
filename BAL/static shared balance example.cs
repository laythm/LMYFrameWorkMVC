using LMYFrameWorkMVC.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.BAL
{
    internal static class Class1
    {
        private static LMYFrameWorkMVCEntities dbContext;

        static Class1()
        {
            dbContext = new LMYFrameWorkMVCEntities();
        }

        public static void AddBalance(int balance, string userid = null)
        {
            var a = dbContext.AspNetUsers.Find("0566715c-5b9b-486f-a05d-762c2c075427");
            a.AccessFailedCount = a.AccessFailedCount + balance;
            System.Threading.Thread.Sleep(9000);
            dbContext.SaveChanges(userid,true);
        }
    }
}
