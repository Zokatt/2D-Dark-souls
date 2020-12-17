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
        private float scale = 1;

        public float lastHP;
        private Texture2D hpBar;
        private float currentHP;
        private float maxHp;
        private float healthPercentage;
        private float visibleWidth;

        //Construkter for boss
        public Boss(Vector2 position, int hp)
        {
            this.position = position;
            this.hp = hp;
            attacks = new List<AttackBox>();
            dAttacks = new List<AttackBox>();
            this.lastHP = 1500;
            currentHP = this.lastHP;
            maxHp = this.lastHP;
            dmg = 3;
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

        //Collision boks bliver overskrevet
        public override Rectangle Collision
        {
            get
            {
                return new Rectangle(
                    (int)position.X, (int)position.Y, (int)(sprite.Height * scale), (int)(sprite.Width * scale));
            }
        }

        //Funktionen OnCollision bliver overskrevet.
        //Funktionen bliver brugt når der sker en kollision med en anden gameobject.
        public override void OnCollision(GameObject other)
        {
            if (other is Player && MainAttackTimer >= 2 && phase == 2) //hvis bossen er igen med at charge spilleren så skift til at angribe
            {
                phase = 3;
            }
            if (other is AttackBox) //dette blev brugt til debugging
            {
                this.color = Color.Red;
            }
            if (other is AttackBox && AttackBox.ID == 1 && onlyTakeDamageOnce == 0) //kun tage skade hvis attack boxen er fra spilleren(ID==1),sådan at den ikke skader sig selv
            {
                this.color = Color.Red;
                hitEffect.Play();
                tknDamage = true; //dette til at sæt farven tilbage til normal
                dmgTimer = 0;
                onlyTakeDamageOnce = 1; //sådan at bossen ikke bliver ramt flere gange af spilleren, eftersom at attack boxen ikke bliver fjernes med det samme
                currentHP -= Player.dmg;
            }
        }

        public void BossAI(GameTime gameTime) //Boss movement
        {
            if (left == true)//hvis den skal til venstre
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

        public void SetPlayer(int playerX, int playerY)//dette er for at få fat på spillerens positioner, kald den i gameworld
        {
            this.playerPositionX = playerX;
            this.playerPositionY = playerY;
        }

        public override void Update(GameTime gametime)//update for boss
        {
            if (activator == true) //bossen skal kun aktivares hvis spilleren er tæt nok på, sæt den til true i gameworld
            {
                enemyAndPlayerDistance = position.X - playerPositionX; //dette er hvor langt spilleren er væk fra bossem
                if (tknDamage == true)//dette bruges til at skifte farven tilbage til normal når bossen har taget damage
                {
                    dmgTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                    if (dmgTimer >= 0.3f)
                    {
                        if (healthPercentage >= 0.3f)
                        {
                            color = Color.White;
                        }
                        else if (healthPercentage <= 0.3f)
                        {
                            color = Color.Gold;
                        }
                        tknDamage = false;
                        onlyTakeDamageOnce = 0;
                    }
                }
                if (phase == 4) //dette er efer bossen her slået, så den er bliver, sådan at spilleren har mulighed forat slå bossen
                {
                    tiredTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                    if (left == true) //den skal vælge sprite alt efer hvilken retning bossen løb i
                    {
                        sprite = tiredLeft;
                    }
                    if (left == false)
                    {
                        sprite = tiredRigh;
                    }
                    if (healthPercentage >= 0.3f) 
                    {
                        if (tiredTimer >= 2)
                        {
                            phase = 1; //gå tilbage til phase 1
                            aAnimation = 0; //reset attack Animations
                        }
                    }
                    else if (healthPercentage <= 0.3f) //hvis bossen kommer under 30% liv, så bliver den mere aggresiv, altså mindre tiredTimer
                    {
                        if (tiredTimer >= 1)
                        {
                            phase = 1;
                            aAnimation = 0;
                        }
                    }
                }
                if (phase == 1) //dette er phase 1,  her står han bare stille
                {
                    tiredTimer = 0; //resetter tired timeren sådan at næste gang han går ilbage til phase 4 så er timeren på 0 far starten af igen
                    if (healthPercentage >= 0.3f)
                    {
                        MainAttackTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                    }
                    else if (healthPercentage <= 0.3f) //hvis bossen er under 30% liv bliver den mere aggresiv, så mindre idle time
                    {
                        MainAttackTimer += (float)gametime.ElapsedGameTime.TotalSeconds * 2;
                    }

                    sprite = idle;
                }
                if (MainAttackTimer >= 2 && phase != 3)//dette er phasen hvor bossen går efter spilleren
                {
                    phase = 2;
                    if (enemyAndPlayerDistance >= 1 - (sprite.Width / 2) && phase == 2)//hvis spilleren er til venstre
                    {
                        sprite = sprites[0]; //spriten for at løbe til venstre
                        left = true;
                        BossAI(gametime);
                    }
                    else if (enemyAndPlayerDistance <= 0 - (sprite.Width / 2) && phase == 2) //hvis spilleren er til højre
                    {
                        sprite = sprites2[0];//spriten for at løbe til højre
                        left = false;
                        BossAI(gametime);
                    }
                }
                if (MainAttackTimer >= 2 && phase == 2) 
                {
                    BossAI(gametime);
                }
                if (phase == 3) //dette er phase 3, bossen angriber nu spilleren, så derfor køres animationer og BossAI på samme tid
                {
                    BossAI(gametime);
                    Animations(gametime);
                }
                if (deleteWhen == true)//til at fjerne attack boxen
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

        public void Draw(SpriteBatch spriteBatch) // draw fucktionen til bossen
        {
            //Der tegnet bossen hvis den har over 1 liv.
           
            if (currentHP > 1)
            {
                base.Draw(spriteBatch);
                if (activator)
                {
                    //Der tegnes en healthbar's baggrund.
                    Rectangle healthRectangle = new Rectangle((int)playerPositionX - 450,
                                     (int)playerPositionY + 450,
                                     Collision.Width,
                                     hpBar.Height / 2);

                    spriteBatch.Draw(hpBar, healthRectangle, Color.Black);
                    //Der tegnes tekst.
                    spriteBatch.DrawString(enemyName, "Greed", new Vector2(healthRectangle.X + (healthRectangle.Width / 2), healthRectangle.Y - healthRectangle.Height), Color.Wheat);

                    healthPercentage = ((float)currentHP / (float)maxHp);

                    visibleWidth = (float)(Collision.Width * 2) * (float)healthPercentage;
                    //Der tegnes en healthbar's forgrund.
                    healthRectangle = new Rectangle((int)playerPositionX - 450,
                                                   (int)playerPositionY + 450,
                                                   (int)(visibleWidth / 2),
                                                   hpBar.Height / 2);

                    spriteBatch.Draw(hpBar, healthRectangle, Color.Red);
                }
            }
        }

        private void Animations(GameTime gametime) // dette er animations til bossen,vil gerne have lidt mere kontrol over det så lavede en ny
        {
            if (phase == 3)
            {
                animationTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.2f && aAnimation <= 1)//dete køre igennem animationerne
                {
                    aAnimation += 1;
                    animationTimer = 0;
                }
            }
            if (aAnimation == 2 && phase == 3)//dette er når bossen skal slå, aanimation 2 er den sidste animation i listen
            {
                deleteTimer = 0;
                deleteWhen = true;//for at slette attack boxen
                if (left == true && attacks.Count <= 1) //den skal kune lave en attack box hvis der er mindre end 1, dette er til sådan at den ikke bliver ved med at lave attack boxe oven i hinanden, og for at det ikke skal lagge spillet
                {
                    position.X -= 15; //bossen bevæger sig en lille smule når den slår
                    attacks.Add(new AttackBox(attackSprite, new Vector2(position.X - 400, position.Y + 750), 800, 3, dmg));//laver attack boxen
                }
                else if (left == false && attacks.Count <= 1)
                {
                    position.X += 15;
                    attacks.Add(new AttackBox(attackSprite, new Vector2(position.X + 500, position.Y + 750), 800, 3, dmg));
                }

                smashTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (smashTimer >= 0.4f) //dette er sådan spilleren rent faktisk kan se den sidse animation, ellers hvil den bare med det samme gå videre il næste phase og det vil se akavet ud
                {
                    aAnimation += 1;
                    animationTimer = 0;
                    smashTimer = 0;
                }
            }
            if (aAnimation >= sprites.Length || aAnimation >= sprites2.Length) //dette er først og fremmest her for ikke at få en exception, men også en god måde at gå videre til næste phase
            {
                phase = 4; //phase 4 er hvor bossen er træt
                MainAttackTimer = 0; //der er ikke nogen phase der bruger denne timer lige nu, så det er et godt tidspunkt at resette den på
            }
            if (aAnimation < sprites.Length && phase == 3 || aAnimation < sprites2.Length && phase == 3)//her sættes spriten
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

        public List<AttackBox> GetList()//for at få fat på bossens angreb i gameworld
        {
            return attacks;
        }

        public void DestroyItem(AttackBox item)//dette er for at fjerne bossens angreb
        {
            dAttacks.Add(item); //hvis norget bliver added til dAttacks, bliver det fjernet
        }
    }
}