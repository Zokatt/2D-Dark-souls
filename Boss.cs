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
        private int aAnimation;
        private float scale;
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
           
        }
        public void BossAI(GameTime gameTime)
        {
            Animation(gameTime,sprites);
            if (left == true && aAnimation !=2)
            {
                this.position.X -= 10;
            }
            else if (left == false && aAnimation !=2)
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
            if (enemyAndPlayerDistance > 0 && attacking == false)
            {
                left = true;
            }
            else if (enemyAndPlayerDistance < 0  && attacking == false)
            {
                left = false;
            }
            
            if (idleCheck == true)
            {
                MainAttackTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (MainAttackTimer <= 2)
            {
                if (left == true)
                {
                    scale = 2.5f;
                    sprite = tiredLeft;
                }
                else if (left == false)
                {
                    scale = 2.5f;
                    sprite = tiredRigh;
                }
            }
            if (MainAttackTimer <= 4 && MainAttackTimer >= 2)
            {
                tired = false;
                scale = 2.5f;
                sprite = idle;
            }
            
            if (MainAttackTimer>=4 )
            {
                idleCheck = false;
                if (enemyAndPlayerDistance >200 && attacking == false)
                {
                    scale = 1.4f;
                    sprite = LSideRun;
                    BossAI(gametime);
                }
                if (enemyAndPlayerDistance <= 200)
                {
                    attacking = true;
                    scale = 1.4f;
                    BossAI(gametime);
                    Animations(gametime);
                }
                
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f); ;
        }
        private void Animations(GameTime gametime)
        {
            if (attacking == true && tired == false)
            {
                animationTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.3f && aAnimation < 2 && attacking)
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
                MainAttackTimer = 0;
                tired = true;
                idleCheck = true;
                attacking = false;
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
