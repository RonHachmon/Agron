using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trex.Content.Entites
{
    public interface IGameEntity
    {
        int DrawOrder { get; }
        void Draw(SpriteBatch SpriteBatch, GameTime GameTime);

        void Update(GameTime gameTime);


    }
}
