using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Trex.Graphic;

namespace Trex.Content.Entites
{
    public class TrexEntity : IGameEntity, ICollidable
    {
        public enum eTrexState
        {
            Idle,
            Running,
            Jumping,
            Falling,
            Ducking,
            Dead,
        }
        public const int K_TREX_WIDTH = 44;
        public const int K_TREX_HIGHT = 50;

        public const int K_DUCKING_TREX_WIDTH = 59;
        public const int K_DUCKING_TREX_HIGHT = 34;
        public const int K_DUCK_COLLISION_REDUCTION = 20;

        public const int K_DEFAULT_SPRITE_SHEET_Y = 0;
        public const int K_DEFAULT_SPRITE_SHEET_X = 848;

        private const int K_IDLE_STATE_SPRITE_X = 36;

        public const int K_RUNNING_SPRITE_SHEET_X = K_DEFAULT_SPRITE_SHEET_X + K_TREX_WIDTH*2;

        public const int K_DUCKING_SPRITE_SHEET_X = K_DEFAULT_SPRITE_SHEET_X + K_TREX_WIDTH * 6;

        private const float K_STARTING_JUMP_VELOCITY = 550f;
        private const float K_DROP_VELOCITY = 1000f;
        private const float K_STARTING_GRAVITY_VELOCITY = K_STARTING_JUMP_VELOCITY / 15;

        private readonly float r_Starting_Y_Position;
        private float m_DefaultDropVelocity;
        private float m_JumpVelocity= K_STARTING_JUMP_VELOCITY;
        private float m_GravityVelocity = K_STARTING_GRAVITY_VELOCITY;

        private Sprite m_Jump_Sprite;
        private Sprite m_Dead_Sprite;
        private SpriteAnimation m_IdleAnimation;
        private SpriteAnimation m_RunningAnimation;
        private SpriteAnimation m_DuckingAnimation;

        public event Action<TrexEntity> TrexDied;

        private SoundEffect m_JumpSound;
        private SoundEffect m_DeathSound;
        public TrexEntity(Texture2D i_Texture,Vector2 i_Position, SoundEffect i_JumpSound,SoundEffect i_DeathSound)
        {
            IsAlive = true;
            m_DefaultDropVelocity = 0;
            Speed = 400f;
            TrexState = eTrexState.Idle;
            Position = i_Position;
            r_Starting_Y_Position = i_Position.Y;
            m_Jump_Sprite = new Sprite(i_Texture, K_DEFAULT_SPRITE_SHEET_X, K_DEFAULT_SPRITE_SHEET_Y, K_TREX_WIDTH, K_TREX_HIGHT);
            m_Dead_Sprite = new Sprite(i_Texture, K_DEFAULT_SPRITE_SHEET_X + K_TREX_WIDTH * 4, K_DEFAULT_SPRITE_SHEET_Y, K_TREX_WIDTH, K_TREX_HIGHT);

            m_IdleAnimation = new SpriteAnimation();
            m_IdleAnimation.AddFrame(new Sprite(i_Texture, K_DEFAULT_SPRITE_SHEET_X, K_DEFAULT_SPRITE_SHEET_Y, K_TREX_WIDTH, K_TREX_HIGHT), 0);
            m_IdleAnimation.AddFrame(new Sprite(i_Texture, K_DEFAULT_SPRITE_SHEET_X+ K_TREX_WIDTH, K_DEFAULT_SPRITE_SHEET_Y, K_TREX_WIDTH, K_TREX_HIGHT), 4f);
            m_IdleAnimation.AddFrame(m_IdleAnimation[1].Sprite, 4.25f);
            m_IdleAnimation.Play();

            m_RunningAnimation = new SpriteAnimation();
            m_RunningAnimation.AddFrame(new Sprite(i_Texture, K_RUNNING_SPRITE_SHEET_X, K_DEFAULT_SPRITE_SHEET_Y, K_TREX_WIDTH, K_TREX_HIGHT), 0);
            m_RunningAnimation.AddFrame(new Sprite(i_Texture, K_RUNNING_SPRITE_SHEET_X+K_TREX_WIDTH, K_DEFAULT_SPRITE_SHEET_Y, K_TREX_WIDTH, K_TREX_HIGHT), 1/10f);
            m_RunningAnimation.AddFrame(m_RunningAnimation[0].Sprite, 1/5f);
            m_RunningAnimation.Play();

            m_DuckingAnimation = new SpriteAnimation();
            m_DuckingAnimation.AddFrame(new Sprite(i_Texture, K_DUCKING_SPRITE_SHEET_X, K_DEFAULT_SPRITE_SHEET_Y, K_DUCKING_TREX_WIDTH, K_TREX_HIGHT), 0);
            m_DuckingAnimation.AddFrame(new Sprite(i_Texture, K_DUCKING_SPRITE_SHEET_X + K_DUCKING_TREX_WIDTH, K_DEFAULT_SPRITE_SHEET_Y, K_DUCKING_TREX_WIDTH, K_TREX_HIGHT), 1 / 10f);
            m_DuckingAnimation.AddFrame(m_DuckingAnimation[0].Sprite, 1 / 5f);
            m_DuckingAnimation.Play();

            m_DeathSound = i_DeathSound;
            m_JumpSound = i_JumpSound;
        }


