
namespace NewAStart
{
    public enum EAStartNodeType
    {
        walk, stop
    }
    public class AStartNode
    {
        //�����
        public int x, y;
        //��������
        public EAStartNodeType type;

        //Ѱ·����
        public  float f, g, h;

        //���ڵ�
        public AStartNode father;

        public AStartNode(int x, int y, EAStartNodeType type)
        {
            this.x = x; this.y = y; this.type = type;
        }

    }
}

