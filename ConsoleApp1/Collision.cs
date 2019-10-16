using System;
using System.Collections.Generic;
using Raylib;

namespace Collision
{
    class AABB
    {
        Vector2 min = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
        Vector2 max = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

        #region FromVec3
        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            return new Vector2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
        }

        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            return new Vector2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
        }

        public static Vector2 Clamp(Vector2 t, Vector2 a, Vector2 b)
        {
            return Max(a, Min(b, t));
        }
        #endregion

        public AABB()
        {
            // Purposefully Blank.
        }

        public AABB(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Create an AABB from a list of points.
        /// </summary>
        /// <param name="points">List from which to make the AABB.</param>
        public void Fit(List<Vector2> points)
        {
            // Invalidate Min and Max so new values can replace them.
            min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

            foreach (Vector2 p in points)
            {
                min = Min(min, p);
                max = Max(max, p);
            }
        }

        /// <summary>
        /// Create an AABB from an array of points.
        /// </summary>
        /// <param name="points">Array from which to make the AABB.</param>
        public void Fit(Vector2[] points)
        {
            // Invalidate Min and Max so new values can replace them.
            min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

            foreach (Vector2 p in points)
            {
                min = Min(min, p);
                max = Max(max, p);
            }
        }

        // Collision Checks --------------------

        // It's faster to check if they DONT collide since it'll excit the check sooner.
        public bool Overlaps(Vector2 p)
        {
            return !(p.x < min.x || p.y < min.y || p.x > max.x || p.y > max.y);
        }

        public bool Overlaps(AABB otherCollider)
        {
            return !(max.x < otherCollider.min.x || max.y < otherCollider.min.y || min.x > otherCollider.max.x || min.y > otherCollider.max.y);
        }

        public bool Overlaps(Circle circleCol)
        {
            return true; //placeholder
            // TODO: Figure out how to make the overlap check for this.
        }

        public Vector2 ClosestPoint(Vector2 p)
        {
            return Clamp(p, min, max);
        }

        // Finding other information from the AABB
        public Vector2 Center()
        {
            return (min + max) * 0.5f;
        }

        public Vector2 Extents()
        {
            return new Vector2(Math.Abs(max.x = min.x) * 0.5f, Math.Abs(max.y = min.y) * 0.5f);
        }

        public List<Vector2> Corners()
        {
            List<Vector2> corners = new List<Vector2>(4);
            corners[0] = min;
            corners[1] = new Vector2(min.x, max.y);
            corners[2] = max;
            corners[3] = new Vector2(max.x, min.y);
            return corners;
        }
    }

    class Circle
    {
        Vector2 center;
        float radius;

        #region FromVec3
        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            return new Vector2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
        }

        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            return new Vector2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
        }

        public static Vector2 Clamp(Vector2 t, Vector2 a, Vector2 b)
        {
            return Max(a, Min(b, t));
        }
        public float Magnitude(Vector2 v)
        {
            return (float)Math.Sqrt(v.x * v.x + v.y * v.y);
        }
        public float MagnitudeSqr(Vector2 v)
        {
            return v.x * v.x + v.y * v.y;
        }
        public float Distance(Vector2 original, Vector2 compareTo)
        {
            float diffX = original.x - compareTo.x;
            float diffY = original.y - compareTo.y;
            return (float)Math.Sqrt(diffX * diffX + diffY * diffY);
        }
        public Vector2 GetNormalized(Vector2 v)
        {
            return (v / Magnitude(v));
        }
        public float DotProduct(Vector2 lhs, Vector2 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }
        #endregion

        public Circle()
        {
            // Purposefully Blank.
        }

        public Circle(Vector2 p, float r)
        {
            center = p;
            radius = r;
        }

        public void Fit(Vector2[] points)
        {
            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);

            for (int i = 0; i < points.Length; i++)
            {
                min = Vector2.Min(min, points[i]);
                max = Vector2.Max(max, points[i]);
            }

            center = (min + max) * 0.5f;
            radius = Distance(center, max);
        }

        public void Fit(List<Vector2> points)
        {
            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);

            foreach (Vector2 p in points)
            {
                min = Vector2.Min(min, p);
                max = Vector2.Max(max, p);
            }

            center = (min + max) * 0.5f;
            radius = Distance(center, max);
        }

        // TODO: Another method of fitting a Sphere to a collection of points is to first find the average position within
        // the collection and set it to the Sphere’s center, then set the radius to the distance between the
        // center and the point farthest from the center.This method requires looping through the points multiple times.
        // Try and implement this second method yourself.

        // Collision Checks --------------------
        public bool Overlaps(Vector2 p)
        {
            Vector2 toPoint = p - center;
            return MagnitudeSqr(toPoint) <= (radius * radius);
        }

        public bool Overlaps(Circle otherCollider)
        {
            Vector2 diff = otherCollider.center - center;
            float r = radius + otherCollider.radius;
            return MagnitudeSqr(diff) <= (r * r);
        }

        public bool Overlaps(AABB aabb)
        {
            Vector2 diff = aabb.ClosestPoint(center) - center;
            return DotProduct(diff, diff) <= (radius * radius);
        }

        Vector2 ClosestPoint(Vector2 p)
        {
            // Distance from the center.
            Vector2 toPoint = p - center;

            // If outside the radius, bring it back to the radius.
            if (MagnitudeSqr(toPoint) > (radius * radius))
            {
                toPoint = GetNormalized(toPoint) * radius;
            }
            return center + toPoint;
        }
    }
}
