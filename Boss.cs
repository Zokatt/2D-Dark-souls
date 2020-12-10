using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private Texture2D attackSprite;
        private SoundEffect hitEffect;
        public bool activator = false; // this activates the boss
        private float hp;
        private float enemyAndPlayerDistance;
        private float playerPositionX;
        private float playerPositionY;
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
        private int phase = 1;
        public static List<AttackBox> attacks;
        private List<AttackBox> dAttacks;
        private float deleteTimer;
        private bool deleteWhen;
        private float dmgTimer;
        private bool tknDamage;
        private int onlyTakeDamageOnce;
        public static int dmg = 100;
        private SpriteFont enemyName;

        public float lastHP;
        private Texture2D hpBar;
        private float currentHP;
        private float maxHp;
        private float healthPercentage;
        private float visibleWidth;

        public Boss(Vector2 position, int hp)
        {
            this.position = position;
            this.hp = hp;
            attacks = new List<AttackBox>();
            dAttacks = new List<AttackBox>();
            this.lastHP = 1000;
            currentHP = this.lastHP;
            maxHp = this.lastHP;
            dmg = 2;
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
                sprites2[i] = contentManager.Load<Texture2D>("Sin1GreedRSideAnimation" + (i + 1));
            }
            enemyName = contentManager.Load<SpriteFont>("Score");
            hpBar = contentManager.Load<Texture2D>("HpBar");
            idle = contentManager.Load<Texture2D>("Sin1Greed");
            LSideRun = contentManager.Load<Texture2D>("Sin1GreedLside");
            RSideRun = contentManager.Load<Texture2D>("Sin1GreedRSide");
            tiredLeft = contentManager.Load<Texture2D>("GreedLeftTired");
            tiredRigh = contentManager.Load<Texture2D>("GreedRightTired");
            attackSprite = contentManager.Load<Texture2D>("BossMainAttackBox");
            hitEffect = contentManager.Load<SoundEffect>("PlayerGotHit");

            sprite = idle;
        }

        public override Rectangle Collision
        {
            get
            {
                return new Rectangle(
                    (int)position.X, (int)position.Y, (int)(sprite.Height * scale), (int)(sprite.Width * scale));
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player && MainAttackTimer >= 2 && phase == 2)
            {
                phase = 3;
            }
            if (other is AttackBox)
            {
                this.color = Color.Red;
            }
            if (other is AttackBox && AttackBox.ID == 1 && onlyTakeDamageOnce == 0)
            {
                this.color = Color.Red;
                hitEffect.Play();
                tknDamage = true;
                dmgTimer = 0;
                onlyTakeDamageOnce = 1;
                currentHP -= Player.dmg;
            }
        }

        public void BossAI(GameTime gameTime)
        {
            if (left == true)
            {
                if (phase == 2)
                {
                    this.position.X -= 10;
                }
                if (phase == 3)
                {
                    this.position.X -= 5;
                }
            }
            else if (left == false)
            {
                if (phase == 2)
                {
                    this.position.X += 10;
                }
                if (phase == 3)
                {
                    this.position.X += 5;
                }
            }
        }

        public void SetPlayer(int playerX, int playerY)
        {
            this.playerPositionX = playerX;
            this.playerPositionY = playerY;
        }

        public override void Update(GameTime gametime)
        {
            if (activator == true)
            {
                enemyAndPlayerDistance = position.X - playerPositionX;
                if (tknDamage == true)
                {
                    dmgTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                    if (dmgTimer >= 0.3f)
                    {
                        if (healthPercentage >=0.3f)
                        {
                            color = Color.White;
                        }
                        else if (healthPercentage <=0.3f)
                        {
                            color = Color.Gold;
                        }
                        tknDamage = false;
                        onlyTakeDamageOnce = 0;
                    }
                }
                if (phase == 4)
                {
                    tiredTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                    if (left == true)
                    {
                        sprite = tiredLeft;
                    }
                    if (left == false)
                    {
                        sprite = tiredRigh;
                    }
                    if (healthPercentage >=0.3f)
                    {
                        if (tiredTimer >= 2)
                        {
                            phase = 1;
                            aAnimation = 0;
                        }
                    }
                    else if (healthPercentage <=0.3f)
                    {
                        if (tiredTimer >= 1)
                        {
                            phase = 1;
                            aAnimation = 0;
                        }
                    }
                }
                if (phase == 1)
                {
                    tiredTimer = 0;
                    if (healthPercentage >= 0.3f)
                    {
                        MainAttackTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                    }
                    else if (healthPercentage <=0.3f)
                    {
                        MainAttackTimer += (float)gametime.ElapsedGameTime.TotalSeconds * 2;
                    }

                    sprite = idle;
                }
                if (MainAttackTimer >= 2 && phase != 3)
                {
                    phase = 2;
                    if (enemyAndPlayerDistance >= 1 - (sprite.Width / 2) && phase == 2)
                    {
                        sprite = sprites[0];
                        left = true;
                        BossAI(gametime);
                    }
                    else if (enemyAndPlayerDistance <= 0 - (sprite.Width / 2) && phase == 2)
                    {
                        sprite = sprites2[0];
                        left = false;
                        BossAI(gametime);
                    }
                }
                if (MainAttackTimer >= 2 && phase == 2)
                {
                    BossAI(gametime);
                }
                if (phase == 3)
                {
                    BossAI(gametime);
                    Animations(gametime);
                }
                if (deleteWhen == true)
                {
                    deleteTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                }
                if (deleteTimer >= 0.5f) //how fast should the attack destroy itself
                {
                    foreach (var item in attacks)
                    {
                        DestroyItem(item);
                    }
                    deleteWhen = false;
                }
                // need to delete attacks
                if (dAttacks != null)
                {
                    if (dAttacks.Count > 0)
                    {
                        foreach (var item in dAttacks)
                        {
                            attacks.Remove(item);
                        }
                        dAttacks.Clear();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentHP > 1)
            {
                base.Draw(spriteBatch);
                if (activator)
                {
                    Rectangle healthRectangle = new Rectangle((int)playerPositionX - 450,
                                     (int)playerPositionY + 450,
                                     Collision.Width,
                                     hpBar.Height / 2);

                    spriteBatch.Draw(hpBar, healthRectangle, Color.Black);
                    spriteBatch.DrawString(enemyName, "Greed", new Vector2(healthRectangle.X + (healthRectangle.Width / 2), healthRectangle.Y - healthRectangle.Height), Color.Wheat);

                    healthPercentage = ((float)currentHP / (float)maxHp);

                    visibleWidth = (float)(Collision.Width * 2) * (float)healthPercentage;

                    healthRectangle = new Rectangle((int)playerPositionX - 450,
                                                   (int)playerPositionY + 450,
                                                   (int)(visibleWidth / 2),
                                                   hpBar.Height / 2);

                    spriteBatch.Draw(hpBar, healthRectangle, Color.Red);
                }
            }
        }

        private void Animations(GameTime gametime)
        {
            if (phase == 3)
            {
                animationTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.2f && aAnimation <= 1)
                {
                    aAnimation += 1;
                    animationTimer = 0;
                }
            }
            if (aAnimation == 2 && phase == 3)
            {
                deleteTimer = 0;
                deleteWhen = true;
                if (left == true && attacks.Count <= 1)
                {
                    position.X -= 15;
                    attacks.Add(new AttackBox(attackSprite, new Vector2(position.X - 400, position.Y + 750), 800, 3, dmg));
                }
                else if (left == false && attacks.Count <= 1)
                {
                    position.X += 15;
                    attacks.Add(new AttackBox(attackSprite, new Vector2(position.X + 500, position.Y + 750), 800, 3, dmg));
                }

                smashTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (smashTimer >= 0.4f)
                {
                    aAnimation += 1;
                    animationTimer = 0;
                    smashTimer = 0;
                }
            }
            if (aAnimation >= sprites.Length || aAnimation >= sprites2.Length)
            {
                phase = 4;
                MainAttackTimer = 0;
            }
            if (aAnimation < sprites.Length && phase == 3 || aAnimation < sprites2.Length && phase == 3)
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

        public List<AttackBox> GetList()
        {
            return attacks;
        }

        public void DestroyItem(AttackBox item)
        {
            dAttacks.Add(item);
        }
    }
}