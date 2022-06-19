using Unity.Mathematics;

namespace Asteroids.Core
{
    public static class VectorHelper
    {
        public static float2 Rotate(float angle, float2 vector)
        {
            var ca = math.cos(angle);
            var sa = math.sin(angle);

            var newValue = new float2(vector.x * ca - vector.y * sa, vector.x * sa + vector.y * ca);
            return newValue;
        }
    }
}