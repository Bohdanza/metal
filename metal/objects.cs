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

namespace metal
{
    public class Hero:Mob
    {
        public Hero(ContentManager contentManager, float x, float y):base(contentManager, x, y, x+0.7f, y+0.9f, "hero", 
            "id", "d")
        {

        }

        public Hero(ContentManager contentManager, float x, float y, double layer)
            : base(contentManager, x, y, x+0.7f, y+0.9f, "hero", "id", "d", layer)
        {

        }

        public override void Update(ContentManager contentManager, Level level)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Space) && Landed)
            {
                AddVector(new Vector2(0, -0.09f));
            }

            if (ks.IsKeyDown(Keys.Left))
            {
                AddVector(new Vector2(-0.02f, 0));
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                AddVector(new Vector2(0.02f, 0));
            }

            base.Update(contentManager, level);
        }
    }
}