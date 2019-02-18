/**
*Copyright(C) 2015 by #COMPANY#
*All rights reserved.
*FileName: #SCRIPTFULLNAME#
*Author: #AUTHOR#
*Version: #VERSION#
*UnityVersion：#UNITYVERSION#
*Date: #DATE#
*Description:
*History:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest : MonoBehaviour {
    public float radius;//如果相机透视关系的话
    public Vector3 center;
    public bool isTrigger;

    public float perspectiveRadius;//透视下的半径变化
    public Vector2 coliderCenterPos;//碰撞器屏幕中心位置
    public Vector2 coliderSidePos;//碰撞器屏幕边界位置

    private void OnDrawGizmos()
    {
        if (!enabled) return;

        Gizmos.color = isTrigger ? Color.yellow * 0.8f : Color.green * 0.8f;
        coliderCenterPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 pos = transform.position;
        pos.x += radius;
        coliderSidePos = Camera.main.WorldToScreenPoint(pos);


        perspectiveRadius = Vector2.Distance(coliderCenterPos, coliderSidePos);
        MyGizmos.DrawCircle(transform.position+ center, radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y), 60);
    }
}



public static partial class MyGizmos
{
    public static void DrawCircle(Vector3 center, float radius, int edgeNumber = 360)
    {
        Vector3 beginPoint = center + Vector3.right * radius;       //三角函数角度是从正右方开始的，画圆起始点是最右边的点
        for (int i = 1; i <= edgeNumber; i++)
        {
            float angle = 2 * Mathf.PI / edgeNumber * i;

            float x = radius * Mathf.Cos(angle) + center.x;
            float y = radius * Mathf.Sin(angle) + center.y;
            Vector3 endPoint = new Vector3(x, y, center.z);

            Gizmos.DrawLine(beginPoint, endPoint);

            beginPoint = endPoint;
        }
    }
}