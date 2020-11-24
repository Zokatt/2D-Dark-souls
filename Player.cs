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
        private Vector2 gravity = new Vector2(0, 0);
        private Vector2 velocity;
        private Texture2D[] animation;
        public bool isDodging;
        public bool isGrounded = false;
        private bool buttonPress = false;
        private float dodgeTimer;
        private int dmg;

        public Player(Vector2 position)
        {
            fps = 4;
            position = this.position;
            
        }

        private void HandleInput()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up)&& buttonPress == true)
            {
                buttonPress = false;
                isGrounded = false;
                this.position.Y-=50;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                position.X--;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                position.X++;
            }
            else if (state.IsKeyUp(Keys.Up))
            {
                buttonPress = true;
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

        public override void LoadContent(ContentManager contentManager)
        {
            //sprite = contentManager.Load<Texture2D>("Jimmy");
            sprites = new Texture2D[3];
            for(int i = 0; i<sprites.Length; i++)
            {
                sprites[i] = contentManager.Load<Texture2D>(i + 1 + "JimmyMoveLeft");
            }
            sprite = sprites[0];

        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enviroment)
            {
                buttonPress = true;
                isGrounded = true;
            }
            
            
        }

        public override void Update(GameTime gametime)
        {
            HandleInput();
            dodgeTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            Animation(gametime);
            

            if (isGrounded == false)
            {
                gravity.Y += 0.2f;
                this.position.Y += gravity.Y;
            }
            else if (isGrounded == true)
            {
                gravity.Y = 0;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            color = Color.White;
        }
    }
}