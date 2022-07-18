using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Trex.Content.Entites;

namespace Trex.System
{
    class InputController
    {
        public bool SpaceHold { get; set; }
        public InputController ()
        {
            SpaceHold = false;
        }
        public event Action<InputController> PressedEnter;
        public void ProcessControlls(GameTime gameTime,TrexEntity trex)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (trex.IsAlive)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    trex.Jump();
                }
                else
                {
                    if (trex.TrexState == TrexEntity.eTrexState.Jumping)
                    {
                        trex.Fall();
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if (trex.TrexState == TrexEntity.eTrexState.Falling)
                    {
                        trex.Drop();
                    }
                    else
                    {
                        trex.Duck();
                    }
                }
                else
                {
                    if (trex.TrexState == TrexEntity.eTrexState.Ducking)
                    {
                        trex.TrexState = TrexEntity.eTrexState.Running;
                    }
                }
            }
            else
            {
                if(keyboardState.IsKeyDown(Keys.Enter))
                {
                    OnEnter();
                }
            }
        }
        protected virtual void OnEnter()
        {
            if (PressedEnter != null)
            {
                PressedEnter.Invoke(this);
            }
        }

    }
}
