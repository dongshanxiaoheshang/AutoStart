using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewAStart
{
    public class AStartManager
    {
        private static AStartManager instance;
        public static AStartManager GetInstance()
        {
            if (instance == null)
                instance = new AStartManager();
            return instance;
        }
        //地图的宽高
        public int mapW, mapH;

        //格子对象
        public AStartNode[,] mapNodes;
        //开启和关闭列表
        public List<AStartNode> openList = new List<AStartNode>();
        public List<AStartNode> closeLists = new List<AStartNode>();

        //初始化地图
        public void InitMap(int w, int h,int stopNum)
        {
            //初始化地图
            mapNodes = new AStartNode[w, h];
            mapW = w;
            mapH = h;

            //实例化地图
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    mapNodes[i, j] = new AStartNode(i, j, EAStartNodeType.walk);
                }
            }
            for (int i = 0; i < stopNum; i++)
            {
                int x = UnityEngine.Random.Range(0, w);
                int y = UnityEngine.Random.Range(0, h);
                mapNodes[x, y].type = EAStartNodeType.stop;
            }

        }

        //寻找路径
        public List<AStartNode> FindPath(Vector2 startPos, Vector2 endPos)
        {
            //清除列表
            openList.Clear();
            closeLists.Clear();

            int spX = Mathf.FloorToInt(startPos.x);
            int spY = Mathf.FloorToInt(startPos.y);
            int epX = Mathf.FloorToInt(endPos.x);
            int epY = Mathf.FloorToInt(endPos.y);

            var startNode = mapNodes[spX, spY];
            var endNode = mapNodes[epX, epY];

            //判断起点或者终点是否在地图外
            if (InMapExternal(spX, spY) || InMapExternal(epX, epY))
            {
                Debug.Log("起点或者终点再地图外");
                return null;
            }
            //判断起点或者终点是否是障碍         
            if (startNode.type == EAStartNodeType.stop || endNode.type == EAStartNodeType.stop)
            {
                Debug.Log("起点或者终点是障碍");
                return null;
            }
            closeLists.Add(startNode);//把起点加入关闭列表中
            bool success = FindEndPoint(spX, spY, epX, epY);
            //是否找到终点
            if (success)
            {
                //从终点开始往前，一直找初始点
                var pathEndNode = closeLists[closeLists.Count - 1];
                var pathList = new List<AStartNode>();
                var curNode = pathEndNode;
                //路径绘制
                while (true)
                {
                    pathList.Add(curNode);
                    if (curNode.father == null)
                    {
                        break;
                    }
                   
                        curNode = curNode.father;
                    
                }
                //因为是从终点开始往起点找的，画路径点的时候要从起点开始，反转一下列表
                pathList.Reverse();
                return pathList;
            }
            return null;

        }
        bool FindEndPoint(int sx, int sy, int ex, int ey)
        {
            var startNode = mapNodes[sx, sy];
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //中心点跳出
                    if (i == 0 & j == 0)
                    {
                        continue;
                    }
                    int cx = sx + i;
                    int cy = sy + j;
                  
                    //如果当前点等于终点，结束
                    if (cx== ex && cy == ey)
                    {
                        return true;
                    }
                    //判断点是否在地图内 是否是障碍，是否再列表中
                    if (InMapExternal(cx, cy) )
                    {
                        continue;
                    }
                    var curNode = mapNodes[cx, cy];
                    if ( curNode.type == EAStartNodeType.stop )
                    {
                        continue;
                    }
                    if (openList.Contains(curNode) || closeLists.Contains(curNode))
                    {
                        continue;
                    }
                   
                    //设置父节点
                    curNode.father = startNode;
                    //f=g+h;  g=父节点的g+当前节点到父节点的距离
                    float d = 1;
                    if (i != 0 || j != 0)
                    {
                        d = 1.4f;
                    }
                    float g = startNode.g + d;

                    float h1 = Mathf.Abs(cx - sx);
                    float h2 = Mathf.Abs(cy - sy);
                    float h = h1 + h2;

                    float f = g + h;
                    curNode.g = g;
                    curNode.h = h;
                    curNode.f = f;

                    openList.Add(curNode);
                }
            }
            //判断是否为死路
            if (openList.Count == 0)
            {
                Debug.LogError("这是一条死路");
                return false;
            }
            //对openList进行排序 从如果返回一，这说明node1大于node2 ，交换位置node2在前面
            openList.Sort((node1, node2) => { return node1.f >= node2.f ? 1 : -1; });
            //把最小的从开放列表移出，放入关闭列表
            var minNode = openList[0];
            openList.RemoveAt(0);
            closeLists.Add(minNode);


            //找到了目标点
            if (minNode.x == ex & minNode.y == ey)
            {
                return true;
            }

            //递归，寻找下一个点
            return FindEndPoint(minNode.x,minNode.y,ex,ey);
        }
        //判断坐标点是否在地图外
        bool InMapExternal(int x, int y)
        {
            if (x < 0 || x >= mapW || y < 0 || y >= mapH) return true;
            return false;
        }
    }

}
