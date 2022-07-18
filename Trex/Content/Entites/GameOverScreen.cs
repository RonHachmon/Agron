using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Trex.Graphic;

namespace Trex.Content.Entites
{
    class GameOverScreen : IGameEntity
    {
        private const int K_GAME_OVER_TEXTURE_POS_X = 655;
        private const int K_GAME_OVER_TEXTURE_POS_Y = 14;
        private const int K_GAME_OVER_WIDTH = 192;
        private const int K_GAME_OVER_HIGHT = 14;

        private const int K_GAME_OVER_RESET_BUTTON_POS_X= 254;
        private const int K_GAME_OVER_RESET_BUTTON_POS_Y = 67;
        private const int K_GAME_OVER_RESET_BUTTON_WIDTH= 36;
        private const int K_GAME_OVER_RESET_BUTTON_HIGHT= 34;

        public int DrawOrder => 100;
        private Sprite m_GameOverText;
        private Sprite m_ResetSymbol;
        public bool IsEnable { get; set; } = false;
        public Vector2 TextPosition { get;private set; }
        public Vector2 ButtonPosition { get; private set; }
        public GameOverScreen(Texture2D i_Texture,int i_ScreenWidth,int i_Screenight)
        {
            m_GameOverText = new Sprite(i_Texture, K_GAME_OVER_TEXTURE_POS_X, K_GAME_OVER_TEXTURE_POS_Y, K_GAME_OVER_WIDTH, K_GAME_OVER_HIGHT);
            TextPosition = new Vector2(i_ScreenWidth / 2 - K_GAME_OVER_WIDTH / 2, i_Screenight / 2 - K_GAME_OVER_RESET_BUTTON_HIGHT / 2-17);
            m_ResetSymbol = new Sprite(i_Texture, K_GAME_OVER_RESET_BUTTON_POS_X, K_GAME_OVER_RESET_BUTTON_POS_Y, K_GAME_OVER_RESET_BUTTON_WIDTH, K_GAME_OVER_RESET_BUTTON_HIGHT);
            ButtonPosition = new Vector2(TextPosition.X + K_GAME_OVER_WIDTH / 2 - K_GAME_OVER_RESET_BUTTON_WIDTH / 2, TextPosition.Y + K_GAME_OVER_HIGHT  + K_GAME_OVER_RESET_BUTTON_HIGHT/2-5);

        }

        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            if(IsEnable)
            {
                m_ResetSymbol.Draw(SpriteBatch, ButtonPosition);
                m_GameOverText.Draw(SpriteBatch, TextPosition);
            }  
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
