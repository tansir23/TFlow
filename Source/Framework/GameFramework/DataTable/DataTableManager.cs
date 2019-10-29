//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #配置文件管理类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    public sealed class DataTableManager : GameFrameworkModule
    {

		private readonly Dictionary<int, DataTableBase> _allDataTables;
		private ResourceManager _resource;

        /// <summary>
        /// 获取数据表数量。
        /// </summary>
        public int Count
        {
            get
            {
                return _allDataTables.Count;
            }
        }


        public DataTableManager()
	    {
            _allDataTables = new Dictionary<int, DataTableBase>();
		    _resource = GameFrameworkMode.GetModule<ResourceManager>();
	    }
	    /// <summary>
	    /// 加载数据表
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="data">配置表的数据</param>
	    /// <returns></returns>
		public void LoadDataTable<T>(string assetBundleName,string dataTablePath) where T :class, IDataTableRow,new()
	    {
            TextAsset textAsset = _resource.LoadAssetSync<TextAsset>(assetBundleName, dataTablePath);

            string data = textAsset.text;
		    DataTable<T> dataTable = new DataTable<T>("");
		    string[] rows = data.Split('\n');
		    foreach (var item in rows)
		    {
				//排除多余的数据
			    if (string.IsNullOrEmpty(item) || item.Length == 0 || item.Contains("#"))
				    continue;
                //Debug.Log(item);
			    dataTable.AddDataRow(item);
		    }
		    int hasCode = typeof(T).GetHashCode();
            _allDataTables[hasCode] = dataTable;
		}

	    /// <summary>
	    /// 获取数据表
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <returns></returns>
	    public IDataTable<T> GetDataTable<T>() where T : class, IDataTableRow, new()
	    {
		    DataTableBase dataTable = null;
		    int hashCode = typeof(T).GetHashCode();
		    if (_allDataTables.TryGetValue(hashCode, out dataTable))
			    return (IDataTable<T>)dataTable;
		    return null;
	    }

        /// <summary>
        /// 获取所有数据表。
        /// </summary>
        /// <returns>所有数据表。</returns>
        public DataTableBase[] GetAllDataTables()
        {
            int index = 0;
            DataTableBase[] results = new DataTableBase[_allDataTables.Count];
            foreach (KeyValuePair<int, DataTableBase> dataTable in _allDataTables)
            {
                results[index++] = dataTable.Value;
            }

            return results;
        }

        public override void OnClose()
        {
            _allDataTables.Clear();

        }
    }
}
