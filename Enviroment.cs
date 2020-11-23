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
        public Vector2 Size { get; set; }
        
        public override Rectangle Collision
        {
            get { return new Rectangle(position.ToPoint(), (Size).ToPoint()); }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            contentManager.Load<Texture2D>("StoneGround");
        }

        public override void OnCollision(GameObject other)
        {
            if(other is Enviroment)
            {
                color = Color.Yellow;
            }
        }

        public override void Update(GameTime gametime)
        {
        }
    }
}