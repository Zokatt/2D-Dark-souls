using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;

namespace _2D_Dark_souls
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D collisionTexture;
        public Player mainPlayer;
        public Camera MainCamera;
        private List<GameObject> gameObjectList;
        private List<Camera> Camera;
        public List<Enemy> enemies;
        private List<AttackBox> enemyAttacks;
        private SpriteFont EnemyTakesDmg;
        public static List<Enemy> deleteObjects;
        public static SoundEffect attackSound;
        public static SoundEffect playerGotHit;
        private Texture2D backgroundMountain;

        private List<AttackBox> drawBoxes;
        public static Rectangle screenBounds = new Rectangle(0, 0, 1600, 900);

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = screenBounds.Width;
            _graphics.PreferredBackBufferHeight = screenBounds.Height;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameObjectList = new List<GameObject>();
            Camera = new List<Camera>();
            mainPlayer = new Player(new Vector2(0, 0));
            MainCamera = new Camera(mainPlayer);


            //First level________________________________________________________________________________________________________
            for (int i = 1; i < 10; i++)
            {
                gameObjectList.Add(new Enviroment("StoneGround", new Vector2(-100, 285 + 85 * -i), 100));
            }

            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(0, 200), 500));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(500, 200), 500));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(1000, 200), 500));

            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(525, -115), 250));    //Floating ground

            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(1500, 200), 500));
            //Second level________________________________________________________________________________________________________
            for (int i = 1; i < 8; i++)
            {
                gameObjectList.Add(new Enviroment("StoneGround", new Vector2(1800 + (150 * i), 170 + 90 * -i), 150));
            }
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(3000, -535), 350));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(3350, -535), 350));

            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(3450, -850), 350));    //Floating ground
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(4100, -850), 350));    //Floating ground

            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(3700, -535), 350));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(4050, -535), 350));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(4400, -535), 350));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(4750, -535), 350));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(5100, -535), 350));
            //Third level________________________________________________________________________________________________________
            for (int i = 1; i < 8; i++)
            {
                gameObjectList.Add(new Enviroment("StoneGround", new Vector2(5300 + (150 * i), -535 + 90 * -i), 150));
            }
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(6500, -1235), 400));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(6900, -1235), 400));

            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(7000, -1550), 350));   //Floating ground
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(7750, -1550), 175));   //Floating ground
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(8450, -1550), 350));   //Floating ground

            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(7300, -1235), 400));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(7700, -1235), 400));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(8100, -1235), 400));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(8500, -1235), 400));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(8900, -1235), 550));
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(9450, -1235), 550));

            deleteObjects = new List<Enemy>();
            //Tilføjet en liste med enemies
            enemies = new List<Enemy>();
            enemies.Add(new Enemy(new Vector2(1000, -110), 300, 3));
            enemies.Add(new Enemy(new Vector2(3000, -800), 300, 3));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            collisionTexture = Content.Load<Texture2D>("Pixel");
            backgroundMountain = Content.Load<Texture2D>("GreyWall");

            foreach (var item in gameObjectList)
            {
                item.LoadContent(this.Content);
            }

            mainPlayer.LoadContent(this.Content);

            foreach (var item in enemies)
            {
                item.LoadContent(this.Content);
            }

            EnemyTakesDmg = Content.Load<SpriteFont>("Score");
            // TODO: use this.Content to load your game content here

            // Johnny
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            mainPlayer.Update(gameTime);
            foreach (var item in gameObjectList)
            {
                item.Update(gameTime);
                mainPlayer.CheckCollision(item);
                foreach (var enemy in enemies)
                {
                    enemy.CheckCollision(item);
                }
            }

            MainCamera.Update(gameTime);
            foreach (var item in enemies)
            {
                item.Update(gameTime);
                item.SetPlayer(mainPlayer.Collision.X);
                foreach (var attackitem in mainPlayer.attacks)
                {
                    item.CheckCollision(attackitem);
                }
                item.CheckCollision(mainPlayer);
                mainPlayer.CheckCollision(item);
                enemyAttacks = item.GetList();
                foreach (var attacks in enemyAttacks)
                {
                    mainPlayer.CheckCollision(attacks);
                }
            }
            foreach (Enemy go in deleteObjects)
            {
                enemies.Remove(go);
            }
            deleteObjects.Clear();

            base.Update(gameTime);
        }

        private void DrawCollisionBox(GameObject go)
        {
            //Der laves en streg med tykkelsen 1 for hver side af Collision.
            Rectangle topLine = new Rectangle(go.Collision.X, go.Collision.Y, go.Collision.Width, 1);
            Rectangle bottomLine = new Rectangle(go.Collision.X, go.Collision.Y + go.Collision.Height, go.Collision.Width, 1);
            Rectangle rightLine = new Rectangle(go.Collision.X + go.Collision.Width, go.Collision.Y, 1, go.Collision.Height);
            Rectangle leftLine = new Rectangle(go.Collision.X, go.Collision.Y, 1, go.Collision.Height);
            //Der tegnes en streg med tykkelsen 1 for hver side af Collision med collsionTexture med farven rød.
            _spriteBatch.Draw(collisionTexture, topLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, bottomLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, rightLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, leftLine, Color.Red);
        }

        public static void Destroy(Enemy go)
        {
            deleteObjects.Add(go);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin(transformMatrix: MainCamera.TransformMatrix);

            _spriteBatch.Draw(backgroundMountain, new Rectangle((int)mainPlayer.pos.X-(backgroundMountain.Width/2), (int)mainPlayer.pos.Y-(backgroundMountain.Height/2)+50, backgroundMountain.Width, backgroundMountain.Height), Color.White);

            foreach (var item in gameObjectList)
            {
                item.Draw(this._spriteBatch);
                DrawCollisionBox(item);
            }

            DrawCollisionBox(mainPlayer);
            mainPlayer.Draw(this._spriteBatch);

            foreach (var item in mainPlayer.attacks)
            {
                item.Draw(this._spriteBatch);
                DrawCollisionBox(item);
            }

            foreach (Enemy item in enemies)
            {
                DrawCollisionBox(item);
                item.Draw(this._spriteBatch);
                drawBoxes = item.GetList();
                foreach (AttackBox boxes in drawBoxes)
                {
                    boxes.Draw(this._spriteBatch);
                    DrawCollisionBox(boxes);
                }
            }

            //_spriteBatch.DrawString(EnemyTakesDmg, "Enemy HP: " + enemies[0].hp, new Vector2(800, -500), Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public Player GetPlayer()
        {
            return (mainPlayer);
        }
    }
}