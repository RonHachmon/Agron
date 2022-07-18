using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using Trex.Graphic;


namespace Trex.Content.Entites
{
    public class FlyingDino : Obstacle
    {
        private const int TEXTURE_COORDS_X = 134;
        private const int TEXTURE_COORDS_Y = 0;

        private const int SPRITE_WIDTH = 46;
        private const int SPRITE_HEIGHT = 42;

        private const float ANIMATION_FRAME_LENGTH = 0.2f;

        private const int VERTICAL_COLLISION_INSET = 10;
        private const int HORIZONTAL_COLLISION_INSET = 6;

        private const float SPEED_PPS = 80f;

        private SpriteAnimation _animation;


        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle collisionBox = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), SPRITE_WIDTH, SPRITE_HEIGHT);
                collisionBox.Inflate(-HORIZONTAL_COLLISION_INSET, -VERTICAL_COLLISION_INSET);
                return collisionBox;
            }
        }

        public override int Width { get; set; }

        public FlyingDino( Vector2 position, Texture2D spriteSheet, int i_Speed) : base(position)
        {
            Sprite spriteA = new Sprite(spriteSheet, TEXTURE_COORDS_X, TEXTURE_COORDS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
            Sprite spriteB = new Sprite(spriteSheet, TEXTURE_COORDS_X + SPRITE_WIDTH, TEXTURE_COORDS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);

            Speed = i_Speed;
            _animation = new SpriteAnimation();
            _animation.AddFrame(spriteA, 0);
            _animation.AddFrame(spriteB, ANIMATION_FRAME_LENGTH);
            _animation.AddFrame(spriteB, ANIMATION_FRAME_LENGTH * 2);
            _animation.Play();
            Width = SPRITE_WIDTH;

        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animation.Draw(spriteBatch, Position);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _animation.Update(gameTime);
        }
        public override bool CheckCollision(ICollidable i_Collidabe)
        {
            if (CollisionBox.Intersects(i_Collidabe.CollisionBox))
            {
                i_Collidabe.OnColid(this);
                return true;
            }
            return false;
        }

        public override void OnColid(ICollidable i_Collidable)
        {
            
        }
    }
}
