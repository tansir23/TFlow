//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #数据节点管理类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework.Taurus
{
    public sealed class NodeManager : GameFrameworkModule
    {
        private static readonly string[] s_EmptyStringArray = new string[] { };

        /// <summary>
        /// 根结点
        /// </summary>
        public DataNode Root { get; private set; }

        /// <summary>
        /// 根结点名称
        /// </summary>
        private const string RootName = "<NodeRoot>";

        public NodeManager()
        {
            Root = new DataNode(RootName, null);
        }

        public override void OnClose()
        {
            Root.Clear();
            Root = null;
        }

        #region Get
        /// <summary>
        /// 数据结点路径切分
        /// </summary>
        /// <param name="path">要切分的数据结点路径</param>
        /// <returns>切分后的字符串数组</returns>
        private static string[] GetSplitPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return s_EmptyStringArray;
            }

            return path.Split(DataNode.s_PathSplit, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 获取数据结点。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径</param>
        /// <param name="node">查找起始结点</param>
        /// <returns>指定位置的数据结点，如果没有找到，则返回空</returns>
        public DataNode Get(string path, DataNode node = null)
        {
            DataNode current = (node ?? Root);

            //获取子结点路径的数组
            string[] splitPath = GetSplitPath(path);

            foreach (string ChildName in splitPath)
            {
                //根据数组里的路径名获取子结点
                current = current.GetChild(ChildName);
                if (current == null)
                {
                    return null;
                }
            }

            return current;
        }


        /// <summary>
        /// 根据类型获取数据结点的数据
        /// </summary>
        /// <typeparam name="T">要获取的数据类型</typeparam>
        /// <param name="path">相对于 node 的查找路径</param>
        /// <param name="node">查找起始结点</param>
        public T Get<T>(string path, DataNode node = null)
        {
            DataNode current = Get(path, node);
            if (current == null)
            {
                throw new GamekException("DataNode is not exsit:" + path);
                //Debug.Log("要获取数据的结点不存在：" + path);
                //return default(T);
            }

            return current.GetData<T>();

        }
        #endregion

        #region Add
        /// <summary>
        /// 获取或增加数据结点
        /// </summary>
        /// <param name="path">相对于 node 的查找路径</param>
        /// <param name="node">查找起始结点</param>
        /// <returns>指定位置的数据结点，如果没有找到，则增加相应的数据结点</returns>
        public DataNode GetOrAdd(string path, DataNode node = null)
        {
            DataNode current = (node ?? Root);
            string[] splitPath = GetSplitPath(path);
            foreach (string childName in splitPath)
            {
                current = current.GetOrAddChild(childName);
            }

            return current;
        }
        #endregion


        #region Remove
        /// <summary>
        /// 移除数据结点
        /// </summary>
        /// <param name="path">相对于 node 的查找路径</param>
        /// <param name="node">查找起始结点</param>
        public void Remove(string path, DataNode node = null)
        {
            DataNode current = (node ?? Root);
            DataNode parent = current.Parent;
            string[] splitPath = GetSplitPath(path);
            foreach (string childName in splitPath)
            {
                parent = current;
                current = current.GetChild(childName);
                if (current == null)
                {
                    return;
                }
            }

            if (parent != null)
            {
                parent.RemoveChild(current.Name);
            }
        }
        #endregion


        #region Set
        /// <summary>
        /// 设置数据结点的数据。
        /// </summary>
        /// <param name="path">相对于 node 的查找路径。</param>
        /// <param name="data">要设置的数据</param>
        /// <param name="node">查找起始结点</param>
        public void Set(string path, object data, DataNode node = null)
        {
            DataNode current = GetOrAdd(path, node);
            current.SetData(data);
        }
        #endregion


    }
}
