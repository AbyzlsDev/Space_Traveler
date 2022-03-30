using Raylib_cs;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Main
{
    public class osuctbButShit
    {


        public static int screenX = 500;
        public static int screenY = 700;

        public static int stage = 1;
        public static int maxStage = 6;

        public static int x = screenX / 2;
        public static int y = screenY / 2 + 150;

        public static float nextTimeToSpawn = 0f;

        public static float currentTime = 0;

        public static float points;

        public static float playerSpeed = 10f;

        public static float spawnTime = 1f;

        public static float maxSpawnTime = 0.1f;

        public static int enemiesToSpawn = 214748364;

        public static int enemiesSpawned = 0;

        public static int speed = 5;
        public static int maxSpeed = 20;

        public static float previousTime = 0;

        public static float bestTimeEver = 0;

        public static float previousSurvivedTime = 0;

        public static Vector2 circlePosition = new Vector2(x, y);
        public static Vector2 circleScale = new Vector2(50, 50);

        public static Square[] circles = new Square[enemiesToSpawn];

        public static float seconds;

        public static float minutes;

        public static bool colliding = false;

        public static string path1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/save1.dat";

        public static string path2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/save2.dat";

        public static int currentScreen = 0;

        static void Main()
        {

            var thread = new Thread(Start);
            thread.Start();


        }

        public static void LastTime(float number)
        {


            BinaryFormatter bf = new BinaryFormatter();

            FileStream fs = new FileStream(path1, FileMode.Create);



            bf.Serialize(fs, number);


            fs.Close();


        }
        public static float LoadDataLastTime()
        {

            if (File.Exists(path1))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path1, FileMode.Open);

                float data = (float)binaryFormatter.Deserialize(fileStream);

                fileStream.Close();

                return data;

            }

            else
            {

                return 0;

            }



        }

        public static void BestTime(float number)
        {


            BinaryFormatter bf = new BinaryFormatter();

            FileStream fs = new FileStream(path2, FileMode.Create);



            bf.Serialize(fs, number);


            fs.Close();


        }
        public static float LoadDataBestTime()
        {

            if (File.Exists(path2))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path2, FileMode.Open);

                float data = (float)binaryFormatter.Deserialize(fileStream);

                fileStream.Close();

                return data;

            }

            else
            {

                return 0;

            }



        }

        public static void Start()
        {


            previousSurvivedTime = LoadDataLastTime();
            bestTimeEver = LoadDataBestTime();



            Raylib.InitWindow(screenX, screenY, "SpaceTraveler");


            Texture2D[] textures = { Raylib.LoadTexture("D:/space_shooter/SpaceShooter/SpaceShooter/Assets/playerShip.png"),
                                     Raylib.LoadTexture("D:/space_shooter/SpaceShooter/SpaceShooter/Assets/meteorite.png")};

            textures[0].width = 70;
            textures[0].height = 70;

            textures[1].width = 10;
            textures[1].height = 10;


            Raylib.SetTargetFPS(60);



            while (!Raylib.WindowShouldClose())
            {
                switch (currentScreen)
                {

                    case 0:

                        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
                        {

                            currentScreen++;

                        }

                        break;

                    case 1:

                        currentTime = (float)Raylib.GetTime();


                        if (currentTime >= previousTime && previousTime > 0)
                        {


                            if (spawnTime > maxSpawnTime)
                            {

                                spawnTime -= 0.1f;

                            }

                            if (speed < maxSpeed)
                            {

                                speed += 5;

                            }

                            if (stage < maxStage)
                            {

                                stage++;

                            }

                            previousTime = currentTime + 20;


                            points += 10;

                        }

                        if (previousTime == 0) previousTime = currentTime + 20;

                       

                        if (currentTime >= nextTimeToSpawn)
                        {

                            float scale = Raylib.GetRandomValue(3, 7);

                            circles[enemiesSpawned].position = new Vector2(Raylib.GetRandomValue(screenX - screenX, screenX - 50), 0);
                            circles[enemiesSpawned].speed.Y = speed;
                            circles[enemiesSpawned].scale = scale;

                            enemiesSpawned++;

                            nextTimeToSpawn = spawnTime + currentTime;
                        }




                        for (int i = 0; i < enemiesSpawned; i++)
                        {

                            circles[i].position.Y += circles[i].speed.Y;


                        }

                        for (int i = 0; i < enemiesSpawned; i++)
                        {


                            if (circlePosition.X < (circles[i].position.X + circles[i].scale) && (circlePosition.X + circleScale.X) > circles[i].position.X

                                 && circlePosition.Y < (circles[i].position.Y + circles[i].scale) && (circlePosition.Y + circleScale.Y) > circles[i].position.Y) colliding = true;



                        }


                        if (Raylib.IsKeyDown(KeyboardKey.KEY_D)) circlePosition.X += playerSpeed;
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_A)) circlePosition.X -= playerSpeed;
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_S)) circlePosition.Y += playerSpeed;
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_W)) circlePosition.Y -= playerSpeed;

                        break;


                    case 2:

                        if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
                        {

                           Environment.Exit(0);

                           
                        }

                      

                        break;

                }



                if (colliding == true)
                {
                    colliding = false;

                    if (bestTimeEver < points)
                    {

                        bestTimeEver = points;

                    }

                    previousSurvivedTime = points;

                    LastTime(previousSurvivedTime);
                    BestTime(bestTimeEver);


                    foreach (Texture2D textr in textures)
                    {

                        Raylib.UnloadTexture(textr);

                    }

                    currentScreen++;



                }

                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.BLACK);

                switch (currentScreen)
                {

                    case 0:

                        Raylib.DrawText("Space Traveler", screenX / 2 - 90, 50, 30, Color.WHITE);

                        Raylib.DrawText("Last Pts: " + (int)previousSurvivedTime, screenX / 2 - 90, screenY / 2 - 50, 30, Color.WHITE);

                        Raylib.DrawText("Best Pts: " + (int)bestTimeEver, screenX / 2 - 90, screenY / 2, 30, Color.WHITE);

                        Raylib.DrawText("Press space to begin", screenX / 2 - 150, screenY / 2 + 150, 30, Color.WHITE);

                        break;

                    case 1:

                        for (int i = 0; i < enemiesSpawned; i++)
                        {

                            Vector2 vector2 = new Vector2(circles[i].position.X, circles[i].position.Y);

                            Raylib.DrawTextureEx(textures[1], vector2, 0, circles[i].scale, Color.WHITE);
                        }

                        Raylib.DrawTextureV(textures[0], circlePosition, Color.WHITE);


                        Raylib.DrawText("Stage: " + stage, 0, 0, 30, Color.WHITE);

                        Raylib.DrawText("Pts. : " + (int)points, screenX / 2 + 100, 0, 30, Color.WHITE);



                        break;

                    case 2:

                        Raylib.DrawText("Game Over", screenX / 2 - 90, 50, 30, Color.WHITE);

                        Raylib.DrawText("Last Pts: " + (int)previousSurvivedTime, screenX / 2 - 90, screenY / 2 - 50, 30, Color.WHITE);

                        Raylib.DrawText("Best Pts: " + (int)bestTimeEver, screenX / 2 - 90, screenY / 2, 30, Color.WHITE);

                        Raylib.DrawText("Press space to exit", screenX / 2 - 150, screenY / 2 + 150, 30, Color.WHITE);



                        break;

                }


                Raylib.EndDrawing();






            }



        }




    }
}


