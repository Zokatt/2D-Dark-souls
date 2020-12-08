using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    public class Boss : GameObject
    {
        private Texture2D idle;
        private Texture2D LSideRun;
        private Texture2D RSideRun;
        private Texture2D tiredLeft;
        private Texture2D tiredRigh;
        private float hp;
        private float enemyAndPlayerDistance;
        private float playerPositionX;
        private Texture2D[] sprites2;
        private float MainAttackTimer;
        private float animationTimer;
        private float tiredTimer;
        private int aAnimation;
        private float scale = 1;
        private bool attacking;
        private bool idleCheck = true;
        private float smashTimer;
        private bool left;
        private bool tired;
        private bool grounded;
        public Boss(Vector2 position, int hp)
        {
            this.position = position;
            this.hp = hp;
            
        }
        public override void LoadContent(ContentManager contentManager)
        {
            sprites = new Texture2D[3];
            sprites2 = new Texture2D[3];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = contentManager.Load<Texture2D>("Sin1GreedLSideAnimation" + (i + 1));
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites2[i] = contentManager.Load<Texture2D>("Sin1GreedRSideAnimation" +( i + 1));
            }
            idle = contentManager.Load<Texture2D>("Sin1Greed");
            LSideRun = contentManager.Load<Texture2D>("Sin1GreedLside");
            RSideRun = contentManager.Load<Texture2D>("Sin1GreedRSide");
            tiredLeft = contentManager.Load<Texture2D>("GreedLeftTired");
            tiredRigh = contentManager.Load<Texture2D>("GreedRightTired");

            sprite = idle;
        }
        public override Rectangle Collision
        {
            get
            {
                return new Rectangle(
                    (int)position.X, (int)position.Y, (int)(sprite.Height*scale),(int)(sprite.Width*scale));
            }
        }
        public override void OnCollision(GameObject other)
        {
            if (other is Player && MainAttackTimer >= 2)
            {
                attacking = true;
            }
        }
        public void BossAI(GameTime gameTime)
        {
            if (left == true)
            {
                this.position.X -= 10;
            }
            else if (left == false)
            {
                this.position.X += 10;
            }
            
        }
        public void SetPlayer(int playerX)
        {
            this.playerPositionX = playerX;
        }
        public override void Update(GameTime gametime)
        {
            enemyAndPlayerDistance = position.X - playerPositionX;
            

            if (idleCheck == true && MainAttackTimer <= 2)
            {
                offsetX = 50;
                MainAttackTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                sprite = idle;
            }
            if (MainAttackTimer >= 2)
            {
                idleCheck = false;
                if (enemyAndPlayerDistance >=1-(sprite.Width / 2) && attacking == false)
                {
                    sprite = LSideRun;
                    left = true;
                    BossAI(gametime);
                }
                else if (enemyAndPlayerDistance <=0 - (sprite.Width/2) && attacking == false)
                {
                    BossAI(gametime);
                    sprite = RSideRun;
                    left = false;
                }
            }
              
            
           
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f); ;
        }
        private void Animations(GameTime gametime)
        {
            if (attacking == true )
            {
                animationTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.3f && aAnimation < 2)
                {
                    aAnimation += 1;
                    animationTimer = 0;
                }
            }
            if (aAnimation == 2)
            {
                smashTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (smashTimer >= 1f)
                {
                    aAnimation += 1;
                }
            }
            if (aAnimation >= sprites.Length)
            {
                
            }
            if (aAnimation < sprites.Length && attacking == true)
            {
                if (left == true)
                {
                    sprite = sprites[aAnimation];
                }
                else if (left == false)
                {
                    sprite = sprites2[aAnimation];
                }
            }
        }
    }
}
