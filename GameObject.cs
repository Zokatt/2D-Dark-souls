﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    public abstract class GameObject
    {
        protected Texture2D sprite;
        protected Texture2D[] sprites;
        protected Vector2 velocity;
        protected Vector2 position;
        protected Color color = Color.White;
        protected SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;

        public Rectangle Collision
        {
            get
            {
                return new Rectangle(
                       (int)position.X,
                       (int)position.Y,
                       (int)sprite.Width,
                       (int)sprite.Height
                   );
            }
        }

        public abstract void LoadContent(ContentManager contentManager);

        //Abstrakt void med hvis funktion er at nedarve og anvende.
        public abstract void OnCollision(GameObject other);

        //Der laves en funktion der checker collison.
        public void CheckCollision(GameObject other)
        {
            //Hvis der er en rectangle der er inde i en anden rectangle sker der følgende.
            if (Collision.Intersects(other.Collision))
            {
                //Gøre funktion OnCollison med den anden.
                OnCollision(other);
            }
        }

        public abstract void Update(GameTime gametime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
        }
    }
}