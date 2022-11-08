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
    public class Hero:Mob
    {
        [JsonIgnore]
        private List<DynamicTexture> HpTextures = new List<DynamicTexture>();

        public Hero(ContentManager contentManager, float x, float y):base(contentManager, x, y, x+0.7f, y+0.9f, "hero", 
            "id", "d")
        {
            ChangeHP(contentManager, -HP + 3);
        }

        public Hero(ContentManager contentManager, float x, float y, double layer)
            : base(contentManager, x, y, x+0.7f, y+0.9f, "hero", "id", "d", layer)
        {
            ChangeHP(contentManager, -HP + 3);
        }

        public override void Update(ContentManager contentManager, Level level)
        {
            var ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Space) && Landed)
            {
                AddVector(new Vector2(0, -0.09f));
            }

            if (ks.IsKeyDown(Keys.Left))
            {
                AddVector(new Vector2(-0.02f, 0));
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                AddVector(new Vector2(0.02f, 0));
            }

            if (ks.IsKeyDown(Keys.H)&&TimeSinceLastDamage>=60)
            {
                ChangeHP(contentManager, -1);
            }

            if (HP > HpTextures.Count)
                for(int i=HpTextures.Count; i<HP; i++)
                    HpTextures.Add(new DynamicTexture(contentManager, "hpfull"));

            if (HP < HpTextures.Count)
                for (int i = HP; i < HpTextures.Count; i++)
                {
                    if (HpTextures[i].BaseName != "hpfading")
                        HpTextures[i] = new DynamicTexture(contentManager, "hpfading");
                    else if (HpTextures[i].CurrentTexture == 3)
                    {
                        HpTextures.RemoveAt(i);
                        i--;
                    }
                }

            for (int i = 0; i < HpTextures.Count; i++)
                HpTextures[i].Update(contentManager);

            base.Update(contentManager, level);
        }

        public override void DrawInterface(SpriteBatch spriteBatch, Color color)
        {
            for(int i=0; i<HpTextures.Count; i++)
            {
                spriteBatch.Draw(HpTextures[i].GetCurrentFrame(),
                    new Vector2(i * HpTextures[i].GetCurrentFrame().Width * Level.TextureScale + 20+i*Level.TextureScale,
                    20), null, color, 0f, new Vector2(0, 0),
                    Level.TextureScale, SpriteEffects.None, 0f);
            }

            base.DrawInterface(spriteBatch, color);
        }
    }
}