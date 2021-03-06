﻿using Microsoft.Xna.Framework;
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

        public static int ID; //use this to determine who spawned this, 1 for player, 2 for enemy

        public int setID
        {
            get { return ID; }
            set { ID = value; }
        }

        public int damage; //hvor meget skade denne attackbox gør

        public AttackBox(Texture2D chosenSprite, Vector2 position, int stretch, int idNumber, int damage)//construktoren for attaxkboxen
        {
            _spriteWidth = stretch; //hvis man selv vil bestemme hvor bred attack boxen skal være
            sprite = chosenSprite;
            this.position = position;
            this.setID = idNumber; //dette er hvem der laver boxen
            this.damage = damage;
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

        //Collisionboks bliver overskrevet.
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

        public override void Draw(SpriteBatch spriteBatch)//draw for attack boxen, den brede og vidé er spritens, så kan man eventuelt lave en tom sprite, og selv bestemme hvor stor den være
        {
            spriteBatch.Draw(sprite,
                new Rectangle((int)position.X, (int)position.Y, _spriteWidth, sprite.Height),
                new Rectangle(1, 1, sprite.Width, sprite.Height), color);
        }
    }
}