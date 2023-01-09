using Unity.Mathematics;

namespace Proto.BasicExtensionUtils
{
    public static class Constants
    {
        public const float DefaultGravity = 49f;

        public const float DegToRad = math.PI / 180f;
        public const float RadToDeg = 180f / math.PI;

        public const int PPU = 32;
        public const float PixelSize = 1 / (float) PPU;

        public const float Epsilon = 0.0001f;

        public const float JumpDefaultLimit = 0.17f;
        public const float JumpHighLimit = 0.3f;

        public const int BlockLayer = 8;
        public const int HalfBlockLayer = 9;
        public const int FXLayer = 21;
    }
}