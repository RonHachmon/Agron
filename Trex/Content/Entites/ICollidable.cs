using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Trex.Content.Entites
{
    public interface ICollidable
    {
        public Rectangle CollisionBox {get;}
        public void OnColid(ICollidable i_Collidable);
    }
}
