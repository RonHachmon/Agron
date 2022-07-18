using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Trex.Graphic
{
    public class Sprite
    {
        public Sprite(Texture2D texture, int x, int y, int width, int height)
        {
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Texture2D Texture { get; private set; }
        public int Y { get; set; }
        public int X { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public void Draw(SpriteBatch spriteBatch,Vector2 Position )
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(X, Y, Width, Height), Color.White);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 Position,int WidthPortion)
        {
            spriteBatch.Draw(Texture, Position, new Rectangle(X, Y, WidthPortion, Height), Color.White);
        }

    }
}
