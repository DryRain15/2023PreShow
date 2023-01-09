using UnityEngine;

namespace Proto.BasicExtensionUtils
{
    public static class FloatExtensions
    {
        public static float ToAngle(this Vector2 vec, bool acute = false)
        {
            float angle = Mathf.Atan2(vec.y, vec.x) * 180 / Mathf.PI;
            while (acute && angle > 90f)
            {
                angle -= 90f;
            }

            return angle - 90f;
        }

        /// <summary>
        /// y=0, x>0 에서 angle은 0이다. (== vector2.right)
        /// 일반적인 사분면을 따른다.
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="acute"></param>
        /// <returns></returns>
        public static float ToAngle(this Vector3 vec, bool acute = false)
        {
            float angle = Mathf.Atan2(vec.y, vec.x) * 180 / Mathf.PI;
            while (acute && angle > 90f)
            {
                angle -= 180f;
                angle = Mathf.Abs(angle);
            }

            return angle;
        }

        /// <summary>
        /// 주어진 각도를 예각으로 만들어주는 함수. 180도 단위로 자른다. 
        /// </summary>
        /// <param name="angle">수정할 각도</param>
        /// <param name="radian">radian으로 반환할 것인가?</param>
        /// <returns></returns>
        public static float ToAcute(this float angle, bool radian = false)
        {
            angle = angle.Abs();

            while (angle > 90f)
            {
                angle -= 180f;
            }

            if (radian)
                return angle.Abs() * Mathf.PI / 180f;
            return angle.Abs();
        }

        public static bool IsVertical(this float angle)
        {
            return (Mathf.Sin(angle * Constants.DegToRad)).Abs() < float.Epsilon;
        }


        public static bool IsHorizontal(this float angle)
        {
            return (Mathf.Cos(angle * Constants.DegToRad)).Abs() < float.Epsilon;
        }

        public static bool IsAlmostZero(this float value, float baseNumber = 0)
        {
            return (value - baseNumber).Abs() < Mathf.Epsilon;
        }

        public static float Abs(this float value)
        {
            return Mathf.Abs(value);
        }

        public static bool IsRelatedDirection(this float value, float other, float wide = 180f)
        {
            float min = (other - wide / 2f).GetPositiveAngle();
            float max = min + wide;

            return value.GetPositiveAngle().IsBetween(min, max);
        }

        public static bool IsBetween(this float value, float min, float max)
        {
            return (value >= min) && (value <= max);
        }

        public static float GetPositiveAngle(this float angle)
        {
            return angle < 0f ? angle + 360f : angle;
        }

        public static float PixelizedFloat(this float value)
        {
            return value.GetRoundedFloat(Constants.PPU);
        }

        public static float GetFilteredFloat(this float value)
        {
            return value.GetRoundedFloat(Constants.Epsilon);
        }
        
        public static float GetRoundedFloat(this float value, float unit)
        {
            return Mathf.Round((value / unit).Abs()) * unit * Mathf.Sign(value);
        }

        public static int RoundToInt(this float value)
        {
            return Mathf.RoundToInt(value);
        }
        
        public static int FloorToInt(this float value)
        {
            return Mathf.FloorToInt(value);
        }
        
        public static int CeilToInt(this float value)
        {
            return Mathf.CeilToInt(value);
        }
    }
}