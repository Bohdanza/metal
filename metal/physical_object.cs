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
    public abstract class PhysicalObject
    {
        public bool Rigid { get; protected set; }
        public bool GravitationAffected { get; protected set; }

        public virtual float X1 { get; protected set; }
        public virtual float Y1 { get; protected set; }
        public virtual float X2 { get; protected set; }
        public virtual float Y2 { get; protected set; }

        public virtual DynamicTexture Texture { get; protected set; }    

        public Vector2 Vector { get; private set; }

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

            Rigid = true;
            GravitationAffected = true;

            Texture = new DynamicTexture(contentManager, name);
            Vector = new Vector2(0, 0);
        }

        protected PhysicalObject(ContentManager contentManager, string name, 
            float x1, float y1, float x2, float y2,
            bool gravitationAffected, bool rigid)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;

            Rigid = rigid;
            GravitationAffected = gravitationAffected;

            Texture = new DynamicTexture(contentManager, name);
            Vector = new Vector2(0, 0);
        }

        public virtual void Update(Level level)
        {
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

            Texture.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch, int x, int y, Color color) 
        { spriteBatch.Draw(Texture.GetCurrentFrame(), new Vector2(x, y), color); }
        
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