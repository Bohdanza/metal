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
    public class Crowler:Monster
    {
        public Crowler(ContentManager contentManager, float x1, float y1):
            base(contentManager, x1, y1, x1+1f, y1+0.5f, "mrcrowley")
        {

        }
    }
}