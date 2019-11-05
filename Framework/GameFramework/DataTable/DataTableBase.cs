//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #配置文件信息的基类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;

namespace GameFramework.Sunny
{
	public abstract class DataTableBase
	{
        private readonly string m_Name;

        /// <summary>
        /// 初始化数据表基类的新实例。
        /// </summary>
        public DataTableBase()
            : this(null)
        {

        }

        /// <summary>
        /// 初始化数据表基类的新实例。
        /// </summary>
        /// <param name="name">数据表名称。</param>
        public DataTableBase(string name)
        {
            m_Name = name ?? string.Empty;
        }

        /// <summary>
        /// 获取数据表名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 获取数据表行的类型。
        /// </summary>
        public abstract Type Type
        {
            get;
        }

        /// <summary>
        /// 获取数据表行数。
        /// </summary>
        public abstract int Count
        {
            get;
        }

        /// <summary>
        /// 增加数据表行。
        /// </summary>
        public abstract void AddDataRow(string dataRowText);
	}
}
