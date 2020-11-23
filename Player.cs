using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    internal class Player : GameObject
    {
        public Player(Vector2 position, Texture2D sprite)
        {
            position = this.position;
            sprite = this.sprite;
        }

        public override void LoadContent(ContentManager contentManager)
        {
        }

        public override void OnCollision(GameObject other)
        {
        }

        public override void Update(GameTime gametime)
        {
            HandleInput();
        }

        private void HandleInput()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up))
            {
                position.Y++;
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                position.Y--;
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                position.X--;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                position.X++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            color = Color.Gray;
        }
    }
}