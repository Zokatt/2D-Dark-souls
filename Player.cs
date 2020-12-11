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
    public class Player : GameObject
    {
        public int lastHp;
        private SoundEffect hitEffect;
        private Texture2D collisionTexture;
        private Texture2D attackSprite;
        private Texture2D spriteIdle;
        private Texture2D spriteIdleLeft;
        private Texture2D spriteJump;
        private Texture2D DodgeRight;
        private Texture2D DodgeLeft;
        private Vector2 gravity = new Vector2(0, 0);
        private Texture2D[] sprites2;
        public bool isDodging;
        public bool isGrounded = false;
        private bool buttonPress = false;
        private bool isJumping = false;
        private float dodgeTimer;
        private float jumpTimer;
        public static int dmg = 10;
        public List<AttackBox> attacks;
        public List<AttackBox> nAttacks;
        public List<AttackBox> dAttack;
        private bool canAttack;
        private float attackTimer;
        private bool noHoldDown;
        private bool deleteWhen;
        public SoundEffect attackSound;
        private float deleteTimer;
        private float timer = 0.0f;
        private float cooldownTime = 2;
        private bool walkOff;
        private int direction = 2;
        private Texture2D hpBar;
        private float maxHp;
        private float healthPercentage;
        private float visibleWidth;
        public float currentHP;
        private int level = 1;
        public float xp = 0;
        private SpriteFont playerLevel;
        private SoundEffect LevelUpSound;
        private Texture2D staminaBar;
        private float maxStamina;
        private float staminaPercentage;
        private float visibleStaminaWidth;
        public float currentStamina;


        public Player(Vector2 position)
        {
            this.lastHp = 10;
            currentHP = this.lastHp;
            maxHp = this.lastHp;
            currentStamina = 100;
            maxStamina = 100;
            fps = 5;
            position = this.position;
            attacks = new List<AttackBox>();
            nAttacks = new List<AttackBox>();
            dAttack = new List<AttackBox>();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            KeyboardState state = Keyboard.GetState();
            sprite = contentManager.Load<Texture2D>("CoolJimmy");
            collisionTexture = contentManager.Load<Texture2D>("Pixel");
            attackSprite = contentManager.Load<Texture2D>("AttackEffects");
            hitEffect = contentManager.Load<SoundEffect>("PlayerGotHit");
            DodgeRight = contentManager.Load<Texture2D>("RollRightSide");
            DodgeLeft = contentManager.Load<Texture2D>("RollLeftSide");
            LevelUpSound = contentManager.Load<SoundEffect>("LevelUp");
            sprites = new Texture2D[3];
            sprites2 = new Texture2D[3];
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = contentManager.Load<Texture2D>(i + 1 + "JimmyMoveLeft");
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites2[i] = contentManager.Load<Texture2D>(i + 1 + "JimmyMoveRight");
            }
            spriteJump = contentManager.Load<Texture2D>("JimmyJump");
            spriteIdle = contentManager.Load<Texture2D>("CoolJimmy");
            spriteIdleLeft = contentManager.Load<Texture2D>("CoolJimmyLeft");
            hpBar = contentManager.Load<Texture2D>("HpBar");
            staminaBar = contentManager.Load<Texture2D>("HpBar");
            attackSound = contentManager.Load<SoundEffect>("PlayerAttack");
            playerLevel = contentManager.Load<SpriteFont>("Score");
        }

        private void HandleInput(GameTime gametime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) && isGrounded == true && isJumping == false && buttonPress == false)
            {
                sprite = spriteJump;
                isJumping = true;
                isGrounded = false;
                buttonPress = true;
                gravity.Y = 0;

            }
            if (state.IsKeyUp(Keys.Up) && isGrounded == false && jumpTimer >= 0.2f)
            {
                isJumping = false;
            }
            if (state.IsKeyUp(Keys.Up))
            {
                if (isDodging == false)
                {
                    sprite = spriteIdle;
                }
                buttonPress = false;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 10;
                if (isDodging == false)
                {
                    Animation(gametime, sprites2);
                }
                if (state.IsKeyDown(Keys.Up) && isGrounded == false)
                {
                    sprite = spriteJump;
                }
                direction = 2;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 10;
                if (isDodging == false)
                {
                    Animation(gametime, sprites);
                    sprite = spriteIdleLeft;
                }
                if (state.IsKeyDown(Keys.Up) && isGrounded == false)
                {
                    sprite = spriteJump;
                }

                direction = 1;
            }

            if (state.IsKeyDown(Keys.D) && canAttack == true && noHoldDown == true)
            {
                attackSound.Play();
                if (direction == 2)
                {
                    attacks.Add(new AttackBox(attackSprite, new Vector2(Collision.Right - 100, Collision.Y), 200, 1, dmg));
                }
                else if (direction == 1)
                {
                    attacks.Add(new AttackBox(attackSprite, new Vector2(Collision.Left - 100, Collision.Y), 200, 1, dmg));
                }
                canAttack = false;
                noHoldDown = false;
                deleteTimer = 0;
                deleteWhen = true;
                attackTimer = 0;
            }
            if (state.IsKeyDown(Keys.E) && isDodging == false && noHoldDown == true && currentStamina >=30)
            {
                currentStamina -= 20;
                noHoldDown = false;
                isDodging = true;
            }
            else if (state.IsKeyUp(Keys.E))
            {
                noHoldDown = true;
            }
        }

        public void PlayerAttack()
        {
            //Attack!!
        }

        public void Dodge()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.F) && dodgeTimer == 2 && buttonPress == false)
            {
                isDodging = true;
                dodgeTimer = 0;
                buttonPress = true;
            }
            if (state.IsKeyUp(Keys.F)) //you can only press dodge again if you let go of the button
            {
                buttonPress = false;
            }
        }

        public override void OnCollision(GameObject other)
        {
            if (other is AttackBox && AttackBox.ID == 2 && timer > cooldownTime)
            {
                if (isDodging == false)
                {
                    TakeDamage(other.pos.X);
                    this.color = Color.Red;
                    currentHP -= Enemy.dmg;
                }
                timer = 0;
            }
            if (other is AttackBox && AttackBox.ID == 3 && timer > cooldownTime)
            {
                if (isDodging == false)
                {
                    TakeDamage(other.pos.X);
                    this.color = Color.Red;
                    currentHP -= Boss.dmg;
                }
                timer = 0;
            }
            if (other is Enemy && timer > cooldownTime)
            {
                //Health(1);
                //timer = 0;
            }
            else if (other is Enviroment)
            {
                isGrounded = true;
                isJumping = false;
                walkOff = false;
                if (position.X > other.Collision.Left && position.X < other.Collision.Right && position.Y > other.Collision.Top)
                {
                }
                else if (position.X + (sprite.Width / 3) <= other.pos.X && position.Y + (sprite.Height / 2) >= other.pos.Y)
                {
                    this.position.X = other.Collision.Left - sprite.Width;
                }
                else if (position.X + (sprite.Width / 2) >= other.pos.X && other.pos.Y - (sprite.Height / 2) < position.Y)
                {
                    this.position.X = other.Collision.Right;
                }
                else if (other.pos.Y >= position.Y)
                {
                    position.Y = other.Collision.Top - Collision.Height;
                }
            }
        }

        public void DrawCollisionBox(GameObject go)
        {
            //Der laves en streg med tykkelsen 1 for hver side af Collision.
            Rectangle topLine = new Rectangle(go.Collision.X, go.Collision.Y, go.Collision.Width, 1);
            Rectangle bottomLine = new Rectangle(go.Collision.X, go.Collision.Y + go.Collision.Height, go.Collision.Width, 1);
            Rectangle rightLine = new Rectangle(go.Collision.X + go.Collision.Width, go.Collision.Y, 1, go.Collision.Height);
            Rectangle leftLine = new Rectangle(go.Collision.X, go.Collision.Y, 1, go.Collision.Height);
            //Der tegnes en streg med tykkelsen 1 for hver side af Collision med collsionTexture med farven rød.
            _spriteBatch.Draw(collisionTexture, topLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, bottomLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, rightLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, leftLine, Color.Red);
        }

        public override void Update(GameTime gametime)
        {
            if (currentStamina<=100)
            {
                currentStamina += 0.1f;
            }
            if (xp >= 15)
            {
                LevelUpSound.Play();
                level += 1;
                dmg *= level;
                xp = 0;
            }
            if (timer < cooldownTime + 1)
            {
                this.color = Color.White;
                timer += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (isDodging == true)
            {
                if (direction == 1)
                {
                    sprite = DodgeLeft;
                    position.X -= 10;
                }
                else if (direction == 2)
                {
                    sprite = DodgeRight;
                    position.X += 10;
                }
                dodgeTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (dodgeTimer >= 0.3)
                {
                    dodgeTimer = 0;
                    isDodging = false;
                }
            }
            HandleInput(gametime);
            attackTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
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
            if (isJumping == true)
            {
                this.position.Y -= 9;
                jumpTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (isJumping == false)
            {
                jumpTimer = 0;
            }
            if (isGrounded == false && isJumping == false || exitCollision == true)
            {
                if (gravity.Y <= 30)
                {
                    gravity.Y += 0.15f;
                }
                this.position.Y += gravity.Y;
            }
            else if (isGrounded == true)
            {
                gravity.Y = 0;
            }

            if (attackTimer >= 1)
            {
                canAttack = true;
            }

            //Attacks.AddRange(nAttacks);
            if (dAttack.Count > 0)
            {
                foreach (var item in dAttack)
                {
                    attacks.Remove(item);
                }
                dAttack.Clear();
            }
            if (jumpTimer > 1f)
            {
                isJumping = false;
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (currentHP > 1)
            {
                base.Draw(spriteBatch);
                color = Color.White;

                Rectangle healthRectangle = new Rectangle((int)position.X - 900,
                                            (int)position.Y - 450,
                                            Collision.Width * 2,
                                            hpBar.Height / 2);

                spriteBatch.Draw(hpBar, healthRectangle, Color.Black);

                healthPercentage = ((float)currentHP / (float)maxHp);

                visibleWidth = (float)(Collision.Width * 2) * (float)healthPercentage;

                healthRectangle = new Rectangle((int)position.X - 900,
                                               (int)position.Y - 450,
                                               (int)(visibleWidth / 2) * 2,
                                               hpBar.Height / 2);

                spriteBatch.Draw(hpBar, healthRectangle, Color.Red);
                spriteBatch.DrawString(playerLevel, "Level: " + level, new Vector2(healthRectangle.X + (healthRectangle.Width / 2), healthRectangle.Y - healthRectangle.Height), Color.Wheat);

                Rectangle staminaRectangle = new Rectangle((int)position.X - 900,
                                           (int)position.Y - 350,
                                           (int)(Collision.Width * 1.5f),
                                           staminaBar.Height / 2);

                spriteBatch.Draw(staminaBar, staminaRectangle, Color.Black);

                staminaPercentage = ((float)currentStamina / (float)maxStamina);

                visibleStaminaWidth = (float)(Collision.Width * 2) * (float)staminaPercentage;

                staminaRectangle = new Rectangle((int)position.X - 900,
                                               (int)position.Y - 350,
                                               (int)(visibleStaminaWidth / 2.5f) * 2,
                                               staminaBar.Height / 2);

                spriteBatch.Draw(staminaBar, staminaRectangle, Color.DarkGreen);
            }

        }

        public void DestroyItem(AttackBox item)
        {
            dAttack.Add(item);
        }

        private void TakeDamage(float otherPosX)
        {
            bool Right = true;
            hitEffect.Play();
            if (otherPosX > this.pos.X)
            {
                Right = true;
            }
            if (otherPosX < this.pos.X)
            {
                Right = false;
            }
            if (Right == true)
            {
                this.position.X -= 40;
            }
            else if (Right == false)
            {
                this.position.X += 40;
            }
        }
        public void gainXP(int howMuch)
        {
            xp += howMuch;
        }
    }
}