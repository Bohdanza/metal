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
    public class Stone:Block
    {
        public Stone(ContentManager contentManager, int x, int y):base(contentManager, "stone", x, y, false) { }
    }

    public class Air : Block
    {
        public Air(ContentManager contentManager, int x, int y) : base(contentManager, "air", x, y, true) { }
    }
}
