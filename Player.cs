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
        private Vector2 gravity;
        private Vector2 velocity;
        private Texture2D[] animation;
        public bool isDodging;
        public bool isGrounded;
        private bool buttonPress = false;
        private float dodgeTimer;
        private int dmg;

        public Player(Vector2 position)
        {
            //_____________________________________________________________________________________________________________________________
            fps = 5;
            speed = 900;
            velocity = Vector2.Zero;
            //_____________________________________________________________________________________________________________________________
            position = this.position;
            
        }

        private void HandleInput()
        {
            velocity = Vector2.Zero;        //_______________________________________________________________________________________________
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up))
            {
                this.position.Y--;
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                position.Y++;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                velocity += new Vector2(-1, 0);         //__________________________________________________________________________________
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                position.X++;
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
            sprite = contentManager.Load<Texture2D>("Jimmy");
            //_____________________________________________________________________________________________________________________________
            sprites = new Texture2D[3];
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i] = contentManager.Load<Texture2D>((i + 1) + "JimmyMoveLeft");
                }
            }
            sprite = sprites[0];
            //_____________________________________________________________________________________________________________________________

        }

        public override void OnCollision(GameObject other)
        {
        }

        public override void Update(GameTime gametime)
        {
            HandleInput();
            dodgeTimer += (float)gametime.ElapsedGameTime.TotalSeconds;

            Move(gametime);
            

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            color = Color.White;
        }
    }
}