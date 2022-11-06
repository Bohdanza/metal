using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace metal
{
    public abstract class Block
    {
        public virtual int X { get; protected set; }
        public virtual int Y { get; protected set; }
         
        public virtual bool Passable { get; protected set; }
    
        public virtual DynamicTexture Texture { get; protected set; }

        protected Block(ContentManager contentManager, string name, int x, int y, bool passable)
        {
            X = x;
            Y = y;

            Passable = passable;

            Texture = new DynamicTexture(contentManager, name);
        }

        public virtual void Update(ContentManager contentManager, Level level)
        {
            Texture.Update(contentManager);
        }

        public virtual void Draw(SpriteBatch spriteBatch, int x, int y, Color color)
        {
            spriteBatch.Draw(Texture.GetCurrentFrame(), new Vector2(x, y+Level.BlockY-Texture.GetCurrentFrame().Height), null, 
                color, 0f, new Vector2(0, 0), Level.TextureScale, SpriteEffects.None, 0f);
        }
    }
}