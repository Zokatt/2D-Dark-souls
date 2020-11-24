using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    internal class Enviroment : GameObject
    {
        private string chosenSprite;
        public Enviroment(string sprite, Vector2 position)
        {
            this.chosenSprite = sprite;
            this.position = position;
        }
        public override void LoadContent(ContentManager contentManager)
        {
            sprite = contentManager.Load<Texture2D>(chosenSprite);
        }

        public override void OnCollision(GameObject other)
        {
        }

        public override void Update(GameTime gametime)
        {
        }
    }
}