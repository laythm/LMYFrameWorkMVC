using LMYFrameWorkMVC.Common.Entities;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.Helpers
{
    public static class CacheHelper
    {
        //use enterprise application block
        static CacheHelper()
        {
            _Cache = CacheFactory.GetCacheManager("LMYFrameWorkCacheManager");
        }
  
        private static void setDependencies(CacheMemberKey cacheMemberKey, List<CacheMemberKey> dependancyListKeys = null)
        {
            if (dependancyListKeys != null)
                foreach (CacheMemberKey dependencyCacheMemberKey in dependancyListKeys)
                {
                    if (_Cache[dependencyCacheMemberKey.GetDependencyFullKey()] == null)
                    {
                        //cache the cacheMemberKey as value to dependencyCacheMemberKey to key
                        _Cache.Add(dependencyCacheMemberKey.GetDependencyFullKey(), new List<CacheMemberKey>() { cacheMemberKey });
                    }
                    else
                    {
                        List<CacheMemberKey> list = _Cache[dependencyCacheMemberKey.GetDependencyFullKey()] as List<CacheMemberKey>;
                        list.Add(cacheMemberKey);
                       // _Cache.Remove(dependencyCacheMemberKey.GetDependencyFullKey());
                        //_Cache.Add(dependencyCacheMemberKey.GetDependencyFullKey(), list);
                    }
                }
        }

        private static ICacheManager _Cache;

        public static object Insert(CacheMemberKey cacheMemberKey, object value, List<CacheMemberKey> dependancyListKeys = null)
        {
            RemoveCache(cacheMemberKey);
            _Cache.Add(cacheMemberKey.GetFullKey(), value);
            setDependencies(cacheMemberKey, dependancyListKeys);

            return value;
            //   _Cache.Insert(key, value, new CacheDependency(null, dependancyListKeys));
        }

        public static object GetValue(CacheMemberKey key)
        {
            try
            {
                return _Cache[key.GetFullKey()];
            }
            catch { };
            return null;
        }

        public static bool RemoveCache(CacheMemberKey key)
        {
            try
            {
                if (_Cache[key.GetDependencyFullKey()] != null)
                    foreach (CacheMemberKey cacheMemberKey in _Cache[key.GetDependencyFullKey()] as List<CacheMemberKey>)
                    {
                        _Cache.Remove(cacheMemberKey.GetFullKey());
                        _Cache.Remove(key.GetDependencyFullKey());
                    }

                _Cache.Remove(key.GetFullKey());
            }
            catch { return false; };
            return true;
        }

    }
}
