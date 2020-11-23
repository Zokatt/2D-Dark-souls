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

        protected float speed;
        protected float fps;
        private float timeElapsed;
        private int currentIndex;

        public virtual Rectangle Collision
        {
            get
            {
                return new Rectangle(
                   (int)position.X,
                   (int)position.Y,
                   (int)this.sprite.Width,
                   (int)this.sprite.Height
                   );
            }
        }

 

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

        public abstract void LoadContent(ContentManager contentManager);

        public abstract void Update(GameTime gametime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0f);
        }

        protected void Move(GameTime gameTime)
        {
            //beregner deltaTime baseret på gameTime (???)
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += ((velocity * speed) * deltaTime);
        }

        protected void Animate(GameTime gametime)
        {
            timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

            currentIndex = (int)(timeElapsed * fps);
            sprite = sprites[currentIndex];

            if (currentIndex >= sprites.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }
        }


    }
}