using Microsoft.Xna.Framework;
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
        protected bool exitCollision;

        protected float fps;
        private float timeElapsed;
        private int currentIndex;
        public Vector2 pos
        {
            get { return this.position; }
        }

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
                exitCollision = false;
                OnCollision(other);
            }
            if (!Collision.Intersects(other.Collision))
            {
                exitCollision = true;
            }
        }

        public abstract void Update(GameTime gametime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }



        protected void Animation(GameTime gametime, Texture2D[] chosenSprites)
        {
            //Adds time that has passed since last update
            timeElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;
            //Calculate the current index
            currentIndex = (int)(timeElapsed * fps);

            sprite = chosenSprites[currentIndex];

            //Checks if we need to restart the animation
            if (currentIndex >= sprites.Length - 1)
            {
                //Resets the animation
                timeElapsed = 0;
                currentIndex = 0;
            }
        }

    }
}