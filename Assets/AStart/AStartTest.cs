using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AStartTest : MonoBehaviour
{
    public int mapW = 10;
    public int mapH = 10;

    private bool firstPoint = true;
    private Vector2 clickPos;

    private Dictionary<string, GameObject> goDic;
    // Start is called before the first frame update
    void Start()
    {

        NewAStart.AStartManager.GetInstance().InitMap(mapW, mapH,15);
        StartCoroutine(CreatCube());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                var go = hit.collider.gameObject;
                var names = go.name.Split('_');
                var x = int.Parse(names[0]);
                var y = int.Parse(names[1]);
                var pos = new Vector2(x, y);
                print(pos);
                //如果是第一次点击，保存起来，第二次点击就是终点，进行寻路
                if (firstPoint)
                {                   
                    clickPos = pos;
                    firstPoint = false;
                    var goName = x + "_" + y;
                    var goPath = goDic[goName];
                    goPath.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);//设置物体的颜色
                }
                else
                {
                    Debug.Log(".......................");
                    var pathlist = NewAStart.AStartManager.GetInstance().FindPath(clickPos, pos);
                    if (pathlist != null)
                    {
                        for (int i = 0; i < pathlist.Count; i++)
                        {
                            //通过名字来查找物体
                            var goName = pathlist[i].x + "_" + pathlist[i].y;
                            var goPath = goDic[goName];
                            Debug.Log(pathlist[i]);
                            Debug.Log(goPath.transform.position);
                            goPath.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);//设置物体的颜色
                            
                        }

                    }
                    var goName2 = x + "_" + y;
                    var goPath2 = goDic[goName2];
                    goPath2.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);//设置物体的颜色
                }

            }
        }
    }
    IEnumerator CreatCube()
    {
      var nodes=  NewAStart.AStartManager.GetInstance().mapNodes;
        goDic = new Dictionary<string, GameObject>(mapH * mapW);
        for (int i = 0; i < mapW; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                //创建立方体
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = new Vector3(i + 0.1f * i, 0, j + 0.1f * j);
                go.name = i + "_" + j;
                goDic.Add(go.name, go);
                var node = nodes[i, j];
                if (node.type == NewAStart.EAStartNodeType.stop)
                {
                    go.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                }

                yield return null;
            }
        }
    }
}
