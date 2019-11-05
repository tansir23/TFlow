//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>$
// <describe> #数据节点# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFramework.Sunny
{
    /// <summary>
    /// 数据结点
    /// </summary>
    public class DataNode
    {

        public static readonly DataNode[] s_EmptyArray = new DataNode[] { };
        public static readonly string[] s_PathSplit = new string[] { ".", "/", "\\" };

        /// <summary>
        /// 结点名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 结点全名
        /// </summary>
        public string FullName
        {
            get
            {
                return Parent == null ? Name : string.Format("{0}{1}{2}", Parent.FullName, s_PathSplit[0], Name);
            }
        }

        /// <summary>
        /// 结点数据
        /// </summary>
        private object m_Data;


        /// <summary>
        /// 父结点
        /// </summary>
        public DataNode Parent { get; private set; }

        /// <summary>
        /// 子结点
        /// </summary>
        private List<DataNode> m_Childs;

        /// <summary>
        /// 子结点数量
        /// </summary>
        public int ChildCount
        {
            get
            {
                return m_Childs != null ? m_Childs.Count : 0;
            }
        }

        public DataNode(string name, DataNode parent)
        {
            if (!IsValidName(name))
            {
                throw new GamekException("DataNode name is invalid!");
            }

            Name = name;
            m_Data = null;
            Parent = parent;
            m_Childs = null;
        }


        /// <summary>
        /// 检测数据结点名称是否合法。
        /// </summary>
        /// <param name="name">要检测的数据节点名称。</param>
        /// <returns>是否是合法的数据结点名称。</returns>
        private static bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }


            foreach (string pathSplit in s_PathSplit)
            {
                if (name.Contains(pathSplit))
                {
                    return false;
                }
            }


            return true;
        }

        #region Set
        /// <summary>
        /// 获取结点数据
        /// </summary>
        public T GetData<T>()
        {
            return (T)m_Data;
        }

        /// <summary>
        /// 根据索引获取子数据结点
        /// </summary>
        /// <param name="index">子数据结点的索引</param>
        /// <returns>指定索引的子数据结点，如果索引越界，则返回空</returns>
        public DataNode GetChild(int index)
        {
            return index >= ChildCount ? null : m_Childs[index];
        }

        /// <summary>
        /// 根据名称获取子数据结点
        /// </summary>
        /// <param name="name">子数据结点名称</param>
        /// <returns>指定名称的子数据结点，如果没有找到，则返回空</returns>
        public DataNode GetChild(string name)
        {
            if (!IsValidName(name))
            {
                //Debug.LogError("子结点名称不合法，无法获取");
                return null;
            }

            if (m_Childs == null)
            {
                return null;
            }

            foreach (DataNode child in m_Childs)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据名称获取或增加子数据结点
        /// </summary>
        /// <param name="name">子数据结点名称</param>
        /// <returns>指定名称的子数据结点，如果对应名称的子数据结点已存在，则返回已存在的子数据结点，否则增加子数据结点</returns>
        public DataNode GetOrAddChild(string name)
        {
            DataNode node = GetChild(name);
            if (node != null)
            {
                return node;
            }

            node = new DataNode(name, this);

            if (m_Childs == null)
            {
                m_Childs = new List<DataNode>();
            }

            m_Childs.Add(node);

            return node;
        }

        /// <summary>
        /// 获取所有子数据结点。
        /// </summary>
        /// <returns>所有子数据结点。</returns>
        public DataNode[] GetAllChild()
        {
            if (m_Childs == null)
            {
                return new DataNode[] { };
            }

            return m_Childs.ToArray();
        }

        #endregion


        #region Set
        /// <summary>
        /// 设置结点数据
        /// </summary>
        public void SetData(object data)
        {
            m_Data = data;
        }
        #endregion


        #region Remove
        /// <summary>
        /// 根据索引移除子数据结点
        /// </summary>
        /// <param name="index">子数据结点的索引位置</param>
        public void RemoveChild(int index)
        {
            DataNode node = GetChild(index);
            if (node == null)
            {
                return;
            }

            node.Clear();
            m_Childs.Remove(node);
        }

        /// <summary>
        /// 根据名称移除子数据结点
        /// </summary>
        /// <param name="name">子数据结点名称</param>
        public void RemoveChild(string name)
        {
            DataNode node = GetChild(name);
            if (node == null)
            {
                return;
            }

            node.Clear();
            m_Childs.Remove(node);
        }
        #endregion

        /// <summary>
        /// 移除当前数据结点的数据和所有子数据结点
        /// </summary>
        public void Clear()
        {
            m_Data = null;
            if (m_Childs != null)
            {
                foreach (DataNode child in m_Childs)
                {
                    child.Clear();
                }

                m_Childs.Clear();
            }
        }

        /// <summary>
        /// 获取数据字符串。
        /// </summary>
        /// <returns>数据字符串。</returns>
        public string ToDataString()
        {
            if (m_Data == null)
            {
                return "<Null>";
            }

            return string.Format("[{0}] {1}", m_Data.GetType().Name, m_Data.ToString());
        }
    }
}
