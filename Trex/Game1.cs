using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Trex.Content.Entites;
using Trex.System;

namespace Trex
{
    public class Game1 : Game
    {
        public enum eGameState
        {
            Normal=0,
            Transition,
            Playing,
            Dead,
        }
        public enum eDisplayMode
        {
            Default,
            Zoomed
        }
        //DEFINES
        private const int K_SCREEN_WIDTH = 600;
        private const int K_SCREEN_HEIGHT = 150;
        private const int K_FADE_IN_TEXTURE_ANIMATION = 820;
        private const int K_STARTING_GAME_SPEED = 420;
        private const int K_STARTING_SCORE_SPEED = 10;
        private const int K_STARTING_CHECKPOINT_SCORE = 100;
        //Properties
        private int m_Speed = 0;
        //initals
        private GraphicsDeviceManager m_Graphics;
        private SpriteBatch m_SpriteBatch;
        private Matrix _transformMatrix = Matrix.Identity;
        public eDisplayMode WindowDisplayMode { get; set; } = eDisplayMode.Default;
        //Sprites
        private Texture2D m_TrexSpritesheet;
        private Texture2D m_FadeInTexture;
        //Sound
        private SoundEffect m_SoundHit;
        private SoundEffect m_SoundScoreReach;
        private SoundEffect m_SoundButtonPress;

        private Vector2 m_TrexLocation;
        private float m_FadeInTextureXPosition;
        //Entites
        private TrexEntity m_TrexEntity;
        private ObstacleManager m_ObstacleManager;
        private EntityManager m_EntityManager;
        private GroundEntity m_GroundEntity;
        private ScoreBoard m_ScoreBoard;
        private MaxScore m_MaxScore;
        private GameOverScreen m_GameOver;

        private InputController m_InputController;
        public eGameState GameState { get; set; }

        public Game1()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            GameState = eGameState.Normal;
            m_FadeInTextureXPosition = TrexEntity.K_TREX_WIDTH;
        }

        protected override void Initialize()
        {
            m_Graphics.PreferredBackBufferWidth = K_SCREEN_WIDTH;
            m_Graphics.PreferredBackBufferHeight = K_SCREEN_HEIGHT;
            m_Graphics.ApplyChanges();
            Window.Title = "T-Rex Runner";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            base.Initialize();
        }

        private void Window_ClientSizeChanged(object sender, global::System.EventArgs e)
        {
            if (WindowDisplayMode == eDisplayMode.Default)
            {
                WindowDisplayMode = eDisplayMode.Zoomed;
                m_Graphics.PreferredBackBufferHeight = K_SCREEN_HEIGHT * 2;
                m_Graphics.PreferredBackBufferWidth = K_SCREEN_WIDTH * 2;
                _transformMatrix = Matrix.Identity * Matrix.CreateScale(2, 2, 1);
            }
            else
            {
                WindowDisplayMode = eDisplayMode.Default;
                m_Graphics.PreferredBackBufferHeight = K_SCREEN_HEIGHT;
                m_Graphics.PreferredBackBufferWidth = K_SCREEN_WIDTH;
                _transformMatrix = Matrix.Identity;
                m_Graphics.ApplyChanges();
            }
        }

        protected override void LoadContent()
        { 
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            m_InputController = new InputController();
            m_InputController.PressedEnter += M_InputController_PressedEnter;

            //Sound
            m_SoundHit = Content.Load<SoundEffect>("Hit");
            m_SoundScoreReach = Content.Load<SoundEffect>("ScoreReached");
            m_SoundButtonPress = Content.Load<SoundEffect>("Button-Press");
            //m_TrexSpritesheet =  Content.Load<Texture2D>("T-RexSpriteSheet");
            m_TrexSpritesheet = Content.Load<Texture2D>("AgronSpriteSheet");
            

            //Sprite
            m_FadeInTexture = new Texture2D(GraphicsDevice, 1, 1);
            m_FadeInTexture.SetData(new Color[] { Color.White });


          

            m_TrexEntity = new TrexEntity(m_TrexSpritesheet, new Vector2(1, K_SCREEN_HEIGHT - 15 - TrexEntity.K_TREX_HIGHT), m_SoundButtonPress, m_SoundHit);
            m_TrexEntity.TrexDied += M_TrexEntity_TrexDied;


            m_GroundEntity = new GroundEntity(m_TrexSpritesheet, new Vector2(0, K_SCREEN_HEIGHT - GroundEntity.K_GROUND_HIGHT-15), K_SCREEN_WIDTH);

            m_ScoreBoard = new ScoreBoard(m_TrexSpritesheet, new Vector2(655, 2), new Vector2(K_SCREEN_WIDTH - 55, 2));
            m_ScoreBoard.CheckPointScore = K_STARTING_CHECKPOINT_SCORE;
            m_ScoreBoard.ReachedCheckPoint += ScoreBoard_ReachedCheckPoint;

            m_MaxScore = new MaxScore(m_TrexSpritesheet, new Vector2(655, 2), new Vector2(K_SCREEN_WIDTH - 195, 2));
            m_GameOver = new GameOverScreen(m_TrexSpritesheet, K_SCREEN_WIDTH, K_SCREEN_HEIGHT);

            //Entitys
            m_EntityManager = new EntityManager();
            m_ObstacleManager = new ObstacleManager(m_TrexSpritesheet, K_SCREEN_WIDTH, K_SCREEN_HEIGHT, K_STARTING_GAME_SPEED, m_TrexEntity);

            m_EntityManager.AddEntity(m_TrexEntity);
            m_EntityManager.AddEntity(m_GameOver);
            m_EntityManager.AddEntity(m_ScoreBoard);
            m_EntityManager.AddEntity(m_MaxScore);


        }

