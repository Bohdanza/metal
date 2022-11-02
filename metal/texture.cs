﻿using Microsoft.VisualBasic;
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
    public class DynamicTexture
    {
        public string BaseName { get; protected set; }

        protected List<Texture2D> Textures { get; set; }
        protected int CurrentTexture { get; set; }

        public DynamicTexture(ContentManager contentManager, string name)
        {
            BaseName = name;

            Load(contentManager);
        }

        /// <summary>
        /// Used to load during initialization and to reload (in case of i dunno what)
        /// </summary>
        /// <param name="contentManager">guess what u must pass here</param>
        public void Load(ContentManager contentManager)
        {
            Textures = new List<Texture2D>();
            CurrentTexture = 0;

            while (File.Exists(@"Content\" + BaseName + CurrentTexture.ToString() + ".xnb"))
            {
                Textures.Add(contentManager.Load<Texture2D>(BaseName + CurrentTexture.ToString()));

                CurrentTexture++;
            }

            CurrentTexture = 0;
        }

        /// <summary>
        /// used to move current frame and stuff
        /// </summary>
        public void Update()
        {
            CurrentTexture++;

            if (CurrentTexture >= Textures.Count)
                CurrentTexture = 0;
        }

        /// <summary>
        /// Use this to get what u need to draw
        /// </summary>
        /// <returns></returns>
        public Texture2D GetCurrentFrame()
        {
            return Textures[CurrentTexture];
        }
    }
}