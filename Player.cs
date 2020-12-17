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
        private Texture2D staminaBar;
        private Texture2D hpBar;
        private Texture2D[] animation;
        private Texture2D[] sprites2;
        private Vector2 velocity = new Vector2(0, 0);
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
        private float maxHp;
        private float healthPercentage;
        private float visibleWidth;
        public float currentHP;
        private int level = 1;
        public float xp = 0;
        private SpriteFont playerLevel;
        private SoundEffect LevelUpSound;
        private float maxStamina;
        private float staminaPercentage;
        private float visibleStaminaWidth;
        public float currentStamina;

        public Player(Vector2 position) // construktor for player
        {
            this.lastHp = 10;
            currentHP = this.lastHp;
            maxHp = this.lastHp;
            currentStamina = 100;
            maxStamina = 100;
            fps = 7;
            position = this.position;
            attacks = new List<AttackBox>();
            nAttacks = new List<AttackBox>();
            dAttack = new List<AttackBox>();
        }

        public override void LoadContent(ContentManager contentManager) // kald dette i GameWorld for at loade content
        {
            KeyboardState state = Keyboard.GetState();
            sprite = contentManager.Load<Texture2D>("CoolJimmy");
            spriteIdle = contentManager.Load<Texture2D>("CoolJimmy");
            spriteJump = contentManager.Load<Texture2D>("JimmyJump");

            collisionTexture = contentManager.Load<Texture2D>("Pixel"); // dette bruges til collisionbox, det er en 1x1 pixel

            attackSprite = contentManager.Load<Texture2D>("AttackEffects"); //attack spriten til når man angriber
            DodgeRight = contentManager.Load<Texture2D>("RollRightSide");
            DodgeLeft = contentManager.Load<Texture2D>("RollLeftSide");

            sprites = new Texture2D[8]; //animations sprites til når man går til højre
            sprites2 = new Texture2D[8];//animations sprites til når man går til venstre
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = contentManager.Load<Texture2D>(i + 1 + "Walk");
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites2[i] = contentManager.Load<Texture2D>(i + 1 + "WalkLeft");
            }

           
            hpBar = contentManager.Load<Texture2D>("HpBar");
            staminaBar = contentManager.Load<Texture2D>("HpBar");
            playerLevel = contentManager.Load<SpriteFont>("Score");
            attackSound = contentManager.Load<SoundEffect>("PlayerAttack");
            LevelUpSound = contentManager.Load<SoundEffect>("LevelUp");
            hitEffect = contentManager.Load<SoundEffect>("PlayerGotHit");
        }

        private void HandleInput(GameTime gametime) //denne metode er til indput
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) && isGrounded == true && isJumping == false && buttonPress == false)
            {
                sprite = spriteJump;
                isJumping = true; 
                isGrounded = false; //den skal sættes til false for at sikre os at spillere ikke kan hoppe igen. collision checker ikke selv når den forlader jorden, skal gøres manuelt
                buttonPress = true; 
                gravity.Y = 0; //resetter gravity, ellers vil spilleren falde meget hurtigt ned igen
            }
            if (state.IsKeyUp(Keys.Up) && isGrounded == false && jumpTimer >= 0.2f)
            {
                isJumping = false; //spilleren falder hvis de ikke holde piltasten nede
            }
            if (state.IsKeyUp(Keys.Up))
            {
                if (isDodging == false)
                {
                    sprite = spriteIdle; //sæt idle sprites, men kune hvis man ikke dodger,
                }
                buttonPress = false;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 10;
                if (isDodging == false)
                {
                    Animation(gametime, sprites); //sprites for at gø til højre skal kun køre hvis man ikke er igang med at dodge
                }
                if (state.IsKeyDown(Keys.Up) && isGrounded == false)
                {
                    sprite = spriteJump;
                }
                direction = 2; // dette er hvilken retning spilleren går imod, bruges til at bestemme hvor spilleren angriber
            }
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 10;
                if (isDodging == false)
                {
                    Animation(gametime, sprites2);
                }
                if (state.IsKeyDown(Keys.Up) && isGrounded == false)
                {
                    sprite = spriteJump;
                }

                direction = 1; //samme kode som højre piltast, bare omvendt, direction er 1 istedet for 2
            }

            if (state.IsKeyDown(Keys.D) && canAttack == true && noHoldDown == true)
            {
                attackSound.Play();
                if (direction == 2) // hvis retnigen er højre skal den spawne en attack box med disse kordinator
                {
                    attacks.Add(new AttackBox(attackSprite, new Vector2(Collision.Right - 100, Collision.Y), 200, 1, dmg));
                }
                else if (direction == 1) // hvis venstre så disse
                {
                    attacks.Add(new AttackBox(attackSprite, new Vector2(Collision.Left - 100, Collision.Y), 200, 1, dmg));
                }
                canAttack = false;
                noHoldDown = false; //sådan at spille ikke kan holde knappen nede
                deleteTimer = 0;
                deleteWhen = true; //dette er til at slette attack boxen
                attackTimer = 0; //hvis spilleren allerede har slået før, kan det være at timeren ikke er 0, så vi resetter her, for at være sikker
            }
            if (state.IsKeyDown(Keys.E) && isDodging == false && noHoldDown == true && currentStamina >= 30)
            {
                currentStamina -= 30; //hver gang mang dodger minusser man stamina med 30, derfor kræver det også 30 and dodge
                noHoldDown = false; //sådan at man ikke kan holde knappen nede, men også for at den ikke minuser stamin flere gange
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

        public void Dodge() //dette er dodge metoden
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

        public override void OnCollision(GameObject other) //dette køre hvis men kolidere med andre objekter, det andet objekt bliver other
        {
            if (other is AttackBox && AttackBox.ID == 2 && timer > cooldownTime) //hvis det andet objekt er en enemy, tag skade og bliv kort rød
            {
                if (isDodging == false)
                {
                    TakeDamage(other.pos.X);
                    this.color = Color.Red;
                    currentHP -= Enemy.dmg;
                }
                timer = 0; //denne timer er til for at skifte spilleren tilbage til normal farve
            }
            if (other is AttackBox && AttackBox.ID == 3 && timer > cooldownTime) //hvis det andet objekt er en boss, gør det samme som enemy med anderledes skade
            {
                if (isDodging == false)
                {
                    TakeDamage(other.pos.X);
                    this.color = Color.Red;
                    currentHP -= Boss.dmg;
                }
                timer = 0;
            }
            if (other is Enemy && timer > cooldownTime) //hvis spilleren løver ind i en enemy
            {
                //Health(1);
                //timer = 0;
            }
            else if (other is Enviroment) // dette er hvis spiller rammer en platform
            {
                isGrounded = true; // for at kunne hoppe igen
                isJumping = false;
                walkOff = false; //den bliver sat til true hvis man forlader en platform
                //alle de forskellige if statemens her nedenunder er de forskellige sider på en platform
                //der er ikke noget kode i den første if statement fordi den ikke virker
                //kan ikke checke siderne af Collision af en eller anden grund
                if (position.X > other.Collision.Left && position.X < other.Collision.Right && position.Y > other.Collision.Top)
                {
                }
                else if (position.X + (sprite.Width / 3) <= other.pos.X && position.Y + (sprite.Height / 2) >= other.pos.Y) //venstre
                {
                    this.position.X = other.Collision.Left - sprite.Width;
                }
                else if (position.X + (sprite.Width / 2) >= other.pos.X && other.pos.Y - (sprite.Height / 2) < position.Y) //højre side
                {
                    this.position.X = other.Collision.Right;
                }
                else if (other.pos.Y >= position.Y) //toppen af platformen
                {
                    position.Y = other.Collision.Top - Collision.Height; //hvis vi ikke - med height, kommer spilleren til at være midt inde i platformen
                }
            }
        }

        public void DrawCollisionBox(GameObject go) //dette til debug, den viser collisionboxen
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

        public override void Update(GameTime gametime) //update, kald dete i gameworld
        {
            if (currentStamina <= 100) //dette er stamina regen, max stamina er 100
            {
                currentStamina += 0.1f;
            }
            if (xp >= 15) //hvis man har 15xp skal spilleren level up, hvis vi havde mere tid vil hvor mege xp man skal bruge stige. f.eks : (xp >= (15*level), men det vill ikke virke særligt godt med det nuværende spil
            {
                LevelUpSound.Play(); 
                level += 1;
                dmg *= level; //vi increaser hvor meget skade spilleren gør
                xp = 0; //sådan at spilleren kan lvl op igen
            }
            if (timer < cooldownTime + 1) //dette er sådan spilleren skifter tilbage til normal farve efter at have taget skade
            {
                this.color = Color.White;
                timer += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (isDodging == true) //dete køre når man trykker på dodge knappen
            {
                if (direction == 1) //hvis spillerens retning er venstre
                {
                    sprite = DodgeLeft;
                    position.X -= 10;
                }
                else if (direction == 2) //hvis den er højre
                {
                    sprite = DodgeRight;
                    position.X += 10;
                }
                dodgeTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (dodgeTimer >= 0.3) //dette er hvor lang tid spilleren dodger i
                {
                    dodgeTimer = 0;
                    isDodging = false;
                }
            }
            HandleInput(gametime); //sådan at handle input kører hele tiden i update
            attackTimer += (float)gametime.ElapsedGameTime.TotalSeconds; // timeren der bruges til at spillere ikke kan angribe konstant
            if (deleteWhen == true) // dette til til at attack boxen ødelægger sig selv
            {
                deleteTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (deleteTimer >= 0.1f) //how fast should the attack destroy itself
            {
                foreach (var item in attacks)
                {
                    DestroyItem(item); //dette vil kallde en metode som putter alle attacks i listen til destruktion
                }
                deleteWhen = false;
            }
            if (isJumping == true) //hvis spilleren har trykket på piltasten op kører dette
            {
                this.position.Y -= 9;
                jumpTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (isJumping == false)
            {
                jumpTimer = 0;
            }
            if (isGrounded == false && isJumping == false || exitCollision == true) //dette er gravity
            {
                if (gravity.Y <= 30)
                {
                    gravity.Y += 0.15f; //prøver at emulere gravity ved at spilleren falder hurtiger og hurtigere
                }
                this.position.Y += gravity.Y;
            }
            else if (isGrounded == true) //for at resette gravity, sådan at spilleren ikke falder super hurtigt med det samme de prøver at hoppe
            {
                gravity.Y = 0;
            }

            if (attackTimer >= 1)// sådan at spilleren ikke kan angribe konstant
            {
                canAttack = true;
            }

            //Attacks.AddRange(nAttacks);
            if (dAttack.Count > 0) //hvis der er nogle attacks i køen til at blive ødelagt, så gør det
            {
                foreach (var item in dAttack)
                {
                    attacks.Remove(item);
                }
                dAttack.Clear();
            }
            if (jumpTimer > 1f) //sådan at spilleren ikke kan hoppe hele tiden
            {
                isJumping = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Der tegnet bossen hvis der har over 1 liv.
            if (currentHP > 1)
            {
                base.Draw(spriteBatch);
                color = Color.White;
                //Der tegnes en healthbar's baggrund.
                Rectangle healthRectangle = new Rectangle((int)position.X - 900,
                                            (int)position.Y - 450,
                                            Collision.Width * 2,
                                            hpBar.Height / 2);

                spriteBatch.Draw(hpBar, healthRectangle, Color.Black);
                healthPercentage = ((float)currentHP / (float)maxHp);

                visibleWidth = (float)(Collision.Width * 2) * (float)healthPercentage;
                //Der tegnes en healthbar's forgrund.
                healthRectangle = new Rectangle((int)position.X - 900,
                                               (int)position.Y - 450,
                                               (int)(visibleWidth / 2) * 2,
                                               hpBar.Height / 2);

                spriteBatch.Draw(hpBar, healthRectangle, Color.Red);
                //Der tegnes tekst
                spriteBatch.DrawString(playerLevel, "Level: " + level, new Vector2(healthRectangle.X + (healthRectangle.Width / 2), healthRectangle.Y - healthRectangle.Height), Color.Wheat);
                //Der tegnes en healthbar's baggrund.
                Rectangle staminaRectangle = new Rectangle((int)position.X - 900,
                                           (int)position.Y - 350,
                                           (int)(Collision.Width * 1.5f),
                                           staminaBar.Height / 2);

                spriteBatch.Draw(staminaBar, staminaRectangle, Color.Black);

                staminaPercentage = ((float)currentStamina / (float)maxStamina);

                visibleStaminaWidth = (float)(Collision.Width * 2) * (float)staminaPercentage;
                //Der tegnes en healthbar's forgrund.
                staminaRectangle = new Rectangle((int)position.X - 900,
                                               (int)position.Y - 350,
                                               (int)(visibleStaminaWidth / 2.5f) * 2,
                                               staminaBar.Height / 2);

                spriteBatch.Draw(staminaBar, staminaRectangle, Color.DarkGreen);
            }
        }

        public void DestroyItem(AttackBox item) // dette er metoden til at fjerne attaxk boxes
        {
            dAttack.Add(item);
        }

        private void TakeDamage(float otherPosX) //dette er et lille feedback metode, som kaldes hvis spilleren tager skade
        {
            bool Right = true;
            hitEffect.Play();
            if (otherPosX > this.pos.X) //spilleren skal rykke sig lidt, alt efter hvilken side de blev ramt 
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

        public void gainXP(int howMuch) //dette er metoden for at få xp, overloaden er hvor meget xå spilleren får
        {
            xp += howMuch;
        }
    }
}