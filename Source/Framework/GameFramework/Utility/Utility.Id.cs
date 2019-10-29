
using System;
using System.Text;

namespace GameFramework.Taurus
{
    public static partial class Utility
    {
        /// <summary>
        /// 字符相关的实用函数。
        /// </summary>
        public static class Id
        {
            private static ushort value;
            /// <summary>
            /// 计算Id
            /// </summary>
            /// <returns>Id</returns>
            public static long GenerateId()
            {
                string timeStr = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                long time = long.Parse(timeStr);
                return (time << 16) + ++value;
            }
        }
    }
}
