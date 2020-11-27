﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    public class Player : GameObject
    {
        private int hp;
        private int speed;
        private Texture2D collisionTexture;
        private Texture2D attackSprite;
        private Vector2 gravity = new Vector2(0, 0);
        private Vector2 velocity = new Vector2(0, 0);
        private Texture2D[] animation;
        public bool isDodging;
        public bool isGrounded = false;
        private bool buttonPress = false;
        private bool isJumping = false;
        private float dodgeTimer;
        private float jumpTimer;
        private int dmg;
        private bool idle;
        private Texture2D spriteIdle;
        public List<AttackBox> Attacks;
        public List<AttackBox> nAttacks;
        public List<AttackBox> dAttack;
        private bool canAttack;
        private float attackTimer;
        private bool noHoldDown;
        private bool deleteWhen;
        private float deleteTimer;
        private bool walkOff;

        public Player(Vector2 position)
        {
            fps = 4;
            position = this.position;
            Attacks = new List<AttackBox>();
            nAttacks = new List<AttackBox>();
            dAttack = new List<AttackBox>();
        }

        private void HandleInput(GameTime gametime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) && isGrounded == true && isJumping == false && buttonPress == false)
            {
                isJumping = true;
                isGrounded = false;
                buttonPress = true;
                gravity.Y = 0;
            }
            if (state.IsKeyUp(Keys.Up) && isGrounded == false && jumpTimer >=0.2f)
            {
                isJumping = false;
                buttonPress = false;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                position.X += 4;
                Animation(gametime);
                idle = false;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                position.X -= 4;    
                Animation(gametime);
                idle = false;
            }
            if (state.IsKeyUp(Keys.Up))
            {
                buttonPress = false;
                sprite = spriteIdle;
            }
            if (state.IsKeyDown(Keys.D)&&canAttack == true && noHoldDown == true)
            {
                Attacks.Add(new AttackBox(attackSprite, new Vector2(Collision.X + 300, Collision.Y), 400,1));
                canAttack = false;
                noHoldDown = false;
                deleteTimer = 0;
                deleteWhen = true;
                attackTimer = 0;
            }
            else if (state.IsKeyUp(Keys.D))
            {
                noHoldDown = true;
            }

        }

        public void PlayerAttack()
        {
            //Attack!!
        }

        public void Dodge()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.F) && dodgeTimer == 2 && buttonPress == false)
            {
                isDodging = true;
                dodgeTimer = 0;
                buttonPress = true;
            }
            if (state.IsKeyUp(Keys.F)) //you can only press dodge again if you let go of the button
            {
                buttonPress = false;
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            KeyboardState state = Keyboard.GetState();
            sprite = contentManager.Load<Texture2D>("0JimmyMoveLeft");
            collisionTexture = contentManager.Load<Texture2D>("Pixel");
            attackSprite = contentManager.Load<Texture2D>("PlayerAttackBox");
            sprites = new Texture2D[3];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = contentManager.Load<Texture2D>(i + 1 + "JimmyMoveLeft");
            }
            spriteIdle = contentManager.Load<Texture2D>("0JimmyMoveLeft");
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enviroment)
            {
                isGrounded = true;
                isJumping = false;
                walkOff = false;
                this.position.Y = other.Collision.Top - sprite.Height; 
            }
        }

        public void DrawCollisionBox(GameObject go)
        {
            //Der laves en streg med tykkelsen 1 for hver side af Collision.
            Rectangle topLine = new Rectangle(go.Collision.X, go.Collision.Y, go.Collision.Width, 1);
            Rectangle bottomLine = new Rectangle(go.Collision.X, go.Collision.Y + go.Collision.Height, go.Collision.Width, 1);
            Rectangle rightLine = new Rectangle(go.Collision.X + go.Collision.Width, go.Collision.Y, 1, go.Collision.Height);
            Rectangle leftLine = new Rectangle(go.Collision.X, go.Collision.Y, 1, go.Collision.Height);
            //Der tegnes en streg med tykkelsen 1 for hver side af Collision med collsionTexture med farven rød.
            _spriteBatch.Draw(collisionTexture, topLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, bottomLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, rightLine, Color.Red);
            _spriteBatch.Draw(collisionTexture, leftLine, Color.Red);
        }

        public override void Update(GameTime gametime)
        {
            HandleInput(gametime);
            dodgeTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            attackTimer += (float)gametime.ElapsedGameTime.TotalSeconds;

            
            

            if (deleteWhen == true)
            {
                deleteTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            if (deleteTimer>=0.1f) //how fast should the attack destroy itself
            {
                foreach (var item in Attacks)
                {
                    DestroyItem(item);
                }
                deleteWhen = false;
            }

            if (isJumping == true)
            {
               this.position.Y -= 9;
               jumpTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                
            }
            if (isJumping == false)
            {
                jumpTimer = 0;
            }
            if (isGrounded == false && isJumping == false || exitCollision == true)
            {
                gravity.Y += 0.15f;
                this.position.Y += gravity.Y;
            }
            else if (isGrounded == true)
            {
                gravity.Y = 0;
            }

            

            if (attackTimer>=1)
            {
                canAttack = true;
            }
            

           
                //Attacks.AddRange(nAttacks);
                if (dAttack.Count > 0)
                {
                    foreach (var item in dAttack)
                    {
                        Attacks.Remove(item);
                    }
                    dAttack.Clear();
                }
            
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            color = Color.White;
        }
        public void DestroyItem(AttackBox item)
        {
            dAttack.Add(item);
        }
    }
}