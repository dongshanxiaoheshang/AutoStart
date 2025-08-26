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
        //��ͼ�Ŀ��
        public int mapW, mapH;

        //���Ӷ���
        public AStartNode[,] mapNodes;
        //�����͹ر��б�
        public List<AStartNode> openList = new List<AStartNode>();
        public List<AStartNode> closeLists = new List<AStartNode>();

        //��ʼ����ͼ
        public void InitMap(int w, int h,int stopNum)
        {
            //��ʼ����ͼ
            mapNodes = new AStartNode[w, h];
            mapW = w;
            mapH = h;

            //ʵ������ͼ
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

        //Ѱ��·��
        public List<AStartNode> FindPath(Vector2 startPos, Vector2 endPos)
        {
            //����б�
            openList.Clear();
            closeLists.Clear();

            int spX = Mathf.FloorToInt(startPos.x);
            int spY = Mathf.FloorToInt(startPos.y);
            int epX = Mathf.FloorToInt(endPos.x);
            int epY = Mathf.FloorToInt(endPos.y);

            var startNode = mapNodes[spX, spY];
            var endNode = mapNodes[epX, epY];

            //�ж��������յ��Ƿ��ڵ�ͼ��
            if (InMapExternal(spX, spY) || InMapExternal(epX, epY))
            {
                Debug.Log("�������յ��ٵ�ͼ��");
                return null;
            }
            //�ж��������յ��Ƿ����ϰ�         
            if (startNode.type == EAStartNodeType.stop || endNode.type == EAStartNodeType.stop)
            {
                Debug.Log("�������յ����ϰ�");
                return null;
            }
            closeLists.Add(startNode);//��������ر��б���
            bool success = FindEndPoint(spX, spY, epX, epY);
            //�Ƿ��ҵ��յ�
            if (success)
            {
                //���յ㿪ʼ��ǰ��һֱ�ҳ�ʼ��
                var pathEndNode = closeLists[closeLists.Count - 1];
                var pathList = new List<AStartNode>();
                var curNode = pathEndNode;
                //·������
                while (true)
                {
                    pathList.Add(curNode);
                    if (curNode.father == null)
                    {
                        break;
                    }
                   
                        curNode = curNode.father;
                    
                }
                //��Ϊ�Ǵ��յ㿪ʼ������ҵģ���·�����ʱ��Ҫ����㿪ʼ����תһ���б�
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
                    //���ĵ�����
                    if (i == 0 & j == 0)
                    {
                        continue;
                    }
                    int cx = sx + i;
                    int cy = sy + j;
                  
                    //�����ǰ������յ㣬����
                    if (cx== ex && cy == ey)
                    {
                        return true;
                    }
                    //�жϵ��Ƿ��ڵ�ͼ�� �Ƿ����ϰ����Ƿ����б���
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
                   
                    //���ø��ڵ�
                    curNode.father = startNode;
                    //f=g+h;  g=���ڵ��g+��ǰ�ڵ㵽���ڵ�ľ���
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
            //�ж��Ƿ�Ϊ��·
            if (openList.Count == 0)
            {
                Debug.LogError("����һ����·");
                return false;
            }
            //��openList�������� ���������һ����˵��node1����node2 ������λ��node2��ǰ��
            openList.Sort((node1, node2) => { return node1.f >= node2.f ? 1 : -1; });
            //����С�Ĵӿ����б��Ƴ�������ر��б�
            var minNode = openList[0];
            openList.RemoveAt(0);
            closeLists.Add(minNode);


            //�ҵ���Ŀ���
            if (minNode.x == ex & minNode.y == ey)
            {
                return true;
            }

            //�ݹ飬Ѱ����һ����
            return FindEndPoint(minNode.x,minNode.y,ex,ey);
        }
        //�ж�������Ƿ��ڵ�ͼ��
        bool InMapExternal(int x, int y)
        {
            if (x < 0 || x >= mapW || y < 0 || y >= mapH) return true;
            return false;
        }
    }

}
