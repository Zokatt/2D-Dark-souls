using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        //private Vector2 offset;
        //private int speed;
        //private int attackTimer;
        //private SpriteFont enemyKilled;
        //private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;
        //private Texture2D enemyJimSprite;
        //private Rectangle rectangle;
        //private bool enemyRotate;
        //private AttackBox attackBox;
        //private int detectPlayerInRangePlus = 200;
        //private int detectPlayerInRangeMinus = -200;
        //private List<GameObject> gameObjectList;
        //private bool noHoldDown;
        //private bool deleteWhen;
        //private float deleteTimer;
        //private Player mainPlayer;
        private GameWorld GameWorld;

        public float lastHP;
        private int scale;
        private int dmg = 2;
        private bool Attacking;
        private Texture2D attackSprite;
        public static List<AttackBox> attacks;
        public List<AttackBox> dAttack;
        private Texture2D collisionTexture;
        private float enemyAndPlayerDistance;
        private float playerPositionX;
        private SoundEffect hitEffect;
        private Texture2D[] HammerAttack;
        private float animationTimer;
        private int cAnimation;
        private Texture2D idle;
        private SpriteFont enemyTakesDmg;
        private Texture2D hpBar;
        private float timer;
        private float cooldownTime = 1;
        private float currentHP;
        private Vector2 offset = new Vector2(-90, -100);
        private double takenDmgTimer;
        private bool takenDmg = false;
        private float maxHp;
        private bool Left; //left is true, right is false
        private float healthPercentage;
        private float visibleWidth;
        private bool deleteWhen;
        private float deleteTimer;

        // Giver en position og scalering af enemy
        public Enemy(Vector2 position, int scale, int hp)
        {
            this.position = position;
            this.scale = scale;
            this.lastHP = 100;
            currentHP = this.lastHP;
            maxHp = this.lastHP;
            this.sprite = idle;
            attacks = new List<AttackBox>();
            dAttack = new List<AttackBox>();
        }

        // Enemy's bevægelseshastighed
        public void AiMovement()
        {
            if (Left == true && Attacking == false)
            {
                position.X -= 2;
            }
            if (Left == false && Attacking == false)
            {
                position.X += 2;
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
            idle = contentManager.Load<Texture2D>("EnemyGhostJimV2");
            attackSprite = contentManager.Load<Texture2D>("EnemyAttackEffect");
            collisionTexture = contentManager.Load<Texture2D>("Pixel");
            enemyTakesDmg = contentManager.Load<SpriteFont>("Score");
            sprites = new Texture2D[1];
            hpBar = contentManager.Load<Texture2D>("HpBar");

            sprites[0] = contentManager.Load<Texture2D>("AttackEffects");

            GameWorld = new GameWorld();

            hitEffect = contentManager.Load<SoundEffect>("PlayerGotHit");

            HammerAttack = new Texture2D[5];
            for (int i = 0; i < HammerAttack.Length; i++)
            {
                HammerAttack[i] = contentManager.Load<Texture2D>(i + "HammerAttackJim");
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
            }
            if (other is AttackBox && timer > cooldownTime)
            {
                hitEffect.Play();
                currentHP -= Player.dmg;
                timer = 0;
            }

            if (lastHP <= 0)
            {
                GameWorld.Destroy(this);
            }
            if (other is Enviroment)
            {
                if (other.pos.Y > position.Y)
                {
                    position.Y = other.Collision.Top - Collision.Height;
                }
                velocity.Y = 0;
            }
        }

        public void SetPlayer(int playerX)
        {
            this.playerPositionX = playerX;
        }

        public override void Update(GameTime gametime)
        {
            if (deleteWhen == true)
            {
                deleteTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (deleteTimer >= 0.1f) //how fast should the attack destroy itself
            {
                foreach (var item in attacks)
                {
                    DestroyItem(item);
                }
                deleteWhen = false;
            }
            if (currentHP != lastHP)
            {
                takenDmgTimer = gametime.TotalGameTime.TotalSeconds;
                takenDmg = true;
                lastHP = currentHP;
            }

            if (takenDmg && gametime.TotalGameTime.TotalSeconds > takenDmgTimer + 2)
            {
                takenDmg = false;
            }

            if (timer < cooldownTime + 1)
                timer += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (exitCollision == true)
            {
                this.velocity.Y += 0.1f;
            }
            this.position.Y += velocity.Y;
            enemyAndPlayerDistance = position.X - playerPositionX;

            if (enemyAndPlayerDistance <= 500 && enemyAndPlayerDistance >= 0 || enemyAndPlayerDistance > -500 && enemyAndPlayerDistance <= 0)
            {

                AiMovement();
                if (enemyAndPlayerDistance <= 500 && enemyAndPlayerDistance >= 0 && Attacking == false)
                {
                    Left = true;
                }
                else if (enemyAndPlayerDistance > -500 && enemyAndPlayerDistance <= -50 && Attacking == false)
                {
                    Left = false;
                }
            }
            if (enemyAndPlayerDistance <=250 && enemyAndPlayerDistance >=0 || enemyAndPlayerDistance >= -250 && enemyAndPlayerDistance <=0)
            {
                Attacking = true;
            }

            if (Attacking == true)
            {
                animationTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.3f && cAnimation < 3)
                {
                    cAnimation += 1;
                    animationTimer = 0;
                }
                else if (animationTimer > 0.2f && cAnimation >= 3)
                {
                    cAnimation += 1;
                    animationTimer = 0;
                }

                if (cAnimation > HammerAttack.Length)
                {
                    Attacking = false;
                    cAnimation = 0;
                }
                if (cAnimation < HammerAttack.Length)
                {
                    sprite = HammerAttack[cAnimation];
                }

                if (cAnimation == 4)
                {
                    deleteTimer = 0;
                    deleteWhen = true;
                    if (Left == true && attacks.Count <=1)
                    {
                        position.X -= 5;
                        attacks.Add(new AttackBox(attackSprite, new Vector2(position.X + -200, position.Y + 100), 200, 2, dmg));
                    }
                    else if (Left == false && attacks.Count <=1)
                    {
                        position.X += 5;
                        attacks.Add(new AttackBox(attackSprite, new Vector2(position.X +300, position.Y + 100), 200, 2, dmg));
                    }
                }
            }
            else
            {
                sprite = idle;
            }
            if (dAttack!= null)
            {
                if (dAttack.Count > 0)
                {
                    foreach (var item in dAttack)
                    {
                        attacks.Remove(item);
                    }
                    dAttack.Clear();
                }
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, scale, scale),

                new Rectangle(1, 1, sprite.Width, sprite.Height), color);

            if (takenDmg == true)
            {
                spriteBatch.DrawString(enemyTakesDmg, Player.dmg.ToString() + "aids", position + offset, Color.White);
            }
            else
            {
                spriteBatch.DrawString(enemyTakesDmg, "", position + offset, Color.White);
            }

            Rectangle healthRectangle = new Rectangle((int)position.X,
                                         (int)position.Y,
                                         hpBar.Width,
                                         hpBar.Height);

            spriteBatch.Draw(hpBar, healthRectangle, Color.Black);

            healthPercentage = ((float)currentHP / (float)maxHp);

            visibleWidth = (float)hpBar.Width * (float)healthPercentage;

            healthRectangle = new Rectangle((int)position.X,
                                           (int)position.Y,
                                           (int)(visibleWidth),
                                           hpBar.Height);

            spriteBatch.Draw(hpBar, healthRectangle, Color.Red);

            //spriteBatch.Draw(attackSprite, new Rectangle((int)position.X, (int)position.Y, scale, scale),
            //  new Rectangle(1, 1, sprite.Width, sprite.Height), color);

            foreach (var item in attacks)
            {
                item.Draw(spriteBatch);
            }
        }

        public List<AttackBox> GetList()
        {
            return attacks;
        }
        public void DestroyItem(AttackBox item)
        {
            dAttack.Add(item);
        }
    }
}