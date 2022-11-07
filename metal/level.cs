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
    public class Level
    {
        public const float Gravity = 0.01f;

        public const int BlockX = 20;
        public const int BlockY = 20;

        [JsonProperty]
        public static int TextureScale= 5;

        [JsonProperty]
        public static int BackgroundScale = 6;

        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public int Width { get; private set; }
        [JsonProperty]
        public int Height { get; private set; }

        [JsonProperty]
        public Block[,] blocks { get; private set ; }

        [JsonProperty]
        public List<PhysicalObject> objects { get; private set; }

        [JsonProperty]
        private string BackgroundName;

        private DynamicTexture BackgroundTexture=null;

        public PhysicalObject Hero { get; private set; }

        [JsonConstructor]
        public Level() 
        { 
            for (int i=0; i<objects.Count; i++) 
            { 
                if(objects[i] is Hero)
                {
                    Hero = objects[i];
                }
            } 
        }

        /// <summary>
        /// Standart init, just for testing. INIT FROM JSON FILE FOR EVERYTHING ELSE
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Level(ContentManager contentManager, int x, int y, string name, string backgroundTextureName)
        {
            BackgroundName = backgroundTextureName;
            BackgroundTexture = new DynamicTexture(contentManager, BackgroundName);

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

            Hero = AddObject(new Hero(contentManager, 2f, 2f, 0.9));
            AddObject(new Box(contentManager, 5f, 10f, 0.9f, 0.9f, "wooden_box", 0.5));

            AddObject(new Crowler(contentManager, 10f, 10f));
        }

        public void Update(ContentManager contentManager)
        {
            if (BackgroundTexture == null)
                BackgroundTexture = new DynamicTexture(contentManager, BackgroundName);
            else
                BackgroundTexture.Update(contentManager);

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    blocks[i, j].Update(contentManager, this);
                }

            for (int i=0; i<objects.Count; i++)
            {
                objects[i].Update(contentManager, this);
            }
        }   

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            if(BackgroundTexture!=null)
            {
                Texture2D toDraw = BackgroundTexture.GetCurrentFrame();

                spriteBatch.Draw(toDraw, new Vector2(
                    (int)((double)x / (Width * BlockX * TextureScale - 1920) * (toDraw.Width * BackgroundScale - 1920)),
                    (int)((double)y / (Height * BlockY * TextureScale - 1080) * (toDraw.Height * BackgroundScale - 1080))),
                    null, Color.White, 0f, new Vector2(0, 0), BackgroundScale, SpriteEffects.None, 0f);
            }

            for (int i = Math.Max(0, -x / (BlockX*TextureScale)); i < Math.Min(Width, (1920 - x) / (BlockX * TextureScale) + 1); i++)
            {
                for (int j = Math.Max(0, -y / (BlockY*TextureScale)); j < Math.Min(Height, (1080 - y) / (BlockY * TextureScale) + 1); j++)
                {
                    blocks[i, j].Draw(spriteBatch, x + i * BlockX * TextureScale, y + j * BlockY * TextureScale, Color.White);
                }
            }

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Draw(spriteBatch,
                    x + (int)((float)objects[i].X1 * BlockX * TextureScale), y + (int)((float)objects[i].Y1 * BlockY * TextureScale),
                    Color.White);
            }
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter("levels/" + Name))
            {
                string str = JsonConvert.SerializeObject(this, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                sw.Write(str);
            }
        }

        public static Level Load(string name)
        {
            using (StreamReader sr = new StreamReader("levels/" + name))
            {
                string str = sr.ReadToEnd();

                return JsonConvert.DeserializeObject<Level>(str, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                }) ;
            }
        }

        public bool PointObstructed(float x, float y, PhysicalObject physicalObject)
        {
            if(physicalObject.BlockRigid && !blocks[(int)Math.Floor(x), (int)Math.Floor(y)].Passable)
            {
                return true;
            }

            if(physicalObject.ObjectRigid)
                foreach(var currentObject in objects)
                {
                    if(currentObject!=physicalObject&&currentObject.ObjectRigid
                        && PointBelongs(x, y, currentObject.X1, currentObject.Y1, currentObject.X2, currentObject.Y2))
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

        public PhysicalObject AddObject(PhysicalObject physicalObject)
        {
            if (objects.Count < 1)
            {
                objects.Add(physicalObject);
                
                return physicalObject;
            }

            double lr = physicalObject.Layer;
            int l = 0, r = objects.Count-1;

            while (l<r-1)
            {
                int mid = (l + r) / 2;

                if (objects[mid].Layer < lr)
                    l = mid;
                else
                    r = mid;
            }

            if (objects[r].Layer < physicalObject.Layer)
                objects.Insert(r, physicalObject);
            else
                objects.Insert(l, physicalObject);

            return physicalObject;
        }
    }
}