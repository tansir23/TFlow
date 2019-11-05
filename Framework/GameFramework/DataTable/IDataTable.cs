//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #数据表接口# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

namespace GameFramework.Sunny
{

    public interface IDataTable<T> where T : class, IDataTableRow,new()
    {
		/// <summary>
		/// 总数
		/// </summary>
	    int Count { get; }
		/// <summary>
		/// 获取数据行
		/// </summary>
		/// <param name="id">数据id</param>
		/// <returns></returns>
		T this[int id] { get; }
		/// <summary>
		/// 是否存在Id的数据行
		/// </summary>
		/// <param name="id">数据id</param>
		/// <returns></returns>
		bool HasDataRow(int id);
	    /// <summary>
	    /// 获取Id的数据行
	    /// </summary>
	    /// <param name="id"></param>
	    /// <returns></returns>
	    T GetDataRow(int id);
	    /// <summary>
	    /// 获取所有的数据行
	    /// </summary>
	    /// <returns></returns>
	    T[] GetAllDataRows();
	}
}
