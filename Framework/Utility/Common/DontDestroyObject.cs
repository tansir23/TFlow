using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Sunny.Tools
{
    /// <summary>
    /// 不销毁该物体
    /// </summary>
    public class DontDestroyObject : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}
