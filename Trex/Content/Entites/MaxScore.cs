using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Trex.Graphic;
namespace Trex.Content.Entites
{
    public class MaxScore : IGameEntity
    {
        private const int K_SCORE_WIDTH = 10;
        private const int K_SCORE_HIGHT = 11;
        private const int K_MAX_SCORE_DIGIT = 4;
        public float CurrentMaxScore { get; set; } = 0;
        public Vector2 Position { get; set; }
        public int DrawOrder { get; set; } = int.MaxValue;
        private Sprite m_ScoreSprite;
        private readonly int r_StatringDrawingPositionX;
        private readonly int r_StatringSpritePositionX;

        public MaxScore(Texture2D i_Texture2D, Vector2 i_SpriteSheetPosition, Vector2 i_ScreenPosition)
        {
            r_StatringDrawingPositionX = (int)i_ScreenPosition.X+40;
            r_StatringSpritePositionX = (int)i_SpriteSheetPosition.X;
            Position = i_ScreenPosition;
            m_ScoreSprite = new Sprite(i_Texture2D, (int)i_SpriteSheetPosition.X, (int)i_SpriteSheetPosition.Y, K_SCORE_WIDTH, K_SCORE_HIGHT);
        }

        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            drawHighScoreText(SpriteBatch, GameTime);
            int scoreToAdd = (int)CurrentMaxScore;
            for (int i = K_MAX_SCORE_DIGIT; i >= 0; i--)
            {
                m_ScoreSprite.X += K_SCORE_WIDTH * ((int)((scoreToAdd / (Math.Pow(10, i)) % 10)));
                m_ScoreSprite.Draw(SpriteBatch, Position);
                Position = new Vector2(Position.X + K_SCORE_WIDTH, Position.Y);
                m_ScoreSprite.X = r_StatringSpritePositionX;
            }

            Position = new Vector2(r_StatringDrawingPositionX, Position.Y);

        }
        private void drawHighScoreText(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            m_ScoreSprite.X += K_SCORE_WIDTH * 10;
            m_ScoreSprite.Draw(SpriteBatch, Position);
            Position = new Vector2(Position.X + K_SCORE_WIDTH, Position.Y);
            m_ScoreSprite.X += K_SCORE_WIDTH;
            m_ScoreSprite.Draw(SpriteBatch, Position);
            Position = new Vector2(Position.X + K_SCORE_WIDTH+20, Position.Y);

            m_ScoreSprite.X = r_StatringSpritePositionX;

        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
