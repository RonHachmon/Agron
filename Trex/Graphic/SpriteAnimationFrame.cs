using System;
using System.Collections.Generic;
using System.Text;

namespace Trex.Graphic
{
    class SpriteAnimationFrame
    {
        public SpriteAnimationFrame(Sprite sprite, float screenTime)
        {
            Sprite = sprite;
            ScreenTime = screenTime;
        }

        public Sprite Sprite { get; set; }
        public float ScreenTime { get;  }
    }
}
