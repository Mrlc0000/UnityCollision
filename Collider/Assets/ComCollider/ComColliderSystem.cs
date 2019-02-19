/*
    用于ComCollider的碰撞事件转发

 */


using System;
using UnityEngine;

namespace SnakeCollider
{
    public class ComColliderSystem : SingeltonCommon<ComColliderSystem>
    {
        #region Editor
        //行列数
        public Vector2Int RanksCounts
        {
            set
            {
                ranksCounts = value;
            }
            get { return ranksCounts; }
        }
        [HideInInspector]
        public Vector2Int ranksCounts;

        //间隔 x,y轴
        public Vector2 Spaceing
        {
            set
            {
                spaceing = value;
                ChangeSpaceing();
            }
            get { return spaceing; }
        }
        [HideInInspector]
        private Vector2 spaceing;
        public Vector2[,] grids;
        public ComRegion[] comRegions;
        public bool isShowLine;
        /// <summary>
        /// 设置宫格 根据屏幕坐标
        /// </summary>
        public void SetRegion()
        {

            if (grids != null)
            {
                grids = null;
            }
#if UNITY_EDITOR
            float sx = Camera.main.pixelWidth / (RanksCounts.x-1);
            float sy = Camera.main.pixelHeight / (RanksCounts.y-1);
#else
            float sx = Screen.currentResolution.width / (RanksCounts.x);
            float sy = Screen.currentResolution.height / (RanksCounts.y);

#endif


            Debug.Log(Camera.main.pixelWidth + "_" + Camera.main.pixelHeight);
            Spaceing = new Vector2(sx, sy);
            grids = new Vector2[RanksCounts.x,RanksCounts.y];
            comRegions = new ComRegion[(ranksCounts.x - 1) * (ranksCounts.y - 1)];

            int regionsIndex= 0;
            for (int x = 0; x < RanksCounts.x; x++)
            {
                for (int y = 0; y < RanksCounts.y; y++)
                {
                   
                    grids[x, y] = Camera.main.ScreenToWorldPoint(new Vector3(x * Spaceing.x, y * Spaceing.y,10));
                    #region 填入网格 偶数格子开始计算

                    int x0 = x;
                    int x1 = x + 1;
                    int y0 = y;
                    int y1 = y + 1;
                    //边界不超过的填入到宫格中 且边界值为相机的像素值
                    if (x1 < ranksCounts.x && y1 < ranksCounts.y)
                    {

                        Vector2 vector2IntA = new Vector3(x0 * Spaceing.x, y0 * Spaceing.y);//左下
                        Vector2 vector2IntB = new Vector3(x1 * Spaceing.x, y0 * Spaceing.y);//右下
                        Vector2 vector2IntC = new Vector3(x0 * Spaceing.x, y1 * Spaceing.y);//左上
                      //  Vector2 vector2IntD = new Vector3(x1 * Spaceing.x, y1 * Spaceing.y);//右上

                        Vector2 vector2IntH = new Vector2(vector2IntA.x, vector2IntB.x);
                        Vector2 vector2Intv = new Vector2(vector2IntA.y, vector2IntC.y);
                        ComRegion comRegion = new ComRegion(vector2IntH, vector2Intv);
                        comRegion.index = regionsIndex;
                        comRegions[regionsIndex] = comRegion;
                        regionsIndex++;
                    }               
                    #endregion
                }
            }
        }
        /// <summary>
        /// 清除网格
        /// </summary>
        public void ClearRegion()
        {
            if (grids != null)
                grids = null;
            if (comRegions != null)
                comRegions = null;
        }
        public void ChangeSpaceing()
        {
            if (grids == null)
                return;
            for (int x = 0; x < grids.GetLength(0); x++)
            {
                for (int y = 0; y < grids.GetLength(1); y++)
                {
                    if (grids[x, y] != null)
                    {
                        grids[x, y] = new Vector3(x * Spaceing.x, y * Spaceing.y);
                    }
                }
            }
        }
        private void OnDrawGizmos()
        {
            if (!isShowLine)
                return;
            if (comRegions == null)
                return;


            for (int i = 0; i < comRegions.Length; i++)
            {
                if (comRegions[i].colliderPools != null)
                {
                    if (comRegions[i].colliderPools.Count != 0)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }
                }
               
                float x0 = comRegions[i].LeftAndRight.x;
                float x1 = comRegions[i].LeftAndRight.y;
                float y0 = comRegions[i].UpAndDown.x;
                float y1 = comRegions[i].UpAndDown.y;

                Vector3 posA = Camera.main.ScreenToWorldPoint(new Vector3(x0, y0, 10));
                Vector3 posB = Camera.main.ScreenToWorldPoint(new Vector3(x1, y0, 10));
                Vector3 posC = Camera.main.ScreenToWorldPoint(new Vector3(x0, y1, 10));
                Vector3 posD = Camera.main.ScreenToWorldPoint(new Vector3(x1, y1, 10));

                Gizmos.DrawLine(posA, posB);
                Gizmos.DrawLine(posB, posD);
                Gizmos.DrawLine(posA, posC);
                Gizmos.DrawLine(posC, posD);

            }        
        }



        #endregion

        #region 事件
        public Action<ComCollider> onUpdateRegion;//用于事实更新区域的广播事件
        private void OnUpdateRegionFunc(ComCollider comCollider) {
            //更新区域
            for (int i = 0; i < comRegions.Length; i++)
            {
                comRegions[i].isEnterThisRgion(comCollider);
            }


        }
        #endregion

        #region Unity回调

        private void OnEnable()
        {
            onUpdateRegion = OnUpdateRegionFunc;//监听事件
            if (grids == null)
            {
                grids = new Vector2[RanksCounts.x, RanksCounts.y];
                for (int x = 0; x < RanksCounts.x; x++)
                {
                    for (int y = 0; y < RanksCounts.y; y++)
                    {

                        grids[x, y] = Camera.main.ScreenToWorldPoint(new Vector3(x * Spaceing.x, y * Spaceing.y, 10));
                    }
                }
            }
        }
        private void OnDisable()
        {
            onUpdateRegion = null;//剔除监听事件
        }
        private void Update()
        {
            //TODO 更新网格的区块 放到collider中自己运行
            //TODO 更新网格的碰撞函数
            for (int i = 0; i < comRegions.Length; i++)
            {
                comRegions[i].Update();
            }
        }

        #endregion




    }
}