/** 
 *Copyright(C) 2019 by 
 *All rights reserved. 
 *FileName:      .cs
 *Author:       刘成
 *Version:      
 *UnityVersion：
 *Date:         
 *Description:    接口用于使用ComCollider组件的mono扩展方法
 *History: 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace SnakeCollider
{
    public interface IComColliderStay
    {
        void OnColliderStay(ComCollider comCollider);
    }
    public interface IComColliderEnter
    {
        void OnColliderEnter(ComCollider comCollider);
    }
    public interface IComColliderExit
    {
        void OnColliderExit(ComCollider comCollider);
    }
}
