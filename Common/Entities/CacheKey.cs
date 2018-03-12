using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Entities
{
    public class CacheMemberKey
    {
        private const string dependancyListKey = "$#$#-__234dependasdwqancyLi^%^$#stKeys__35$#$#";
        public LMYFrameWorkMVC.Common.LookUps.CacheKeys CacheKey { get; set; }
        public string ObjectId { get; set; }

        public string GetFullKey()
        {
            return this.CacheKey.ToString() + "_" + this.ObjectId;
        }

        public string GetDependencyFullKey()
        {
            return dependancyListKey + "_" + this.CacheKey.ToString() + "_" + this.ObjectId;
        }
    }
}
