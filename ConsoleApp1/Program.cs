using Raylib;
using Collision;
using static Raylib.Raylib;

namespace ConsoleApp1
{
    static class Program
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            int screenWidth = 800;
            int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - basic window");

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------
            MyShape triangle = new MyShape();
            triangle.MyPoints.Add(new Vector2(10, 10));
            triangle.MyPoints.Add(new Vector2(20, 30));
            triangle.MyPoints.Add(new Vector2(30, 10));
            triangle.MyPoints.Add(new Vector2(10, 10));
            triangle.position = new Vector2(100, 100);
            triangle.color = Color.GREEN;

            MyShape star = new MyShape();
            star.MyPoints.Add(new Vector2(5, 40));
            star.MyPoints.Add(new Vector2(20, 0));
            star.MyPoints.Add(new Vector2(35, 40));
            star.MyPoints.Add(new Vector2(-5, 20));
            star.MyPoints.Add(new Vector2(45, 20));
            star.MyPoints.Add(new Vector2(5, 40));
            star.position = new Vector2(200, 100);
            star.color = Color.GOLD;

            Circle circleCol = new Circle(new Vector2(300, 300), 20);

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // TODO: Update your variables here
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(Color.RAYWHITE);

                DrawText("Congrats! You created your first window!", 190, 200, 20, Color.LIGHTGRAY);

                if (IsKeyDown(KeyboardKey.KEY_W))
                {
                    star.position.y -= star.Swooce;
                }
                if (IsKeyDown(KeyboardKey.KEY_A))
                {
                    star.position.x -= star.Swooce;
                }
                if (IsKeyDown(KeyboardKey.KEY_S))
                {
                    star.position.y += star.Swooce;
                }
                if (IsKeyDown(KeyboardKey.KEY_D))
                {
                    star.position.x += star.Swooce;
                }

                if (triangle.collisionBox.Overlaps(star.collisionBox))
                {
                    triangle.color = Color.RED;
                }
                else
                {
                    triangle.color = Color.GREEN;
                }

                if (circleCol.Overlaps(star.collisionBox))
                {
                    star.color = Color.SKYBLUE;
                }
                else
                {
                    star.color = Color.GOLD;
                }

                DrawCircle(300, 300, 20, Color.PURPLE);

                triangle.Update();
                triangle.Draw();
                triangle.position.x += triangle.Swooce;
                if (triangle.position.x <= 99 || triangle.position.x >= 300) triangle.Swooce = -triangle.Swooce;

                star.Update();
                star.Draw();
                

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        
            // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
