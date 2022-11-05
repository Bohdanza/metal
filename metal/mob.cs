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
    public abstract class Mob:PhysicalObject
    {
        [JsonProperty]
        public string Action { get; private set; }
        [JsonProperty]
        public string Direction { get; private set; }
        private string Name;

        public Mob(ContentManager contentManager, float x1, float y1, float x2, float y2, 
            string name, string action, string direction):base(contentManager, name, x1, y1, x2, y2)
        {
            Action = action;
            Direction = direction;

            Texture = new DynamicTexture(contentManager, name + "_" + Action + "_" + Direction);
            Name = name;
        }

        public Mob(ContentManager contentManager, float x1, float y1, float x2, float y2,
            string name, string action, string direction, double layer) : base(contentManager, name, x1, y1, x2, y2, layer)
        {
            Action = action;
            Direction = direction;

            Texture = new DynamicTexture(contentManager, name + "_" + Action + "_" + Direction);
            Name = name;
        }

        protected void ChangeAction(ContentManager contentManager, string action)
        {
            if (action != Action)
            {
                Action = action;

                Texture = new DynamicTexture(contentManager, Name + "_" + Action + "_" + Direction);
            }
        }

        protected void ChangeDirection(ContentManager contentManager, string direction)
        {
            if (Direction != direction)
            {
                Direction = direction;

                Texture = new DynamicTexture(contentManager, Name + "_" + Action + "_" + Direction);
            }
        }
    }
}