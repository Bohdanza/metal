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
    public abstract class Monster : Mob
    {
        public List<Action> Actions { get; protected set; }

        public Monster(ContentManager contentManager, float x1, float y1, float x2, float y2, string name)
            : base(contentManager, x1, y1, x2, y2, name, "id", "d")
        {
            Actions.Add(b);
        }

        public static void b()
        {

        }

        protected static void a()
        {

        }
    }
}