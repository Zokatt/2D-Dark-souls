using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace _2D_Dark_souls
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D collisionTexture;
        private Player mainPlayer;
        public Camera MainCamera;
        private List<GameObject> gameObjectList;
        private List<Camera> Camera;
        private Enemy enemyJim;

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
            gameObjectList.Add(new Enviroment("StoneGround", new Vector2(1, 200), 5000));

            enemyJim = new Enemy(new Vector2(400, 0), 1000);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            collisionTexture = Content.Load<Texture2D>("Pixel");

            foreach (var item in gameObjectList)
            {
                item.LoadContent(this.Content);
            }

            mainPlayer.LoadContent(this.Content);
            enemyJim.LoadContent(this.Content);
            // TODO: use this.Content to load your game content here

            // Johnny
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            foreach (var item in gameObjectList)
            {
                item.Update(gameTime);
            }
            mainPlayer.Update(gameTime);
            foreach (var item in gameObjectList)
            {
                mainPlayer.CheckCollision(item);
            }
            MainCamera.Update(gameTime);

            enemyJim.Update(gameTime);

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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: MainCamera.TransformMatrix);

            foreach (var item in gameObjectList)
            {
                item.Draw(this._spriteBatch);
                DrawCollisionBox(item);
            }

            DrawCollisionBox(mainPlayer);
            mainPlayer.Draw(this._spriteBatch);

            DrawCollisionBox(enemyJim);
            enemyJim.Draw(this._spriteBatch);


            // TODO: Add your drawing code here

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}