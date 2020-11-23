using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2D_Dark_souls
{
    public class Camera
    {
        public float Zoom { get; set; } = 0.75f;
        public float Rotation { get; set; } = 0f;

        public static Rectangle screenBounds = new Rectangle(0, 0, 1600, 900);

        public Matrix TransformMatrix
        {
            get
            {
                return
                    Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(screenBounds.Width * 0.5f, screenBounds.Height * 0.5f, 0));
            }
        }

        public Player Player { get; private set; }
        public Vector2 Position { get; private set; }

        public Vector2 GetMousePositionGlobal()
        {
            return Mouse.GetState().Position.ToVector2() + Position - screenBounds.Size.ToVector2() * 0.5f;
        }

        public Camera(Player player = null) 
        {
            Player = player;
        }

        

        public void Update(GameTime gameTime)
        {
            if (Player != null)
                Position = new Vector2(Player.Collision.Center.X, Position.Y);
        }
    }
}
