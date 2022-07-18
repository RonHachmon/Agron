using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;


namespace Trex.Content.Entites
{
    public abstract  class Obstacle : IGameEntity, ICollidable
    {
        public int DrawOrder { get; set; }
        public Vector2 Position { get; set; }

        public static int Speed { get; set; } = 0;
        public float DistanceTraveled { get; set; } = 0;


        public abstract int Width { get; set; }
        public abstract Rectangle CollisionBox { get; }
        public abstract void Draw(SpriteBatch SpriteBatch, GameTime GameTime);
        public abstract bool CheckCollision(ICollidable i_Collidabe);

        public Obstacle(Vector2 i_Position)
        {
            Position = i_Position;
        }

        public virtual void Update(GameTime gameTime)
        {
            float currentDistanceTraveled = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            DistanceTraveled += currentDistanceTraveled;
            float posX = Position.X - currentDistanceTraveled;
            Position = new Vector2(posX, Position.Y);

        }


        public abstract void OnColid(ICollidable i_Collidable);

    }
}
