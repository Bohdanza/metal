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
    public class Hero:PhysicalObject
    {
        public Hero(ContentManager contentManager, float x, float y):base(contentManager, "hero", x, y, x+0.7f, y+0.9f)
        {

        }

        public override void Update(ContentManager contentManager, Level level)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Space)||ks.IsKeyDown(Keys.W))   
                if((level.PointObstructed(X1, Y1 + 0.0001f, this) ||
                level.PointObstructed(X1, Y2 + 0.0001f, this) ||
                level.PointObstructed(X2, Y1 + 0.0001f, this) ||
                level.PointObstructed(X2, Y2 + 0.0001f, this)))
                    AddVector(new Vector2(0, -0.2f));

            if (ks.IsKeyDown(Keys.Left))
                AddVector(new Vector2(-0.05f, 0));

            if (ks.IsKeyDown(Keys.Right))
                AddVector(new Vector2(0.05f, 0));

            base.Update(contentManager, level);
        }
    }
}