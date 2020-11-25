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
        private int x;
        private int y;
        private int scale;

        // Giver en position og scalering af enemy
        public Enemy(Vector2 position, int scale)
        {
            this.position = position;
            this.scale = scale;
            
        }


        // Enemy's bevægelseshastighed
        public void AiMovement()
        {
            
        }

        public override Rectangle Collision
        {
            get
            {
                return new Rectangle(
                    (int)position.X, (int)position.Y, scale, scale);
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

            spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, scale, scale), 
                new Rectangle(1,1, sprite.Width, sprite.Height), Color.Black);
            
            
        }

    }
}