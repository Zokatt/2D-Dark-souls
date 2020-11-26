using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    public class Enemy : GameObject
    {
        public int hp;
        private Vector2 offset;
        private int speed;
        private int attackTimer;
        private int dmg;
        private SpriteFont enemyKilled;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D enemyJimSprite;
        private Rectangle rectangle;
        private int scale;
        private bool enemyRotate;
        private GameWorld gameWorld;
        private AttackBox attackBox;

        // Giver en position og scalering af enemy
        public Enemy(Vector2 position, int scale, int hp)
        {
            this.position = position;
            this.scale = scale;
            this.hp = hp;
            
        }


        // Enemy's bevægelseshastighed
        public void AiMovement()
        {
            if (enemyRotate == true)
            {
                if (position.X == 0)
                {
                    enemyRotate = false;
                }
                position.X -= 5;
            }
            if (enemyRotate == false)
            {
                if (position.X >= 1000)
                {
                    enemyRotate = true;
                }
                position.X += 5;
            }
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

            gameWorld = new GameWorld();

        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                color = Color.Red;
            }

            if (other is AttackBox)
            {
                attackBox.damage -= hp;
            }

        }

        public override void Update(GameTime gametime)
        {

            AiMovement();
            

            if (hp <= 0)
            {
                gameWorld.enemies.Remove(this);
            }


        }



        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, scale, scale), 
                new Rectangle(1,1, sprite.Width, sprite.Height), color);
            
            
        }

    }
}