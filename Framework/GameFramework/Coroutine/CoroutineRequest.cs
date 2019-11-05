//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #协程请求# </describe>
// <time> #2019# </time>
//-----------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameFramework.Sunny
{
    public class CoroutineRequest<T> : CustomYieldInstruction where T : class
    {
        public CoroutineHandler Handler;
        private CoroutineManager coroutine = GameFrameworkMode.GetModule<CoroutineManager>();

        public override bool    keepWaiting
        {
            get
            {
                bool result = (this.Handler == null || (this.Handler != null && !this.Handler.IsRunning && this.Handler.IsCompleted));
                return !result;
            }
        }

        public CoroutineRequest<T> Start(IEnumerator rIEnum)
        {
            this.Handler = coroutine.StartHandler(rIEnum);
            return this;
        }

        public void Stop()
        {
            coroutine.Stop(this.Handler);
        }
    }
}