using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    internal class Enemy : GameObject
    {
        private int hp;
        private Vector2 offset;
        private int speed;
        private int attackTimer;
        private int dmg;
        private SpriteFont enemyKilled;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D enemyJimSprite;
        private Rectangle rectangle;
        private int enemyXMovement;
        private Vector2 position;

        public Enemy(Vector2 position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
        }



        public void AiMovement()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.E))
            {
                position.Y -= 10;
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {

            sprite = contentManager.Load<Texture2D>("EnemyGhostJimV2");



        }

        public override void OnCollision(GameObject other)
        {
        }

        public override void Update(GameTime gametime)
        {

            AiMovement();



        }



        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
            color = Color.Black;
        }

    }
}