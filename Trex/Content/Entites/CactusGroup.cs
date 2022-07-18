using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Trex.Graphic;

namespace Trex.Content.Entites
{
    public class CactusGroup : Obstacle
    {
        public enum eCactusSize
        {
            Small=1,
            Medium=2,
            Large=3
        }
        private const int K_TINY_CACTUS_WIDTH = 17;
        private const int K_TINY_CACTUS_HIGHT = 36;
        private const int K_BIG_CACTUS_WIDTH = 25;
        private const int K_BIG_CACTUS_HIGHT = 50;
        private const int K_TINY_CACTUS_SPRITE_SHEET_POSITION_X = 228;
        private const int K_TINY_CACTUS_SPRITE_SHEET_POSITION_Y = 0;
        private const int K_BIG_CACTUS_SPRITE_SHEET_POSITION_X = 332;
        private const int K_BIG_CACTUS_SPRITE_SHEET_POSITION_Y = 0;
        public override int Width { get; set; }
        public bool  IsLarge { get; set; }
        public Sprite Sprite { get;private set; }
        public eCactusSize Size { get; }

        public override Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
                box.Inflate(-3, -3);
                return box;
            }
        }

        public CactusGroup(Texture2D i_Texture,Vector2 i_Position,eCactusSize i_Size,bool i_Large,int i_Speed):base(i_Position)
        {
            Speed = i_Speed;
            IsLarge = i_Large;
            Size = i_Size;
            DrawOrder = 50;
            Sprite = GenerateSprite(i_Texture);
            if(IsLarge)
            {
                Position = new Vector2(i_Position.X - Sprite.Width, i_Position.Y - Sprite.Height - 15);
            }
            {
                Position = new Vector2(i_Position.X - Sprite.Width, i_Position.Y - Sprite.Height -5);
            }
         

        }
        private Sprite GenerateSprite(Texture2D i_Texture)
        {
            Sprite sprite;
            int offSet = Size.GetHashCode();

            if (IsLarge)
            {
                Width = K_BIG_CACTUS_WIDTH * offSet;
                sprite = new Sprite(i_Texture,
                        K_BIG_CACTUS_SPRITE_SHEET_POSITION_X + (K_BIG_CACTUS_WIDTH * offSet),
                        K_BIG_CACTUS_SPRITE_SHEET_POSITION_Y
                        , Width,
                        K_BIG_CACTUS_HIGHT);



            }
            else
            {
                
                Width = K_TINY_CACTUS_WIDTH * offSet;

                sprite = new Sprite(i_Texture,
                        K_TINY_CACTUS_SPRITE_SHEET_POSITION_X+(K_TINY_CACTUS_WIDTH*offSet),
                        K_TINY_CACTUS_SPRITE_SHEET_POSITION_Y
                        , Width,
                        K_TINY_CACTUS_HIGHT);
            }
            return sprite;
        }
        public override void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            Sprite.Draw(SpriteBatch, Position);
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

