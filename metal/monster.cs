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
        [JsonIgnore]
        public List<Action<Monster>> Actions { get; protected set; }

        [JsonIgnore]
        public int CurrentState { get; protected set; } = 0;

        public Monster(ContentManager contentManager, float x1, float y1, float x2, float y2, string name)
            : base(contentManager, x1, y1, x2, y2, name, "id", "d", 0.65f)
        {
            Actions = new List<Action<Monster>>();

            Actions.Add(ZeroAction);
        }

        public override void Update(ContentManager contentManager, Level level)
        {
            Actions[CurrentState](this);

            base.Update(contentManager, level);
        }

        /// <summary>
        /// Just to prevent from chashing
        /// </summary>
        /// <param name="monster"></param>
        public static void ZeroAction(Monster monster)
        {

        }
    }
}