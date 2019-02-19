/*
        碰撞触发器
 */
using System.Collections.Generic;
using UnityEngine;

namespace SnakeCollider
{
    [System.Serializable]
    public class ComRegion
    {
        public int index;
        public Vector2 UpAndDown;//上下区域
        public Vector2 LeftAndRight;//左右区域
        public ComRegion(Vector2 leftAndRight, Vector2 upAndDown) {
            UpAndDown = upAndDown;
            LeftAndRight = leftAndRight;
        }

        ~ComRegion() {
            colliderPools = null;
        }

        public List<ComCollider> colliderPools;

        /// <summary>
        /// 检测是否有object进入到此区域内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool isEnterThisRgion( ComCollider comCollider) {
            //判定方形区域
            if (comCollider.ColiderCenterPos.x >= LeftAndRight.x && comCollider.ColiderCenterPos.x < LeftAndRight.y) {
                if (comCollider.ColiderCenterPos.y >= UpAndDown.x && comCollider.ColiderCenterPos.y < UpAndDown.y)
                {
                    AddColliderToPools(comCollider);
                    //   Debug.Log(string.Concat(comCollider.name, "离开到Region", index));
                    return false;
                }
                else {
                    RemoveColliderFromPools(comCollider);
                    //  Debug.Log(string.Concat(comCollider.name, "进入到Region", index));
                    return true;
                }
            }
            else {
                RemoveColliderFromPools(comCollider);
              //  Debug.Log(string.Concat(comCollider.name, "离开到Region", index));
                return false;
            }
        }
        /// <summary>
        /// 添加进入此碰撞器的物体到区块池子
        /// </summary>
        /// <param name="obj"></param>
        public void AddColliderToPools(ComCollider obj) {
            bool isContain = colliderPools.Find(x => { return x == obj; });
            if (!isContain)
                colliderPools.Add(obj);
        }
        /// <summary>
        /// 移除此碰撞器的物体到区块池子
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveColliderFromPools(ComCollider obj)
        {
            bool isContain = colliderPools.Find(x => { return x == obj; });
            if (isContain)
                colliderPools.Remove(obj);
        }
        /// <summary>
        /// 检测这个区块的池子的碰撞函数
        /// </summary>
        private void CheckThisPoolCollision() {
      

            for (int i = 0; i < colliderPools.Count; i++)
            {
                if (colliderPools[i] == null) {
                    colliderPools.RemoveAt(i);//防止出现collider出现空的现象
                    continue;
                }
                for (int a = 0; a < colliderPools.Count; a++)
                {
                    if (colliderPools[a] == null)
                    {
                        colliderPools.RemoveAt(a);//防止出现collider出现空的现象
                        continue;
                    }

                    if (i != a) {
                        float dis = Vector2.Distance(colliderPools[i].ColiderCenterPos, colliderPools[a].ColiderCenterPos);
                        //Debug.Log(dis);
                        float radius = colliderPools[i].perspectiveRadius + colliderPools[a].perspectiveRadius;

                        if (dis < radius) {//半径在范围内的时候则 触发Stay
                            if (colliderPools[i].onComCollider != null&& colliderPools[i].isTrigger)
                                colliderPools[i].onComCollider(colliderPools[a]);
                                if (colliderPools[a].onComCollider != null&& colliderPools[a].isTrigger)
                                    colliderPools[a].onComCollider(colliderPools[i]);
                        }                      
                    }
                }
            }
        }

        public void Update() {
            CheckThisPoolCollision();
        }
    }
}
