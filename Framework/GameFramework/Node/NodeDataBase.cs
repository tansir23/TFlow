//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2018 Zhang Yang. All rights reserved.
// </copyright>$
// <describe> #数据节点的基类# </describe>
// <email> yeozhang@qq.com </email>
// <time> #2018年8月26日 17点34# </time>
//-----------------------------------------------------------------------

namespace GameFramework.Sunny
{
	public abstract class NodeDataBase
	{
        /// <summary>
        /// 完整名称
        /// </summary>
        public string FullName;

        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName;

        /// <summary>
        /// 值
        /// </summary>
        public string ValueName;

        /// <summary>
        /// 键
        /// </summary>
        public string KeyName;


        /// <summary>
        /// 清除
        /// </summary>
		public abstract void Clear();
	}
}
