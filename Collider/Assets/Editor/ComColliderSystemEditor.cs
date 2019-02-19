

using UnityEngine;
using SnakeCollider;
using UnityEditor;

[CustomEditor(typeof(ComColliderSystem))]
[CanEditMultipleObjects]
public class ComColliderSystemEditor : Editor
{
    [SerializeField]
    ComColliderSystem script;

    // Use this for initialization
    private void Awake()
    {
        script = (ComColliderSystem)target;
    }
    public override void OnInspectorGUI()
    {


        base.OnInspectorGUI();

        GUILayout.BeginVertical();
        //ranks 行列数
        //size 尺寸大小
        //Speace 空间更改

        script.RanksCounts = EditorGUILayout.Vector2IntField("RanksCounts", script.RanksCounts);


        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("生成宫格", GUILayout.MaxWidth(60), GUILayout.Height(20)))
        {

            script.SetRegion();
        }
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("删除宫格", GUILayout.Width(60), GUILayout.Height(20)))
        {

            script.ClearRegion();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("显示线条", GUILayout.Width(60), GUILayout.Height(20)))
        {

            script.isShowLine = !script.isShowLine;
        }


        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (script.grids != null)
        {
            for (int x = 0; x < script.grids.GetLength(0); x++)
            {
                GUILayout.BeginVertical();
                GUIStyle gUIStyle = new GUIStyle();
                gUIStyle.fontSize = 10;
                for (int y = script.grids.GetLength(1)-1; y >= 0; y--)
                {
                   
                    EditorGUILayout.TextField(string.Concat("坐标",x,y)+script.grids[x, y], gUIStyle,GUILayout.Width(5));
                }
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndHorizontal();




        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }


    }

   
}
