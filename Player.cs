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
        private int hp;
        private int speed;
        private Texture2D collisionTexture;
        private Texture2D attackSprite;
        private Texture2D spriteIdle;
        private Vector2 gravity = new Vector2(0, 0);
        private Vector2 velocity = new Vector2(0, 0);
        private Texture2D[] animation;
        protected Texture2D[] sprites2;
        public bool isDodging;
        public bool isGrounded = false;
        private bool buttonPress = false;
        private bool isJumping = false;
        private float dodgeTimer;
        private float jumpTimer;
        public static int dmg = 2;
        private bool idle;
        public List<AttackBox> attacks;
        public List<AttackBox> nAttacks;
        public List<AttackBox> dAttack;
        private bool canAttack;
        private float attackTimer;
        private bool noHoldDown;
        private bool deleteWhen;
        public SoundEffect attackSound;
        private float deleteTimer;
        private bool walkOff;
        private int direction = 2;

        public Player(Vector2 position)
        {
            fps = 2;
            position = this.position;
            attacks = new List<AttackBox>();
            nAttacks = new List<AttackBox>();
            dAttack = new List<AttackBox>();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            KeyboardState state = Keyboard.GetState();
            sprite = contentManager.Load<Texture2D>("0JimmyMoveLeft");
            collisionTexture = contentManager.Load<Texture2D>("Pixel");
            attackSprite = contentManager.Load<Texture2D>("AttackEffects");
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
            spriteIdle = contentManager.Load<Texture2D>("0JimmyMoveLeft");
            attackSound = contentManager.Load<SoundEffect>("PlayerAttack");
        }

        private void HandleInput(GameTime gametime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) && isGrounded == true && isJumping == false && buttonPress == false)
            {
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
                sprite = spriteIdle;
                buttonPress = false;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 4;
                direction = 2;
                Animation(gametime,sprites2);
                //idle = false;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 4;
                direction = 1;
                Animation(gametime,sprites);
                //idle = false;
            }

            if (state.IsKeyDown(Keys.D) && canAttack == true && noHoldDown == true)
            {
                attackSound.Play();

                if (direction == 2)
                {
                    attacks.Add(new AttackBox(attackSprite, new Vector2(Collision.Right + 25, Collision.Y), 300, 1, dmg));
                }
                else if (direction == 1)
                {
                    attacks.Add(new AttackBox(attackSprite, new Vector2(Collision.Left - 300, Collision.Y), 300, 1, dmg));
                }
                canAttack = false;
                noHoldDown = false;
                deleteTimer = 0;
                deleteWhen = true;
                attackTimer = 0;
            }
            else if (state.IsKeyUp(Keys.D))
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
           
            if (other is Enviroment)
            {
                isGrounded = true;
                isJumping = false;
                walkOff = false;
                //if (other.pos.X >= position.X  && other.Collision.Location.X != position.X)
                //{
                //    this.position.X = other.Collision.Left - sprite.Width;
                //}
                if (position.X > other.Collision.Left && position.X < other.Collision.Right && position.Y > other.Collision.Top)
                {

                }
                else if (position.X+(sprite.Width/3) <= other.pos.X && position.Y + (sprite.Height/2) >= other.pos.Y)
                {
                    this.position.X = other.Collision.Left - sprite.Width;
                }
                else if (position.X + (sprite.Width / 3) >= other.pos.X && position.Y + (sprite.Height / 2) >= other.pos.Y)
                {
                    //this.position.X = other.Collision.Right;
                }
                else if (other.pos.Y >= position.Y)
                {
                    this.position.Y = other.Collision.Top - sprite.Height;
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
            HandleInput(gametime);
            dodgeTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
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
                if (gravity.Y <=30)
                {
                    gravity.Y += 0.15f;
                }
                this.position.Y += gravity.Y;
            }
            else if (isGrounded == true)
            {
                gravity.Y = 0;
            }
            else if (exitCollision == true && gravity.Y !=0)
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
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            color = Color.White;
        }

        public void DestroyItem(AttackBox item)
        {
            dAttack.Add(item);
        }

       
    }
}