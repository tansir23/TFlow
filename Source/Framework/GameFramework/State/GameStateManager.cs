﻿//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2019 TanSir. All rights reserved.
// </copyright>
// <describe> #游戏状态管理类# </describe>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework.Taurus
{
    public sealed class GameStateManager : GameFrameworkModule,IUpdate,IFixedUpdate
    {
        #region 属性
        private GameStateContext _stateContext;
        private GameState _startState;
        /// <summary>
        /// 当前的游戏状态
        /// </summary>
        public GameState CurrentState
        {
            get
            {
                if (_stateContext == null)
                    return null;
                return _stateContext.CurrentState;
            }
        }
        #endregion

        #region 外部接口
        /// <summary>
        /// 创建游戏状态的环境
        /// </summary>
        /// <param name="assembly">重写游戏状态所在的程序集</param>
        public void CreateContext(Assembly assembly)
        {
            if (_stateContext != null)
                return;

            GameStateContext stateContext = new GameStateContext();
            List<GameState> listState = new List<GameState>();

            Type[] types = assembly.GetTypes();
            foreach (var item in types)
            {
                object[] attribute = item.GetCustomAttributes(typeof(GameStateAttribute), false);
                if (attribute.Length <= 0 || item.IsAbstract)
                    continue;
                GameStateAttribute stateAttribute = (GameStateAttribute)attribute[0];
                if (stateAttribute.StateType == GameStateType.Ignore)
                    continue;
                object obj = Activator.CreateInstance(item);
                GameState gs = obj as GameState;
                if (gs != null)
                {
                    listState.Add(gs);
                    if (stateAttribute.StateType == GameStateType.Start)
                        _startState = gs;
                }
            }
            stateContext.SetAllState(listState.ToArray());
            _stateContext = stateContext;
        }
        /// <summary>
        /// 设置状态开始
        /// </summary>
        public void SetStateStart()
        {
            if (_stateContext != null && _startState != null)
                _stateContext.SetStartState(_startState);
        }
        #endregion

        #region 重写函数
        /// <summary>
        /// 渲染帧函数
        /// </summary>
        public void OnUpdate()
        {
            if (_stateContext != null)
                _stateContext.Update();
        }
        /// <summary>
        /// 固定帧函数
        /// </summary>
        public void OnFixedUpdate()
        {
            if (_stateContext != null)
                _stateContext.FixedUpdate();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            _stateContext.Close();
            _stateContext = null;
        }
        #endregion
    }
}
