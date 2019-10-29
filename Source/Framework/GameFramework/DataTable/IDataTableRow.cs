//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #数据表行解析# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

namespace GameFramework.Taurus
{
    public interface IDataTableRow
    {
        /// <summary>
        /// Id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 解析当前数据的接口
        /// </summary>
        /// <param name="data"></param>
        void ParseRowData(string data);
    }
}