        private void ScoreBoard_ReachedCheckPoint(ScoreBoard obj)
        {
            obj.CheckPointScore += K_STARTING_CHECKPOINT_SCORE;
            m_SoundScoreReach.Play();
            m_ScoreBoard.IsFlashing = true;
            m_Speed += 10;
        }

        private void M_InputController_PressedEnter(InputController obj)
        {
            m_ObstacleManager.Clear();
            m_TrexEntity.Revive();
            m_TrexEntity.Position = new Vector2(1, K_SCREEN_HEIGHT - 15 - TrexEntity.K_TREX_HIGHT);
            m_Speed = K_STARTING_GAME_SPEED;
            m_GameOver.IsEnable = false;
            m_GroundEntity.UpdateSpeed(K_STARTING_GAME_SPEED);
            m_ObstacleManager.Speed = K_STARTING_GAME_SPEED;
            m_ScoreBoard.DefaultsScoreIncrease = K_STARTING_SCORE_SPEED;
            m_ScoreBoard.CheckPointScore = K_STARTING_CHECKPOINT_SCORE;

            m_ScoreBoard.CurrentScore = 0;

        }

        private void M_TrexEntity_TrexDied(TrexEntity obj)
        {
            m_Speed = 0;
            if (m_MaxScore.CurrentMaxScore < m_ScoreBoard.CurrentScore)
                m_MaxScore.CurrentMaxScore = m_ScoreBoard.CurrentScore;
            m_GroundEntity.UpdateSpeed(m_Speed);
            m_ObstacleManager.Speed = m_Speed;
            m_ScoreBoard.DefaultsScoreIncrease = m_Speed;
            m_GameOver.IsEnable = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.F12))
            {
                Window.AllowUserResizing = true;
                ToggleDisplayMode();
            }

            if (GameState == eGameState.Playing)
            {
                m_InputController.ProcessControlls(gameTime, (TrexEntity)m_EntityManager[0]);
                m_ObstacleManager.Update(gameTime);
            }
            if (GameState == eGameState.Normal)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    GameState = eGameState.Transition;
                    m_TrexEntity.Jump();
                }

            }
            if (GameState == eGameState.Transition)
            {
                m_FadeInTextureXPosition += K_FADE_IN_TEXTURE_ANIMATION * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (m_FadeInTextureXPosition > K_SCREEN_WIDTH)
                {
                    GameState = eGameState.Playing;
                    m_Speed = K_STARTING_GAME_SPEED;


                }
            }
            m_GroundEntity.UpdateSpeed(m_Speed);
            m_ObstacleManager.Speed = m_Speed;
            m_EntityManager.Update(gameTime);
            m_GroundEntity.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            m_SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _transformMatrix);
            m_EntityManager.Draw(m_SpriteBatch, gameTime);
            m_GroundEntity.Draw(m_SpriteBatch, gameTime);
            if (GameState == eGameState.Normal || GameState == eGameState.Transition)
            {
                m_SpriteBatch.Draw(m_FadeInTexture, new Rectangle((int)m_FadeInTextureXPosition, 0, K_SCREEN_WIDTH, K_SCREEN_HEIGHT), Color.White);
            }
            m_ObstacleManager.Draw(m_SpriteBatch, gameTime);
            m_SpriteBatch.End();


            base.Draw(gameTime);
        }
        private void ToggleDisplayMode()
        {


            m_Graphics.ApplyChanges();

        }
    }
}
