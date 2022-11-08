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
            Actions.Add(MoveRight);
            Actions.Add(MoveLeft);

            CurrentState = 1;
        }

        public static void MoveRight(ContentManager contentManager, Monster monster, Level level)
        {
            if (monster.PreviousState == 1 && monster.Vector.X < 0.005f-0.005f/4)
                monster.CurrentState = 2;

            monster.AddVector(new Vector2(0.005f, 0f));
        }

        public static void MoveLeft(ContentManager contentManager, Monster monster, Level level)
        {
            if (monster.PreviousState == 2 && monster.Vector.X > -0.005f+0.005f/4)
                monster.CurrentState = 1;

            monster.AddVector(new Vector2(-0.005f, 0f));
        }
    }
}