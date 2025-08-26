
namespace NewAStart
{
    public enum EAStartNodeType
    {
        walk, stop
    }
    public class AStartNode
    {
        //坐标点
        public int x, y;
        //格子类型
        public EAStartNodeType type;

        //寻路消耗
        public  float f, g, h;

        //父节点
        public AStartNode father;

        public AStartNode(int x, int y, EAStartNodeType type)
        {
            this.x = x; this.y = y; this.type = type;
        }

    }
}

