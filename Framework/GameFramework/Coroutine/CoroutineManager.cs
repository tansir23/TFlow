//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #Coroutine协程管理类# </describe>
// <author> tansir </author>
//-----------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System;

namespace GameFramework.Sunny
{
    public class CoroutineManager : GameFrameworkModule
    {
        private GameObject mCoroutineRootObj;

        public CoroutineManager() {

        }

        public void Initialize(GameObject rootObj)
        {
            mCoroutineRootObj = rootObj;
        }

        public CoroutineHandler StartHandler(IEnumerator rIEnum)
        {
            var rCourtineObj = CreateGameObject(this.mCoroutineRootObj, "coroutine");
            CoroutineHandler rHandler = rCourtineObj.ReceiveComponent<CoroutineHandler>();
            rHandler.SetCoroutineManager(this);
            rHandler.StartHandler(rIEnum);
            return rHandler;
        }

        public Coroutine Start(IEnumerator rIEnum)
        {
            return this.StartHandler(rIEnum).Coroutine;
        }

        public void Stop(CoroutineHandler rCoroutineHandler)
        {
            if (rCoroutineHandler != null)
            {
                rCoroutineHandler.StopAllCoroutines();
                GameObject.DestroyImmediate(rCoroutineHandler.gameObject);
                rCoroutineHandler.Coroutine = null;
            }
            rCoroutineHandler = null;
        }

        private GameObject CreateGameObject(GameObject rTemplateGo, GameObject rParentGo)
        {
            GameObject rGo = GameObject.Instantiate(rTemplateGo);
            rGo.transform.parent = rParentGo.transform;

            rGo.name = rTemplateGo.name;
            rGo.transform.localPosition = Vector3.zero;
            rGo.transform.localRotation = Quaternion.identity;
            rGo.transform.localScale = Vector3.one;

            return rGo;
        }

        private GameObject CreateGameObject(GameObject rParentGo, string rName, params Type[] rComps)
        {
            GameObject rGo = new GameObject(rName, rComps);
            rGo.transform.parent = rParentGo.transform;

            rGo.transform.localPosition = Vector3.zero;
            rGo.transform.localRotation = Quaternion.identity;
            rGo.transform.localScale = Vector3.one;

            return rGo;
        }

        public override void OnClose()
        {
        }
    }
}

