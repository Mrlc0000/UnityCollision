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
