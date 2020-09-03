using System;
using System.Runtime.Caching;

namespace TestMemoryCache.Test
{
    public class MemoryCacheTest
    {
        private MemoryCache cache;
        private readonly CacheItemPolicy cacheItemPolicy;

        public MemoryCacheTest()
        {
            // initialize the MemoryCache and CacheItemPolicy at constructor level
            cache = new MemoryCache("Test");
            cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(600.0)
            };            
        }

        /// <summary>
        /// This method is used to check the incoming input and act accordingly
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string CheckInput(string input)
        {
            string result = string.Empty; 
            
            // check for empty or null input
            if (string.IsNullOrEmpty(input))
            {
                return "Please use a valid input";
            }


            if (input.Length > 0) // if input has some value
            {
                if (input.Contains("|")) // check if the input contain pipe (|)
                {
                    var arrInput = input.Split('|');
                    if (arrInput[0].ToString() == "Add")
                    {
                        result = AddIntoCache(arrInput[1].ToString());
                    }
                    else if (arrInput[0].ToString() == "Get")
                    {
                        result = GetFromCache(arrInput[1].ToString());
                    }
                    else if (arrInput[0].ToString() == "Has")
                    {
                        result = CheckCache(arrInput[1].ToString());
                    }
                    else if (arrInput[0].ToString() == "Remove")
                    {
                        result = RemoveFromCache(arrInput[1].ToString());
                    }
                }
                else // simple input without pipe (|)
                {
                    result = ResetCache();
                }
                
            }
            return result;
        }

        private string RemoveFromCache(string param)
        {
            return cache.Remove(param).ToString().Length > 0 ? "True" : "False";
        }

        private string ResetCache()
        {
            var countItems = cache.GetCount().ToString();
            cache.Dispose(); // Reset the Cache object
            return countItems;
        }

        private string AddIntoCache(string param)
        {
            var cacheItem = new CacheItem(param, param);

            return Convert.ToString(cache.Add(cacheItem, cacheItemPolicy));
        }

        private string GetFromCache(string param)
        {
            if (cache.Contains(param))
            {
                return cache.Get(param).ToString();
            }
            else
            {
                return "NULL";
            }
            
        }

        private string CheckCache(string param)
        {
            return cache.Contains(param).ToString();
        }
    }
}
