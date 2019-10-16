using System.Collections.Generic;
using Raylib;
using Collision;

namespace ConsoleApp1
{
    class MyShape
    {
        public Vector2 position = new Vector2();
        public List<Vector2> MyPoints = new List<Vector2>();
        public Color color = Color.BLACK;
        public AABB collisionBox = new AABB();
        public float Swooce = 1f;

        public void Draw()
        {
            Vector2 Last = new Vector2();
            for (int idx = 0; idx < MyPoints.Count; idx++)
            {
                if (idx > 0)
                    Raylib.Raylib.DrawLineEx(position + Last, position + MyPoints[idx], 2, color);
                Last = MyPoints[idx];
            }
        }

        public void Update()
        {
            List<Vector2> temp = new List<Vector2>();
            for (int idx = 0; idx < MyPoints.Count; idx++)
            {
                temp.Add(MyPoints[idx] + position);
            }
            collisionBox.Fit(temp);
        }
    }
}
