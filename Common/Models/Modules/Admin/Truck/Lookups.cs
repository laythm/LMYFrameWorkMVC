using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck
{
    public class Lookups
    {
        public static class TruckLoadStatus
        {
            public static string Canceled { get { return "06A68E20-C651-4E72-A956-C2EC45714A73"; } }
            public static string Completed { get { return "3D602C3E-1270-4ADA-8A77-F53DFC4905EE"; } }
            public static string UpComing { get { return "8264BA08-08A9-4525-939A-E8D315963188"; } }
            public static string InProcess { get { return "CCD71CF9-170B-4722-AB53-326AC5A86014"; } }

        }
    }
}
