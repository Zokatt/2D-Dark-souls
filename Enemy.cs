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
        private float currentHealth;
        private Texture2D enemyHealthBarOutlineGreed;
        private Texture2D enemyHealthBarGreedV2;

        private Player mainPlayer;

        // Giver en position og scalering af enemy
        public Enemy(Vector2 position, int scale, int hp)
        {
            this.position = position;
            this.scale = scale;
            this.hp = hp;
            attacks = new List<AttackBox>();

            currentHealth = 0.4f;
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
            attackSprite = contentManager.Load<Texture2D>("AttackEffects");
            collisionTexture = contentManager.Load<Texture2D>("Pixel");
            enemyHealthBarGreedV2 = contentManager.Load<Texture2D>("EnemyHealthBarGreedV2");
            enemyHealthBarOutlineGreed = contentManager.Load<Texture2D>("EnemyHealthBarOutlineGreed");


            sprites = new Texture2D[1];

            sprites[0] = contentManager.Load<Texture2D>("AttackEffects");

            GameWorld = new GameWorld();

            hitEffect = contentManager.Load<SoundEffect>("PlayerGotHit");
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
            AiMovement();

            enemyAndPlayerDistance = playerPositionX - position.X;

            if (enemyAndPlayerDistance <= 200 && enemyAndPlayerDistance >= -200)
            {
                attacks.Add(new AttackBox(attackSprite, new Vector2(position.X + 50, position.Y), 300, 2, dmg));
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

            spriteBatch.Draw(enemyHealthBarGreedV2, new Rectangle((int)position.X, (int)position.Y - 200, (int)currentHealth, scale),
            new Rectangle((int)position.X, (int)position.Y -200, enemyHealthBarGreedV2.Width, enemyHealthBarGreedV2.Height), color);

            //spriteBatch.Draw(enemyHealthBarGreedV2, new Vector2(position.X, position.Y - 200), Color.White);
            spriteBatch.Draw(enemyHealthBarOutlineGreed, new Vector2(400, -200), new Rectangle((int)position.X, (int)position.Y, enemyHealthBarOutlineGreed.Height, enemyHealthBarGreedV2.Height), Color.White);
        }

        public List<AttackBox> GetList()
        {
            return attacks;
        }
    }
}