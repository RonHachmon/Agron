using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Trex.Content.Entites
{

    public class EntityManager
    {
        List<IGameEntity> m_GameEntities=new List<IGameEntity>();

        public void Update(GameTime gameTime)
        {
            foreach (IGameEntity entity in m_GameEntities)
            {
                entity.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spiritBatch,GameTime gameTime)
        {
            foreach (IGameEntity entity in m_GameEntities.OrderBy(f => f.DrawOrder))
            {
                entity.Draw(spiritBatch, gameTime);
            }

        }
        public void AddEntity(IGameEntity entity)
        {
            if(entity is null)
            {
                throw new ArgumentNullException("Cant add null to entity");
            }
            m_GameEntities.Add(entity);
        }
        public void RemoveEntity(IGameEntity entity)
        {
            m_GameEntities.Remove(entity);
        }
        public void Clear()
        {
            m_GameEntities.Clear();
        }
        public IGameEntity this[int index]
        {
            get
            {
                return m_GameEntities[index];
            }
        }
        public IEnumerable<T> GetEntitesOfType<T>()where T:IGameEntity
        {
            return m_GameEntities.OfType<T>();
        }
        public bool IsEmpty()
        {
            return m_GameEntities.Count == 0;
        }

    }
}
