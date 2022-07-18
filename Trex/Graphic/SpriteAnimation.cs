using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Trex.Graphic
{
    class SpriteAnimation
    {
        List<SpriteAnimationFrame> m_Frames=new List<SpriteAnimationFrame>();
        public bool IsPlaying { get;private set; }
        public float PlayBack { get; private set; }
        public float Duration
        { 
            get
            {
                if(!m_Frames.Any())
                {
                    return 0;
                }
                return m_Frames.Max(f => f.ScreenTime);
            }
            
            }
        public SpriteAnimationFrame this[int index]
        {
            get
            {
                if (index < 0 || index > m_Frames.Count)
                    throw new ArgumentOutOfRangeException("index " + index + " is out of range");

                return m_Frames[index];
            }
        }
        public void AddFrame(Sprite Sprite,float ScreenTime)
        {
            SpriteAnimationFrame spriteAnimationFrame = new SpriteAnimationFrame(Sprite, ScreenTime);
            m_Frames.Add(spriteAnimationFrame);
        }
        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                return m_Frames
                    .Where(f => f.ScreenTime <= PlayBack)
                    .OrderBy(f => f.ScreenTime)     
                    .LastOrDefault();
            }
         }
        public void Update(GameTime gameTime)
        {
            if(IsPlaying)
            {
                PlayBack +=(float) gameTime.ElapsedGameTime.TotalSeconds;
            }
            if(PlayBack>Duration)
            {
                PlayBack -= Duration;
            }
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            SpriteAnimationFrame spriteAnimationFrame = CurrentFrame;
            if(spriteAnimationFrame!=null)
            {
                spriteAnimationFrame.Sprite.Draw(spriteBatch, Position);
            }
        }
        public void Play()
        {
            IsPlaying = true;
        }
        public void Stop()
        {
            IsPlaying = false;
            PlayBack = 0;
        }
    }
}
