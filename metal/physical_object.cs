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
    public abstract class PhysicalObject
    {
        [JsonProperty]
        public bool BlockRigid { get; protected set; }
        [JsonProperty]
        public bool ObjectRigid { get; protected set; }

        [JsonProperty]
        public bool GravitationAffected { get; protected set; }
        
        [JsonProperty]
        public virtual float X1 { get; protected set; }
        [JsonProperty]
        public virtual float Y1 { get; protected set; }
        [JsonProperty]
        public virtual float X2 { get; protected set; }
        [JsonProperty]
        public virtual float Y2 { get; protected set; }

        [JsonProperty]
        public virtual DynamicTexture Texture { get; protected set; }

        [JsonProperty]
        public Vector2 Vector { get; private set; }

        [JsonProperty]
        public double Layer { get; private set; } = 0.5;
        protected bool Landed { get; private set; } = false;

        /// <summary>
        /// initializer with rigidness and gravitation enabled
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="name">texture base name</param>
        /// <param name="x1">coords</param>
        /// <param name="y1">coords</param>
        /// <param name="x2">coords</param>
        /// <param name="y2">coords</param>
        protected PhysicalObject(ContentManager contentManager, string name, float x1, float y1, float x2, float y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;

            BlockRigid = true;
            ObjectRigid = true;

            GravitationAffected = true;

            Texture = new DynamicTexture(contentManager, name);
            Vector = new Vector2(0, 0);
        }

        protected PhysicalObject(ContentManager contentManager, string name, 
            float x1, float y1, float x2, float y2, double layer)
        {
            Layer = layer;

            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;

            BlockRigid = true;
            ObjectRigid = true;

            GravitationAffected = true;

            Texture = new DynamicTexture(contentManager, name);
            Vector = new Vector2(0, 0);
        }

        protected PhysicalObject(ContentManager contentManager, string name, 
            float x1, float y1, float x2, float y2,
            bool gravitationAffected, bool blockRigid, bool objectRigid)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;

            BlockRigid = blockRigid;
            ObjectRigid = objectRigid;

            GravitationAffected = gravitationAffected;

            Texture = new DynamicTexture(contentManager, name);
            Vector = new Vector2(0, 0);
        }

        protected PhysicalObject(ContentManager contentManager, string name,
            float x1, float y1, float x2, float y2,
            bool gravitationAffected, bool blockRigid, bool objectRigid, double layer)
        {
            Layer = layer;

            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;

            BlockRigid = blockRigid;
            ObjectRigid = objectRigid;

            GravitationAffected = gravitationAffected;

            Texture = new DynamicTexture(contentManager, name);
            Vector = new Vector2(0, 0);
        }

        public virtual void Update(ContentManager contentManager, Level level)
        {
            bool b1 = level.PointObstructed(X1, Y1 + 0.0001f, this);
            bool b2 = level.PointObstructed(X1, Y2 + 0.0001f, this);
            bool b3 = level.PointObstructed(X2, Y1 + 0.0001f, this);
            bool b4 = level.PointObstructed(X2, Y2 + 0.0001f, this);

            if (b1 || b2 || b3 || b4)
            {
                Landed = true;
            }
            else
            {
                Landed = false;
            }

            X1 += Vector.X;
            X2 += Vector.X;

            if (level.PointObstructed(X1, Y1, this) || level.PointObstructed(X1, Y2, this)
                || level.PointObstructed(X2, Y1, this) || level.PointObstructed(X2, Y2, this))
            {
                X1 -= Vector.X;
                X2 -= Vector.X;

                float l = 0f, r = Vector.X;

                while (Math.Abs(r - l) >= 0.0001f)
                {
                    float mid = (r + l) / 2;
                    
                    if (level.PointObstructed(X1 + mid, Y1, this) || level.PointObstructed(X1 + mid, Y2, this)
                        || level.PointObstructed(X2 + mid, Y1, this) || level.PointObstructed(X2 + mid, Y2, this))
                    {
                        r = mid;
                    }
                    else
                    {
                        l = mid;
                    }
                }

                AddVector(new Vector2(-Vector.X+l, 0f));

                X1 += Vector.X;
                X2 += Vector.X;
            }

            AddVector(new Vector2(-Vector.X / 4, 0f));

            Y1 += Vector.Y;
            Y2 += Vector.Y;

            if (level.PointObstructed(X1, Y1, this) || level.PointObstructed(X1, Y2, this)
                || level.PointObstructed(X2, Y1, this) || level.PointObstructed(X2, Y2, this))
            {
                Y1 -= Vector.Y;
                Y2 -= Vector.Y;

                float l = 0f, r = Vector.Y;

                while(Math.Abs(r-l)>=0.0001f)
                {
                    float mid = (r + l) / 2;

                    if (level.PointObstructed(X1, Y1+mid, this) || level.PointObstructed(X1, Y2 + mid, this)
                        || level.PointObstructed(X2, Y1 + mid, this) || level.PointObstructed(X2, Y2 + mid, this))
                    {
                        r = mid;
                    }
                    else
                    {
                        l = mid;
                    }
                }

                AddVector(new Vector2(0f, -Vector.Y+l));

                Y1 += Vector.Y;
                Y2 += Vector.Y;    
            }

            if(GravitationAffected)
            {
                AddVector(new Vector2(0f, Level.Gravity));
            }

            Texture.Update(contentManager);
        }

        public virtual void Draw(SpriteBatch spriteBatch, int x, int y, Color color)
        {
            spriteBatch.Draw(Texture.GetCurrentFrame(), new Vector2(x, y), null,
                color, 0f, new Vector2(0, 0), Level.TextureScale, SpriteEffects.None, 0f);
        }
        
        public virtual void AddVector(Vector2 vector)
        {
            Vector = new Vector2(Vector.X + vector.X, Vector.Y + vector.Y);
        }

        public virtual void AddVector(float x, float y)
        {
            Vector = new Vector2(Vector.X + x, Vector.Y + y);
        }
    }
}