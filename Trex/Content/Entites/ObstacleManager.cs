using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Trex.Content.Entites
{
    public class ObstacleManager : IGameEntity
    {
        private static readonly int[] FLYING_DINO_Y_POSITIONS = new int[] { 90, 62, 24 };
        private const int K_MIN_DISTANCE_BETWEEN_OBSTACE = 250;
        private const int K_MAX_DISTANCE_BETWEEN_OBSTACE = 700;
        public int DrawOrder => 0;


        List<Obstacle> m_GameObstacles = new List<Obstacle>();
        List<Obstacle> m_GameObstaclesToRemove = new List<Obstacle>();
        private readonly int r_ScreenWidth;
        private readonly int r_ScreenHight;
        private ICollidable m_Collidable;
        private static readonly Random sr_RandomObstacle = new Random();
        private readonly Texture2D m_Texture;
        private int m_DistanceToNextObstacle = K_MAX_DISTANCE_BETWEEN_OBSTACE;
        public bool CanSpawnObstacle { get; set; } = false;
        public int Speed { get; set; }

        public ObstacleManager(Texture2D i_Texture, int i_ScreenWidth, int i_ScreenHight, int i_Speed, ICollidable i_Collidable)
        {
            m_Collidable = i_Collidable;
            Speed = i_Speed;
            m_Texture = i_Texture;
            r_ScreenWidth = i_ScreenWidth;
            r_ScreenHight = i_ScreenHight;

        }
        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            foreach (Obstacle obstacle in m_GameObstacles)
            {
                obstacle.Draw(SpriteBatch, GameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Obstacle obstacle in m_GameObstacles)
            {
                
                if(obstacle.CheckCollision(m_Collidable))
                {
                    break;
                }
                obstacle.Update(gameTime);

                if (obstacle.Position.X < -obstacle.Width)
                {
                    m_GameObstaclesToRemove.Add(obstacle);
                }
            }

            if (m_GameObstacles.Count == 0 || m_GameObstacles.Last().DistanceTraveled > m_DistanceToNextObstacle)
            {
                generateObstacle();
                m_DistanceToNextObstacle = sr_RandomObstacle.Next(K_MIN_DISTANCE_BETWEEN_OBSTACE, K_MAX_DISTANCE_BETWEEN_OBSTACE);
            }
            foreach (Obstacle obstacle in m_GameObstaclesToRemove)
            {
                m_GameObstacles.Remove(obstacle);
            }
            m_GameObstaclesToRemove.Clear();
        }
        private void generateObstacle()
        {
            Obstacle obstacle = null;

            int cactusGroupSpawnRate = 75;
            int flyingDinoSpawnRate = 25;

            int rng = sr_RandomObstacle.Next(0, cactusGroupSpawnRate + flyingDinoSpawnRate + 1);

            if (rng <= cactusGroupSpawnRate)
            {
                bool isBig = sr_RandomObstacle.NextDouble() > 0.6f;
                CactusGroup.eCactusSize size = (CactusGroup.eCactusSize)sr_RandomObstacle.Next((int)CactusGroup.eCactusSize.Small, (int)CactusGroup.eCactusSize.Large + 1);
                obstacle = new CactusGroup(m_Texture, new Vector2(r_ScreenWidth + 10, r_ScreenHight), size, isBig, Speed);

            }
            else
            {
                int verticalPosIndex = sr_RandomObstacle.Next(0, FLYING_DINO_Y_POSITIONS.Length);
                float posY = FLYING_DINO_Y_POSITIONS[verticalPosIndex];
                obstacle = new FlyingDino(new Vector2(r_ScreenWidth, posY), m_Texture, Speed);
            }
            m_GameObstacles.Add(obstacle);
        }

        public void Clear()
        {
            m_GameObstacles.Clear();
            m_GameObstaclesToRemove.Clear();
        }
    }
}

