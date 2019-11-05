//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #事件参数基类# </describe>
//-----------------------------------------------------------------------

using System;

namespace GameFramework.Sunny
{
    public interface IEventArgs
    {
        int Id { get; }
    }

    public abstract class GameEventArgs<T>: IEventArgs where T : IEventArgs
    {
        public int Id
        {
            get
            {
                return typeof(T).GetHashCode();
            }
        }
    }
}
