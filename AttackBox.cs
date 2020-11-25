using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    public class AttackBox : GameObject
    {
        //private string chosenSprite;
        private int _spriteWidth;

        public AttackBox(Texture2D chosenSprite, Vector2 position, int stretch)
        {
            this._spriteWidth = stretch;
            this.sprite = chosenSprite;
            this.position = position;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            //sprite = contentManager.Load<Texture2D>(chosenSprite);
        }

        public override void OnCollision(GameObject other)
        {
        }

        public override void Update(GameTime gametime)
        {
        }

        public override Rectangle Collision
        {
            get
            {
                return new Rectangle(
                       (int)position.X,
                       (int)position.Y,
                       (int)this._spriteWidth,
                       (int)this.sprite.Height
                   );
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite,
                new Rectangle((int)position.X, (int)position.Y, _spriteWidth, sprite.Height),
                new Rectangle(1, 1, sprite.Width, sprite.Height), color);
        }
    }
}