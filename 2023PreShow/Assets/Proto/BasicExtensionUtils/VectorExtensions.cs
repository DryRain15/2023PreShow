using UnityEngine;

namespace Proto.BasicExtensionUtils
{

    public static class VectorExtensions
    {
        public static Vector2 ToVector2(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static Vector3 ToVector3(this Vector2 vec, float depth = 0f)
        {
            return new Vector3(vec.x, vec.y, depth);
        }

        public static Vector2 GetXFlat(this Vector2 vec, float y = 0)
        {
            return new Vector2(vec.x, y);
        }

        public static Vector2 GetYFlat(this Vector2 vec, float x = 0)
        {
            return new Vector2(x, vec.y);
        }

        public static Vector3 GetXFlat(this Vector3 vec, float y = 0)
        {
            return new Vector3(vec.x, y, vec.z);
        }

        public static Vector3 GetYFlat(this Vector3 vec, float x = 0)
        {
            return new Vector3(x, vec.y, vec.z);
        }

        public static Vector3 GetOnes(this Vector3 vec)
        {
            return new Vector3(Mathf.Sign(vec.x), Mathf.Sign(vec.y), vec.z);
        }

        public static Vector3 GetXY0(this Vector2 vec, float z = 0f)
        {
            return new Vector3(vec.x, vec.y, z);
        }

        public static Vector3 GetXY0(this Vector3 vec, float z = 0f)
        {
            return new Vector3(vec.x, vec.y, z);
        }


        public static Vector2 FlipX(this Vector2 vec)
        {
            return new Vector2(-vec.x, vec.y);
        }

        public static Vector2 FlipY(this Vector2 vec)
        {
            return new Vector2(vec.x, -vec.y);
        }

        public static Vector3 FlipX(this Vector3 vec)
        {
            return new Vector3(-vec.x, vec.y, vec.z);
        }

        public static Vector3 FlipY(this Vector3 vec)
        {
            return new Vector3(vec.x, -vec.y, vec.z);
        }

        public static Vector3 AlignVector(this Vector3 vec)
        {
            return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), vec.z);
        }

        public static Vector3 AlignXVector(this Vector3 vec)
        {
            return new Vector3(Mathf.Round(vec.x), vec.y, vec.z);
        }

        public static Vector3 AlignYVector(this Vector3 vec)
        {
            return new Vector3(vec.x, Mathf.Round(vec.y), vec.z);
        }

        public static Vector3 PixelizeVector(this Vector3 vec)
        {
            return new Vector3(vec.x.PixelizedFloat(), vec.y.PixelizedFloat(), vec.z);
        }

        public static Vector2 PixelizeVector(this Vector2 vec)
        {
            return new Vector2(vec.x.PixelizedFloat(), vec.y.PixelizedFloat());
        }

        public static Vector3 GetFilteredVector(this Vector3 vec)
        {
            return new Vector3(vec.x.GetFilteredFloat(), vec.y.GetFilteredFloat(), vec.z);
        }

        public static Vector2 GetFilteredVector(this Vector2 vec)
        {
            return new Vector2(vec.x.GetFilteredFloat(), vec.y.GetFilteredFloat());
        }

        public static Vector2 GetRoundedVector(this Vector2 vec, float unit)
        {
            return new Vector2(vec.x.GetRoundedFloat(unit), vec.y.GetRoundedFloat(unit));
        }
        
        public static Vector3 GetRoundedVector(this Vector3 vec, float unit)
        {
            return new Vector3(vec.x.GetRoundedFloat(unit), vec.y.GetRoundedFloat(unit), vec.z.GetRoundedFloat(unit));
        }

        public static Vector2 Abs(this Vector2 vec)
        {
            return new Vector2(vec.x.Abs(), vec.y.Abs());
        }

        public static Vector3 Abs(this Vector3 vec)
        {
            return new Vector3(vec.x.Abs(), vec.y.Abs(), vec.z);
        }

        public static bool InBound(this Vector2 vec, Bounds bound)
        {
            return (vec.x >= bound.min.x && vec.y >= bound.min.y && vec.x <= bound.max.x && vec.y <= bound.max.y);
        }

        public static Vector3 ToFloat(this Vector3Int vec, float z = 0f)
        {
            return new Vector3(vec.x, vec.y, z);
        }
        
        public static Vector3Int RoundToInt(this Vector3 vec, int z = 0)
        {
            return new Vector3Int(vec.x.RoundToInt(), vec.y.RoundToInt(), z);
        }
        
        public static Vector3Int FloorToInt(this Vector3 vec, int z = 0)
        {
            return new Vector3Int(vec.x.FloorToInt(), vec.y.FloorToInt(), z);
        }
        
        public static Vector3Int CeilToInt(this Vector3 vec, int z = 0)
        {
            return new Vector3Int(vec.x.CeilToInt(), vec.y.CeilToInt(), z);
        }

        public static Vector3Int GetXY0Int(this Vector3Int vec, int z = 0)
        {
            return new Vector3Int(vec.x, vec.y, z);
        }
        
        public static Vector2 GetLeftPerpendicular(this Vector2 vec)
        {
            return new Vector2(-vec.y, vec.x);
        }
        
        public static Vector2 GetRightPerpendicular(this Vector2 vec)
        {
            return new Vector2(vec.y, -vec.x);
        }
    }
}