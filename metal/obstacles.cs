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
    public class Box : PhysicalObject
    {
        public Box(ContentManager contentManager, float x, float y, float width, float hight, string textureName) : 
            base(contentManager, textureName, x, y, x + width, y + hight)
        {

        }

        public Box(ContentManager contentManager, float x, float y, float width, float hight, string textureName, double layer) :
            base(contentManager, textureName, x, y, x + width, y + hight, true, true, false, layer)
        {

        }
    }
}