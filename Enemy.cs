using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    internal class Enemy : GameObject
    {
        private int hp;
        private Vector2 offset;
        private int speed;
        private int attackTimer;
        private int dmg;

        public override void LoadContent(ContentManager contentManager)
        {
        }

        public override void OnCollision(GameObject other)
        {
        }

        public override void Update(GameTime gametime)
        {
        }

        protected Enemy(Vector2 position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
        }

        private void Almovement()
        {

        }
    }
}