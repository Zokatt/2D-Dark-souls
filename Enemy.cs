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

        public Enemy(Vector2 position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
        }

        public void AiMovement()
        {

        }

        public override void LoadContent(ContentManager contentManager)
        {
            sprite = contentManager.Load<Texture2D>("Jimmy");
        }

        public override void OnCollision(GameObject other)
        {
        }

        public override void Update(GameTime gametime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            color = Color.Black;
        }

    }
}