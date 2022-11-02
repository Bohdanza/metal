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
    public class Level
    {
        public const float Gravity = 0.01f;

        public const int BlockX = 100;
        public const int BlockY = 100;

        public string Name { get; private set; }
        
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Block[,] blocks { get; private set; }
        public List<PhysicalObject> objects { get; private set; }

        public Level(ContentManager contentManager, int x, int y, string name)
        { 
            Name = name;

            Width = x;
            Height = y;

            blocks = new Block[Width, Height];

            for(int i=0; i<Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    blocks[i, j] = new Air(contentManager, i, j);

                    if (i == 0 || j == 0 || i == Width - 1 || j == Height - 1 || (j == Height - 3 && i != 1))
                        blocks[i, j] = new Stone(contentManager, i, j);
                }

            objects = new List<PhysicalObject>();

            objects.Add(new Hero(contentManager, 2f, 2f));
        }
        
        public void Update(ContentManager contentManager)
        {
            for(int i=0; i<objects.Count; i++)
            {
                objects[i].Update(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            for (int i = Math.Max(0, -x / BlockX); i < Math.Min(Width, (1920 - x) / BlockX + 1); i++)
            {
                for (int j = Math.Max(0, -y / BlockY); j < Math.Min(Height, (1080 - y) / BlockY + 1); j++)
                {
                    blocks[i, j].Draw(spriteBatch, x + i * BlockX, y + j * BlockY, Color.White);
                }
            }

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Draw(spriteBatch, 
                    x+(int)((float)objects[i].X1 * BlockX), y+(int)((float)objects[i].Y1 * BlockY), 
                    Color.White);
            }
        }

        public bool PointObstructed(float x, float y, PhysicalObject physicalObject)
        {
            if(!blocks[(int)Math.Floor(x), (int)Math.Floor(y)].Passable)
            {
                return true;
            }

            foreach(var currentObject in objects)
            {
                if(currentObject!=physicalObject&&currentObject.Rigid&&PointBelongs(x, y, currentObject.X1, currentObject.Y1, currentObject.X2, currentObject.Y2))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if (x, y) belongs to square [x1; y1; x2; y2]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public bool PointBelongs(float x, float y, float x1, float y1, float x2, float y2)
        {
            if(x>=x1&&x<=x2&&y>=y1&&y<=y2)
            {
                return true;
            }

            return false;
        }
    }
}