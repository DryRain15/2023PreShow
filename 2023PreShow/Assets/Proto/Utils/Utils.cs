using System.Text;
using UnityEngine;

namespace Proto.Utils
{
    public static class Utils
    {
        private static StringBuilder _stringBuilderInner = new StringBuilder();
        public static Vector2 UnitProduct(Vector2 A, Vector2 B, float depth = 0)
        {
            return new Vector2(A.x * B.x, A.y * B.y);
        }

        public static Vector3 UnitProduct(Vector3 A, Vector3 B, float depth = 0)
        {
            return new Vector3(A.x * B.x, A.y * B.y, depth);
        }

        public static string BuildString(params string[] str)
        {
            _stringBuilderInner.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                _stringBuilderInner.Append(str[i]);
            }

            return _stringBuilderInner.ToString();
        }
        
        public static Vector2 GetDirection(this InputAxis axis, float mult = 1f)
        {
            Vector2 direction = Vector2.zero;
        
            if (axis.HasFlag(InputAxis.N))
                direction.y += 1;
            if (axis.HasFlag(InputAxis.S))
                direction.y -= 1;
            if (axis.HasFlag(InputAxis.E))
                direction.x += 1;
            if (axis.HasFlag(InputAxis.W))
                direction.x -= 1;

            return direction.normalized * mult;
        }
    }
}
