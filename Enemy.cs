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
        public int hp;
        private Vector2 offset;
        private int speed;
        private int attackTimer;
        private int dmg = 2;
        private SpriteFont enemyKilled;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D enemyJimSprite;
        private Rectangle rectangle;
        private int scale;
        private bool enemyRotate;
        private GameWorld GameWorld;
        private AttackBox attackBox;
        private int detectPlayerInRangePlus = 200;
        private int detectPlayerInRangeMinus = -200;
        private List<GameObject> gameObjectList;
        private bool canAttack;
        private bool noHoldDown;
        private bool deleteWhen;
        private float deleteTimer;
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
        private Player mainPlayer;
        private bool Left; //left is true, right is false

        // Giver en position og scalering af enemy
        public Enemy(Vector2 position, int scale, int hp)
        {
            this.position = position;
            this.scale = scale;
            this.hp = 100;
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

            sprites = new Texture2D[1];

            sprites[0] = contentManager.Load<Texture2D>("AttackEffects");

            GameWorld = new GameWorld();

            hitEffect = contentManager.Load<SoundEffect>("PlayerGotHit");

            HammerAttack = new Texture2D[5];
            for (int i = 0; i < HammerAttack.Length; i++)
            {
                HammerAttack[i] = contentManager.Load<Texture2D>(i  + "HammerAttackJim");
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player)
            {
                color = Color.Pink;
            }
            if (other is AttackBox)
            {
                hitEffect.Play();
                hp -= Player.dmg;
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
           

            enemyAndPlayerDistance = position.X - playerPositionX;

            if (enemyAndPlayerDistance <= 500 && enemyAndPlayerDistance >= 0 || enemyAndPlayerDistance > -500 && enemyAndPlayerDistance <= 0)
            {

                if (enemyAndPlayerDistance <= 500 && enemyAndPlayerDistance >= 0)
                {
                    Left = true;
                }
                else if (enemyAndPlayerDistance > -500 && enemyAndPlayerDistance <= 0)
                {
                    Left = false;
                }

                AiMovement();
                animationTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.5f && cAnimation <3)
                {
                    cAnimation += 1;
                    animationTimer = 0;
                }
                else if (animationTimer >0.1f && cAnimation >= 3)
                {
                    cAnimation += 1;
                    animationTimer = 0;
                }

                if (cAnimation >= HammerAttack.Length)
                {
                    cAnimation = 0;
                }
                sprite = HammerAttack[cAnimation];

                if (cAnimation == 4)
                {
                    position.X -= 5;
                    if (Left == true)
                    {
                        attacks.Add(new AttackBox(attackSprite, new Vector2(position.X + -200, position.Y + 100), 200, 2, dmg));

                    }
                    else if (Left == false)
                    {
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