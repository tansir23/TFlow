
using GameFramework.Sunny;

namespace StarForce
{
    /// <summary>
    /// 场景配置表。
    /// </summary>
    public class DRScene : IDataTableRow
    {
        private int m_Id = 0;

        /// <summary>
        /// 场景编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 背景音乐编号。
        /// </summary>
        public int BackgroundMusicId
        {
            get;
            private set;
        }

        /*public override void ParseDataRow(string dataRowSegment)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowSegment);
            int index = 0;
            index++;
            m_Id = int.Parse(text[index++]);
            index++;
            AssetName = text[index++];
            BackgroundMusicId = int.Parse(text[index++]);
        }*/

        public void ParseRowData(string dataRowSegment)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowSegment);
            int index = 0;
            index++;
            m_Id = int.Parse(text[index++]);
            index++;
            AssetName = text[index++];
            BackgroundMusicId = int.Parse(text[index++]);

        }
    }
}