        public bool IsAlive { get; private set; } = true;
        public float Speed { get; set; }
        public eTrexState TrexState { get; set; }
        public int DrawOrder { get;  set; }

        public Vector2 Position { get; set; }

        public Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle((int)Position.X, (int)Position.Y, K_TREX_WIDTH, K_TREX_HIGHT);
                box.Inflate(-3, -3);
                if (TrexState == eTrexState.Ducking)
                {
                    box.Y += K_DUCK_COLLISION_REDUCTION;
                    box.Height -= K_DUCK_COLLISION_REDUCTION;

                }
                return box;
            }
        }

        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            if(TrexState == eTrexState.Idle)
            {
                m_IdleAnimation.Draw(SpriteBatch, Position);
            }
            if (TrexState == eTrexState.Jumping|| TrexState == eTrexState.Falling)
            {
                m_Jump_Sprite.Draw(SpriteBatch, Position);
            }
            if (TrexState == eTrexState.Running)
            {
                m_RunningAnimation.Draw(SpriteBatch, Position);
            }
            if (TrexState == eTrexState.Ducking)
            {
                m_DuckingAnimation.Draw(SpriteBatch, Position);
            }
            if (TrexState == eTrexState.Dead)
            {
                m_Dead_Sprite.Draw(SpriteBatch, Position);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (TrexState == eTrexState.Idle)
            {
                m_IdleAnimation.Update(gameTime);
            }
            if (TrexState == eTrexState.Jumping)
            {
                Position = new Vector2(Position.X, Position.Y - m_JumpVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                m_JumpVelocity -= m_GravityVelocity;
                m_GravityVelocity = m_JumpVelocity / 15;
                    if (Position.Y < -5)
                    {
                    m_JumpVelocity = K_STARTING_JUMP_VELOCITY;
                    Position = new Vector2(Position.X, 0);
                        TrexState = eTrexState.Falling;
                    }
            }
            if (TrexState == eTrexState.Falling)
            {
                Position = new Vector2(Position.X, Position.Y + (m_JumpVelocity + m_DefaultDropVelocity) * (float)gameTime.ElapsedGameTime.TotalSeconds);
                m_JumpVelocity -= m_GravityVelocity;
                if (Position.Y >= r_Starting_Y_Position)
                {
                    m_GravityVelocity = K_STARTING_GRAVITY_VELOCITY;
                    m_JumpVelocity = K_STARTING_JUMP_VELOCITY;
                    Position = new Vector2(Position.X, r_Starting_Y_Position);
                    TrexState = eTrexState.Running;
                }
                m_DefaultDropVelocity = 0;
            }
            if (TrexState == eTrexState.Running)
            {
                m_RunningAnimation.Update(gameTime);
            }
            if (TrexState == eTrexState.Ducking)
            {
                m_DuckingAnimation.Update(gameTime);
            }
        }
        public void Jump()
        {
            if (TrexState == eTrexState.Jumping || TrexState == eTrexState.Falling)
            {
                return;

            }
            if (TrexState != eTrexState.Jumping)
            {
                m_JumpSound.Play();
                TrexState = eTrexState.Jumping;

            }

        }
        public void Duck()
        {
            if (TrexState == eTrexState.Running)
            {
                TrexState = eTrexState.Ducking;
            }
        }
        public void Drop()
        {
            if (TrexState == eTrexState.Falling)
            {
                m_DefaultDropVelocity = K_DROP_VELOCITY;
            }
        }
        public void Fall()
        {
            TrexState = eTrexState.Falling;
            m_JumpVelocity = K_STARTING_JUMP_VELOCITY;
        }
        public void Die()
        {
            if(IsAlive)
            {
                m_DeathSound.Play();
                TrexState = eTrexState.Dead;
                IsAlive = false;
                OnDeath();
            }
        }
        public void Revive()
        {
            TrexState = eTrexState.Running;
            m_JumpVelocity = K_STARTING_JUMP_VELOCITY;
            m_GravityVelocity = K_STARTING_GRAVITY_VELOCITY;
            IsAlive = true;
        }

        public void OnColid(ICollidable i_Collidable)
        {
            if(i_Collidable is Obstacle)
            {
                this.Die();
            }
        }
        protected virtual void OnDeath()
        {
            if(TrexDied!= null)
            {
                TrexDied.Invoke(this);
            }
        }
    }
}
