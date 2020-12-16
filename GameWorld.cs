﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace _2D_Dark_souls
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D collisionTexture;
        public static Player mainPlayer;
        public Platform platformGenerator;
        public Camera MainCamera;
        private static List<GameObject> gameObjectList;
        private List<Camera> Camera;
        private List<AttackBox> enemyAttacks;
        private SpriteFont EnemyTakesDmg;
        public Boss greedBoss;
        public List<Enemy> enemies;
        public static List<Enemy> deleteObjects;

        public static SoundEffect attackSound;
        public static SoundEffect playerGotHit;
        private Texture2D backgroundMountain;
        private Texture2D backgroundMountainCloud;
        private Song song;

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
            mainPlayer = new Player(new Vector2(0, 0));
            Camera = new List<Camera>();
            MainCamera = new Camera(mainPlayer);
            greedBoss = new Boss(new Vector2(11000, -700), 100);
            platformGenerator = new Platform();
            platformGenerator.Initialize();

            deleteObjects = new List<Enemy>();
            //Tilføjet en liste med enemies
            enemies = new List<Enemy>();
            enemies.Add(new Enemy(new Vector2(5460, -775), 300, 3));
            enemies.Add(new Enemy(new Vector2(9400, -1385), 300, 3));
            enemies.Add(new Enemy(new Vector2(7530, -1385), 300, 3));
            enemies.Add(new Enemy(new Vector2(1000, -110), 300, 3));
            enemies.Add(new Enemy(new Vector2(3000, -800), 300, 3));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            song = Content.Load<Song>("Bloodborne");
            //Spilles en sang
            MediaPlayer.Play(song);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Tilføjer sprite for collisionboxen
            collisionTexture = Content.Load<Texture2D>("Pixel");
            //Tilføjer sprite for baggrunden
            backgroundMountain = Content.Load<Texture2D>("GreyWall");
            //Tilføjer sprite for nr. 2 baggrund, som skifter ved bossen
            backgroundMountainCloud = Content.Load<Texture2D>("BackgroundMountainCloud");

            foreach (var item in gameObjectList)
            {
                item.LoadContent(this.Content);
            }

            mainPlayer.LoadContent(this.Content);
            greedBoss.LoadContent(this.Content);

            foreach (var item in enemies)
            {
                item.LoadContent(this.Content);
            }
            //Font
            EnemyTakesDmg = Content.Load<SpriteFont>("Score");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (mainPlayer.currentHP <= 1)
            {
                Exit();
            }
            //Der køres funktion Update og CheackCollision i klassen enemy, boss og player
            mainPlayer.Update(gameTime);
            greedBoss.Update(gameTime);
            greedBoss.SetPlayer(mainPlayer.Collision.X, mainPlayer.Collision.Y);
            greedBoss.CheckCollision(mainPlayer);
            mainPlayer.CheckCollision(greedBoss);
            foreach (var attackitem in mainPlayer.attacks)
            {
                greedBoss.CheckCollision(attackitem);
            }
            foreach (var item in gameObjectList)
            {
                item.Update(gameTime);
                mainPlayer.CheckCollision(item);
                greedBoss.CheckCollision(item);
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
            enemyAttacks = greedBoss.GetList();
            foreach (var attacks in enemyAttacks)
            {
                mainPlayer.CheckCollision(attacks);
            }
            //Et forloop bliver kørt af listen deleteObjcets, som fjerner Enemys fra en anden liste.
            foreach (Enemy go in deleteObjects)
            {
                enemies.Remove(go);
            }
            //Listen rydes.
            deleteObjects.Clear();

            if (mainPlayer.pos.X >= 10440)
            {
                greedBoss.activator = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            _spriteBatch.Begin(transformMatrix: MainCamera.TransformMatrix);
            if (greedBoss.activator == false)
            {

                _spriteBatch.Draw(backgroundMountain, new Rectangle((int)mainPlayer.pos.X - (backgroundMountain.Width / 2), (int)mainPlayer.pos.Y - (backgroundMountain.Height / 2) + 50, backgroundMountain.Width, backgroundMountain.Height), Color.White);
            }
            if (greedBoss.activator == true)
            {

                _spriteBatch.Draw(backgroundMountainCloud, new Rectangle((int)mainPlayer.pos.X - (backgroundMountain.Width / 2), (int)mainPlayer.pos.Y - (backgroundMountain.Height / 2) + 50, backgroundMountain.Width, backgroundMountain.Height), Color.White);
            }
            //Collsion boxs bliver tegnet sammen med figuren.
            foreach (var item in gameObjectList)
            {
                item.Draw(this._spriteBatch);
                DrawCollisionBox(item);
            }

            DrawCollisionBox(mainPlayer);
            mainPlayer.Draw(this._spriteBatch);
            DrawCollisionBox(greedBoss);

            foreach (var item in mainPlayer.attacks)
            {
                item.Draw(this._spriteBatch);
                DrawCollisionBox(item);
            }

            greedBoss.Draw(this._spriteBatch);
            drawBoxes = greedBoss.GetList();
            foreach (var item in drawBoxes)
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

        //Funktion som adder Enemes til en liste for at fjerne dem fra spillet samt giver xp til spilleren
        public void Destroy(Enemy go, int xpToPlayer)
        {
            mainPlayer.gainXP(xpToPlayer);
            deleteObjects.Add(go);
        }

        public Player GetPlayer()
        {
            return (mainPlayer);
        }

        public static void AddToList(Enviroment enviroment)
        {
            gameObjectList.Add(enviroment);
        }

        public void exitGame()
        {
            this.Exit();
        }
    }
}