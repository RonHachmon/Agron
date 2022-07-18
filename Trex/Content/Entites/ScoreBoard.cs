using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Trex.Graphic;
namespace Trex.Content.Entites
{
    public  class ScoreBoard : IGameEntity
    {
        private const int K_SCORE_WIDTH = 10;
        private const int K_SCORE_HIGHT = 11;
        private const int K_MAX_SCORE_DIGIT = 4;
        private const int K_TIMES_FLASHING = 3 ;
        private const float K_TIMES_BETWEENFLIP = 0.25f;

        private readonly int r_StatringDrawingPositionX;
        private readonly int r_StatringSpritePositionX;

        public Vector2 Position { get; set; }
        public int DrawOrder { get; set; } = int.MaxValue;
        public int DefaultsScoreIncrease { get; set; } = 20;
        public float CurrentScore { get; set; }
        public bool IsFlashing { get; set; }


        private Sprite m_ScoreSprite;
        private int? m_CheckPointScore;
        private float m_TimeFlashing = K_TIMES_BETWEENFLIP;
        private byte m_CountFlash = K_TIMES_FLASHING;
        public int CheckPointScore
        {
            get { return (int)m_CheckPointScore; }
            set { m_CheckPointScore = value; }
        }


        public event Action<ScoreBoard> ReachedCheckPoint;

        public ScoreBoard(Texture2D i_Texture2D,Vector2 i_SpriteSheetPosition,Vector2 i_ScreenPosition)
        {
            r_StatringDrawingPositionX = (int)i_ScreenPosition.X;
            r_StatringSpritePositionX = (int)i_SpriteSheetPosition.X;
            Position = i_ScreenPosition;
            m_ScoreSprite = new Sprite(i_Texture2D, (int)i_SpriteSheetPosition.X, (int)i_SpriteSheetPosition.Y, K_SCORE_WIDTH, K_SCORE_HIGHT);
        }

        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            if(IsFlashing)
            {
                if(m_TimeFlashing > 0)
                {
                    scoreToDraw(SpriteBatch, (int)CurrentScore);    
                }
               
            }
            else
            {
                scoreToDraw(SpriteBatch, (int)CurrentScore);
            }

        }
        
        public void Update(GameTime gameTime)
        {
            if (IsFlashing)
            {
                m_TimeFlashing -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (m_TimeFlashing < -K_TIMES_BETWEENFLIP)
                {
                    m_TimeFlashing = K_TIMES_BETWEENFLIP;
                    m_CountFlash--;
                    if(m_CountFlash==0)
                    {
                        m_CountFlash = K_TIMES_FLASHING;
                        IsFlashing = false;
                    }
                }

            }

           CurrentScore += DefaultsScoreIncrease * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(CurrentScore>m_CheckPointScore)
            {
                OnReachCheckPoint();
            }

        }
        private void scoreToDraw(SpriteBatch SpriteBatch,int i_ScoreToDraw)
        {
            for (int i = K_MAX_SCORE_DIGIT; i >= 0; i--)
            {
                m_ScoreSprite.X += K_SCORE_WIDTH * ((int)((i_ScoreToDraw / (Math.Pow(10, i)) % 10)));
                m_ScoreSprite.Draw(SpriteBatch, Position);
                Position = new Vector2(Position.X + K_SCORE_WIDTH, Position.Y);
                m_ScoreSprite.X = r_StatringSpritePositionX;
            }

            Position = new Vector2(r_StatringDrawingPositionX, Position.Y);

        }
        protected virtual void OnReachCheckPoint()
        {
            if (ReachedCheckPoint != null)
            {
                ReachedCheckPoint.Invoke(this);
            }
        }
    }
}
