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
using UnityEngine.UI;

public class ColliderSystem : MonoBehaviour {
    public ColliderTest[] Test;
    public float dis;
    public bool isTrigger;
    public Text text;
    // Use this for initialization


    private void OnDrawGizmos()
    {
       
       // Vector2 posA = Camera.main.ScreenToWorldPoint(Test[0].coliderCenterPos);
       // Vector2 posB = Camera.main.ScreenToWorldPoint(Test[1].coliderCenterPos);
         Gizmos.DrawLine(Test[0].transform.position, Test[1].transform.position);
        dis = Vector2.Distance(Test[0].coliderCenterPos, Test[1].coliderCenterPos);
        if (dis <= Test[0].perspectiveRadius + Test[1].perspectiveRadius)
        {
            isTrigger = true;
            text.text = "触碰了";
        }
        else {
            isTrigger = false;
            text.text = "未触碰";
        }

;    }


}
