//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #数据表扩展类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;

namespace GameFramework.Sunny
{
    public static class DataTableExtension
    {

        public static string[] SplitDataRow(string dataRowSegment)
        {
            return dataRowSegment.Substring(0, dataRowSegment.Length).Split(ColumnSplitSeparator, StringSplitOptions.None);
        }

        private const string DataRowClassPrefixName = "StarForce.DR";
        private static readonly string[] ColumnSplitSeparator = new string[] { "\t" };
        /*
        public static void LoadDataTable(this DataTableComponent dataTableComponent, string dataTableName, LoadType loadType, object userData = null)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string[] splitNames = dataTableName.Split('_');
            if (splitNames.Length > 2)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string dataRowClassName = DataRowClassPrefixName + splitNames[0];

            Type dataRowType = Type.GetType(dataRowClassName);
            if (dataRowType == null)
            {
                Log.Warning("Can not get data row type with class name '{0}'.", dataRowClassName);
                return;
            }

            string dataTableNameInType = splitNames.Length > 1 ? splitNames[1] : null;
            dataTableComponent.LoadDataTable(dataRowType, dataTableName, dataTableNameInType, AssetUtility.GetDataTableAsset(dataTableName, loadType), loadType, Constant.AssetPriority.DataTableAsset, userData);
        }
        */
        
    }
}
