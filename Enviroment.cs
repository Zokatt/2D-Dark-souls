using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    public class Enviroment : GameObject
    {
        private string chosenSprite;
        private int _spriteWidth;
        private int _spriteHeight;

        //Construtor for floor
        public Enviroment(string sprite, Vector2 position, int stretch)
        {
            this._spriteWidth = stretch;
            this.chosenSprite = sprite;
            this.position = position;
            _spriteHeight = 100;
        }

        //Construtor for wall
        public Enviroment(Vector2 position, int hight)
        {
            this.position = position;
            this._spriteHeight = hight;
            _spriteWidth = 200;
            chosenSprite = "StoneGround";
        }


        public override void LoadContent(ContentManager contentManager)
        {
            sprite = contentManager.Load<Texture2D>(chosenSprite);
        }

        public override void Update(GameTime gametime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite,
                new Rectangle((int)position.X, (int)position.Y, _spriteWidth, _spriteHeight),
                new Rectangle(1, 1, sprite.Width, sprite.Height), color);
        }


        public override Rectangle Collision
        {
            get
            {
                return new Rectangle(
                       (int)position.X,
                       (int)position.Y,
                       (int)this._spriteWidth,
                       (int)this._spriteHeight
                   );
            }
        }

        public override void OnCollision(GameObject other)
        {
        }

    }
}