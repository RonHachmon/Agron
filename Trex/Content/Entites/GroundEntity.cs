using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Trex.Graphic;
using System.Linq;

namespace Trex.Content.Entites
{
    public class GroundEntity : IGameEntity
    {
        private const int K_GROUND_WIDTH = 1200;
        public const int K_GROUND_HIGHT = 12;
        private const int K_AMOUNT_OF_LAYERS = 4;
        private const int K_LAYER_WIDTH = K_GROUND_WIDTH/ K_AMOUNT_OF_LAYERS;

        private int m_ScreenWidth ;
        private readonly List<Sprite>  m_GroundArray=new List<Sprite>();
        private  List<GroundLayer> m_CurrentlyPlayingGroundTheme = new List<GroundLayer>();

        private  static readonly Random sr_RandomGroundLayer = new Random();
        public Vector2 StartingPosition { get; set; }
        public GroundEntity(Texture2D i_Texture, Vector2 i_Position,int i_ScreenWidth)
        {
            DrawOrder = 1;
            m_ScreenWidth = i_ScreenWidth;
            StartingPosition = i_Position;
            for (int i=0;i< K_AMOUNT_OF_LAYERS; i++)
            {
                Sprite GroundPiece = new Sprite(i_Texture, 2+ K_LAYER_WIDTH*i, 54, K_LAYER_WIDTH, K_GROUND_HIGHT);
                m_GroundArray.Add(GroundPiece);
            }
            int randomGround = sr_RandomGroundLayer.Next(m_GroundArray.Count);
            int amountOfPlayingLayers =(int)Math.Ceiling((double)m_ScreenWidth / K_LAYER_WIDTH);
            for (int i =0;i< amountOfPlayingLayers;i++)
            {
                m_CurrentlyPlayingGroundTheme.Add(new GroundLayer( m_GroundArray[randomGround], i_Position));
                i_Position= new Vector2(StartingPosition.X + K_LAYER_WIDTH, StartingPosition.Y);
                randomGround = sr_RandomGroundLayer.Next(m_GroundArray.Count);
            }

        }
        public int DrawOrder { get; set; }

        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {

            foreach (GroundLayer groundLayer in m_CurrentlyPlayingGroundTheme)
            {
                groundLayer.Draw(SpriteBatch, GameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
           if( m_CurrentlyPlayingGroundTheme.First().Position.X< -K_LAYER_WIDTH)
            {
                m_CurrentlyPlayingGroundTheme.Remove(m_CurrentlyPlayingGroundTheme[0]);
            }

            if (m_CurrentlyPlayingGroundTheme.Last().Position.X<m_ScreenWidth)
            {
                Vector2 newPositionOflastindex = new Vector2(m_CurrentlyPlayingGroundTheme.Last().Position.X + K_LAYER_WIDTH, m_CurrentlyPlayingGroundTheme.Last().Position.Y);
                m_CurrentlyPlayingGroundTheme.Add(new GroundLayer(m_GroundArray[sr_RandomGroundLayer.Next(m_GroundArray.Count)], newPositionOflastindex));
            }

            foreach (GroundLayer groundLayer in m_CurrentlyPlayingGroundTheme)
            {
                groundLayer.Update(gameTime);
            }

        }
        
        public void UpdateSpeed(int i_GroundSpeed)
        {
            m_CurrentlyPlayingGroundTheme[0].Speed = i_GroundSpeed;
        }
    }
}
