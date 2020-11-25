using Microsoft.Xna.Framework;
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
        private Vector2 gravity = new Vector2(0, 0);
        private Vector2 velocity = new Vector2(0,0);
        private Texture2D[] animation;
        public bool isDodging;
        public bool isGrounded = false;
        private bool buttonPress = false;
        private bool isJumping = false;
        private float dodgeTimer;
        private float jumpTimer;
        private int dmg;
        public List<GameObject> Attacks;
        private Rectangle hitbox;

        public Player(Vector2 position)
        {
            position = this.position;
            
        }

        private void HandleInput()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up)&& buttonPress == true)
            {
                isJumping = true;
                buttonPress = false;
                isGrounded = false;
                
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                position.X-=4;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                position.X+=4;
            }
            else if (state.IsKeyUp(Keys.Up))
            {
                buttonPress = true;
            }
            else if (state.IsKeyDown(Keys.D))
            {
                PlayerAttack();
            }
        }

        public void PlayerAttack()
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y,50,50);
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

        public override void LoadContent(ContentManager contentManager)
        {
            sprite = contentManager.Load<Texture2D>("Jimmy");
            collisionTexture = contentManager.Load<Texture2D>("Pixel");
            attackSprite = contentManager.Load<Texture2D>("StoneGround");
            Attacks = new List<GameObject>();
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enviroment)
            {
                buttonPress = true;
                isGrounded = true;
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
            HandleInput();
            dodgeTimer += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (isJumping == true)
            {
                jumpTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (jumpTimer<=0.3f)
                {
                        this.position.Y -= 9;
                }
                else if (jumpTimer>=0.3f)
                {
                    jumpTimer = 0;
                    isJumping = false;
                }
            }
            

            if (isGrounded == false && isJumping == false)
            {
                gravity.Y += 0.5f;
                this.position.Y += gravity.Y;
            }
            else if (isGrounded == true)
            {
                gravity.Y = 0.5f;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);


            
          
            color = Color.White;
        }
    }
}