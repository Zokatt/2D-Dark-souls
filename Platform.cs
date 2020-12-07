using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    public class Platform
    {

        public Platform()
        {

        }

        //gameObjectList.Add(new Enviroment("StoneGround", new Vector2(3000, -535), 350));

        public void Initialize()
        {

            Wall();
            //First level________________________________________________________________________________________________________
            FirstLevel();
            //Second level________________________________________________________________________________________________________
            SecondLevel();
            //Third level________________________________________________________________________________________________________
            ThirdLevel();

        }


        private void Wall()
        {
                GameWorld.AddToList(new Enviroment(new Vector2(-200, -450), 750));  
        }

        private void FirstLevel()
        {
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(0, 200), 500));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(500, 200), 500));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(1000, 200), 500));

            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(525, -115), 250));    //Floating ground

            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(1500, 200), 500));
        }
        private void SecondLevel()
        {
            //Second level________________________________________________________________________________________________________
            for (int i = 1; i < 8; i++)
            {
                GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(1800 + (150 * i), 170 + 90 * -i), 150));
            }
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(3000, -535), 350));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(3350, -535), 350));

            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(3450, -850), 350));    //Floating ground
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(4100, -850), 350));    //Floating ground

            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(3700, -535), 350));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(4050, -535), 350));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(4400, -535), 350));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(4750, -535), 350));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(5100, -535), 350));
        }
        private void ThirdLevel()
        {
            //Third level________________________________________________________________________________________________________
            for (int i = 1; i < 8; i++)
            {
                GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(5300 + (150 * i), -535 + 90 * -i), 150));
            }
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(6500, -1235), 400));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(6900, -1235), 400));

            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(7000, -1550), 350));   //Floating ground
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(7750, -1550), 175));   //Floating ground
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(8450, -1550), 350));   //Floating ground

            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(7300, -1235), 400));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(7700, -1235), 400));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(8100, -1235), 400));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(8500, -1235), 400));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(8900, -1235), 550));
            GameWorld.AddToList(new Enviroment("StoneGround", new Vector2(9450, -1235), 550));
        }
    }
}
