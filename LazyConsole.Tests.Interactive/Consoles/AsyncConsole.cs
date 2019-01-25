using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyConsole.Tests.Interactive.Consoles
{
    public static class AsyncConsole
    {
        /// <summary>
        /// Testing async signature
        /// </summary>
        /// <returns></returns>
        public static async Task ReturnAsync()
        {
            var values = new List<int>();
            var selectedValue = values.Select(async v => v).First();
            return;

        }
        /// <summary>
        /// Testing async generic signature
        /// </summary>
        /// <returns></returns>
        public static async Task<int> ReturnIntAsync()
        {
            var values = new List<int>();
            var selectedValue = values.Select(async v => v).First();
            return await selectedValue;
        }
        /// <summary>
        /// Testing exception
        /// </summary>
        /// <returns></returns>
        public static async Task<int> ReturnIntAsyncWithException()
        {
            var values = new List<int>();
            var selectedValue = await values.Select(async v => v).First();
            throw new System.Exception("Exception!");
        }
    }
}
