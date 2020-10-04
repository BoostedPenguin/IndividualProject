using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    /// <summary>
    /// Example Service
    /// </summary>
    public class RandomGenerator
    {
        private Random random;
        public RandomGenerator()
        {
            random = new Random();
        }

        public object GetRandomItem(object[] arr)
        {
            return arr[random.Next(0, arr.Length - 1)];
        }
    }
}
