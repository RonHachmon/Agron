using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Trex.Graphic;

namespace Trex.Content.Entites
{
    public class GroundLayer :IGameEntity
    {
        private static int s_GroundSpeed = 0;
        public GroundLayer(Sprite groundLayerSprite, Vector2 position, int i_Speed = 0)
        {
            GroundLayerSprite = groundLayerSprite;
            Position = position;
        }

        public Sprite GroundLayerSprite { get; set; }
        public Vector2 Position { get; set; }
 

        public int Speed
        {
            get { return s_GroundSpeed; }
            set { s_GroundSpeed = value; }
        }


        public int DrawOrder { get; set; }

        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            GroundLayerSprite.Draw(SpriteBatch, Position);
        }

        public void Update(GameTime gameTime)
        {
            if (Speed != 0)
            {
                Position = new Vector2(Position.X - Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
            }

        }

    }
}
