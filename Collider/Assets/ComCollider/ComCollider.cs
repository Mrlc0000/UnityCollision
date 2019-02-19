using System;
using System.Collections.Generic;
using UnityEngine;
namespace SnakeCollider
{
    public class ComCollider : MonoBehaviour
    {

        #region 参数
        public float radius;//半径设置
        public Vector3 center;
        public bool isTrigger;

        public float perspectiveRadius;//用于判定碰撞的 半径系数
        [HideInInspector]
        public Vector2 ColiderCenterPos {
            set {
                if (coliderCenterPos != value) {
                    //限定只有左边的值改变的时候才更新regions
                    if (ComColliderSystem.Instance != null)
                        ComColliderSystem.Instance.onUpdateRegion(this);
                }
                coliderCenterPos = value;

            }
            get { return coliderCenterPos;  }
        }
       // [SerializeField]
        private Vector2 coliderCenterPos;//碰撞器屏幕中心位置
        private Vector2 coliderSidePos;//碰撞器屏幕边界位置
        #endregion

        #region 事件
        public Action<ComCollider> onComCollider;//碰撞事件
        public  Action<ComCollider> onComColliderStay;//碰撞事件
        public Action<ComCollider> onComColliderEnter;//碰撞事件刚进入
        public Action<ComCollider> onComColliderExit;//碰撞事件刚进入
        private List<ComCollider> colliderList;//储存的碰撞函数
        #endregion
        public IComColliderStay iComColliderStay;
        public IComColliderEnter iComColliderEnter;
        public IComColliderExit iComColliderExit;
        private void OnEnable()
        {
            //自己的判定函数
            onComCollider += CheckOutCollider;
            //查询是否有MONO继承该接口  用于判定是否实现其接口
            iComColliderStay = GetComponent<IComColliderStay>();
            iComColliderEnter = GetComponent<IComColliderEnter>();
            iComColliderExit = GetComponent<IComColliderExit>();
            if (iComColliderStay != null)
                onComColliderStay += iComColliderStay.OnColliderStay;

            if (iComColliderEnter != null)
                onComColliderEnter += iComColliderEnter.OnColliderEnter;

            if (iComColliderExit != null)
                onComColliderExit += iComColliderExit.OnColliderExit;
        }
        private void OnDisable()
        {
            onComColliderStay -= CheckOutCollider;

            if (iComColliderStay != null)
                onComColliderStay -= iComColliderStay.OnColliderStay;

            if (iComColliderEnter != null)
                onComColliderEnter -= iComColliderEnter.OnColliderEnter;

            if (iComColliderExit != null)
                onComColliderExit -= iComColliderExit.OnColliderExit;

        }

        /// <summary>
        /// 检查碰撞停留的函数
        /// </summary>
        /// <param name="comCollider"></param>
        private void CheckOutCollider(ComCollider comCollider) {
            if (iComColliderStay != null) {
                if (onComColliderStay != null)
                {
                    onComColliderStay(comCollider);
                }
            }
                CheckOutColliderEnter(comCollider);
        }

        /// <summary>
        /// 是否colliderEnter
        /// </summary>
        /// <param name="comCollider"></param>
        private void CheckOutColliderEnter(ComCollider comCollider) {
            if (colliderList == null)
                colliderList = new List<ComCollider>();

            ComCollider com = colliderList.Find((x) =>{ return x == comCollider; });
            if (com == null)
            {
                //TODO执行一次Enter;
                if (onComColliderEnter != null)
                {
                    onComColliderEnter(comCollider);
                }
                colliderList.Add(comCollider);
            }
        }
        /// <summary>
        /// 检查是否有CheckCollider出范围 只有 mono有接口时候才会实现该方法
        /// </summary>
        private void CheckOutColliderExit() {
            if (colliderList== null)//防止属性为空
                return;
            if (colliderList.Count == 0)//防止多余的计算
                return;

            for (int i = 0; i < colliderList.Count; i++)
            {
                if (colliderList[i] == null)
                {
                    colliderList.RemoveAt(i);//防止Exit情况下数组有的物体已经摧毁的现象
                }
                else
                {

                    float dis = Vector2.Distance(colliderList[i].ColiderCenterPos, this.ColiderCenterPos);
                    float tmepRadius = colliderList[i].perspectiveRadius + this.perspectiveRadius;
                    if (dis >= tmepRadius)
                    {
                        if (onComColliderExit != null)
                        {
                            onComColliderExit(colliderList[i]);
                        }
                        colliderList.RemoveAt(i);
                    }
                }
            }
        }

        private void Update()
        {
            if (!isTrigger)
                return;

            if (!enabled) return;

            ColiderCenterPos = Camera.main.WorldToScreenPoint(transform.position + center);
            coliderSidePos = Camera.main.WorldToScreenPoint(transform.position + Camera.main.transform.right * radius);
            perspectiveRadius = Vector2.Distance(ColiderCenterPos, coliderSidePos);

            CheckOutColliderExit();
        }
        private void OnDrawGizmos()
        {
            if (!enabled) return;
            Gizmos.color = isTrigger ? Color.yellow * 0.8f : Color.green * 0.8f;
            ColiderCenterPos = Camera.main.WorldToScreenPoint(transform.position+ center);
            Vector3 pos = transform.position;
          //  pos.x += radius;
          //修改成相机的Pos逻辑 可以防止Camera的逻辑
            coliderSidePos = Camera.main.WorldToScreenPoint(pos+Camera.main.transform.right*radius);
            perspectiveRadius = Vector2.Distance(ColiderCenterPos, coliderSidePos);
            MonoBehaviourEX.DrawCircle(transform.position + center, radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), 60);
        }
    }
}


