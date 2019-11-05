//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #协程句柄# </describe>
// <time> #2019# </time>
//-----------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.Sunny
{
    public class CoroutineHandler : MonoBehaviour
    {
        private CoroutineManager coroutineManager;

        public Coroutine    Coroutine;
        public bool         IsCompleted;
        public bool         IsRunning;


        public void SetCoroutineManager(CoroutineManager coroutineManager)
        {
            this.coroutineManager = coroutineManager;
        }

        public Coroutine StartHandler(IEnumerator rIEnum)
        {
            this.IsCompleted = false;
            this.IsRunning = true;
            this.Coroutine = this.StartCoroutine(this.StartHandler_Async(rIEnum));
            return this.Coroutine;
        }

        private IEnumerator StartHandler_Async(IEnumerator rIEnum)
        {
            yield return rIEnum;
            this.IsRunning = false;
            this.IsCompleted = true;

            // 要等一帧才能把自己删掉，要不然会崩溃
            yield return 0;

            // 自己把自己删掉，并将对应的数据都清空
            coroutineManager.Stop(this);
        }
    }
}
