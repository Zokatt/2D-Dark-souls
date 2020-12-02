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

        public int hp;
        private int scale;
        private int dmg = 2;
        private bool Attacking;
        private Texture2D attackSprite;
        public static List<AttackBox> attacks;
        private Texture2D collisionTexture;
        private float enemyAndPlayerDistance;
        private float playerPositionX;
        private SoundEffect hitEffect;
        private Texture2D[] HammerAttack;
        private float animationTimer;
        private int cAnimation;
        private Texture2D idle;
        private SpriteFont enemyTakesDmg;
        private float timer;
        private float cooldownTime = 1;
        private int currentHP;
        private Vector2 offset = new Vector2(-100, -100);
        private float takenDmgTimer;
        private bool takenDmg = false;

        private bool Left; //left is true, right is false

        // Giver en position og scalering af enemy
        public Enemy(Vector2 position, int scale, int hp)
        {
            this.position = position;
            this.scale = scale;
            this.hp = 5;
            currentHP = this.hp;
            this.sprite = idle;
            attacks = new List<AttackBox>();
        }

        // Enemy's bevægelseshastighed
        public void AiMovement()
        {
            if (Left == true)
            {
                position.X -= 2;
            }
            if (Left == false)
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
            attackSprite = contentManager.Load<Texture2D>("AttackEffects");
            collisionTexture = contentManager.Load<Texture2D>("Pixel");
            enemyTakesDmg = contentManager.Load<SpriteFont>("Score");
            sprites = new Texture2D[1];

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
                hp -= Player.dmg;
                timer = 0;
            }

            if (hp <= 0)
            {
                GameWorld.Destroy(this);
            }
        }

        public void SetPlayer(int playerX)
        {
            this.playerPositionX = playerX;
        }

        public override void Update(GameTime gametime)
        {
            if (hp != currentHP)
            {
                takenDmgTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                takenDmg = true;
            }

            if (takenDmgTimer >= 1)
            {
                takenDmg = false;
                currentHP = hp;
                takenDmgTimer = 0;
            }

            if (timer < cooldownTime + 1)
                timer += (float)gametime.ElapsedGameTime.TotalSeconds;

            enemyAndPlayerDistance = position.X - playerPositionX;

            if (enemyAndPlayerDistance <= 500 && enemyAndPlayerDistance >= 0 || enemyAndPlayerDistance > -500 && enemyAndPlayerDistance <= 0)
            {
                Attacking = true;
                if (enemyAndPlayerDistance <= 500 && enemyAndPlayerDistance >= 0)
                {
                    Left = true;
                }
                else if (enemyAndPlayerDistance > -500 && enemyAndPlayerDistance <= 0)
                {
                    Left = false;
                }
            }

            if (Attacking == true)
            {
                AiMovement();
                animationTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.4f && cAnimation < 3)
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
                    cAnimation = 0;
                }
                if (cAnimation < HammerAttack.Length)
                {
                    sprite = HammerAttack[cAnimation];
                }

                if (cAnimation == 4)
                {
                    Attacking = false;

                    if (Left == true)
                    {
                        position.X -= 5;
                        attacks.Add(new AttackBox(attackSprite, new Vector2(position.X + -200, position.Y + 100), 200, 2, dmg));
                    }
                    else if (Left == false)
                    {
                        position.X += 5;
                        attacks.Add(new AttackBox(attackSprite, new Vector2(position.X + +200, position.Y + 100), 200, 2, dmg));
                    }
                }
            }
            else
            {
                sprite = idle;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, scale, scale),

                new Rectangle(1, 1, sprite.Width, sprite.Height), color);

            if (takenDmg == true)
            {
                spriteBatch.DrawString(enemyTakesDmg, Player.dmg.ToString(), position + offset, Color.White);
            }
            else if (takenDmg == false)
            {
                spriteBatch.DrawString(enemyTakesDmg, "", position + offset, Color.White);
            }

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
    }
}