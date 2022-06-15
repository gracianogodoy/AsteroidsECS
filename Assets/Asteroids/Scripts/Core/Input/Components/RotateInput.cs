using Unity.Entities;

namespace Asteroids.Core
{
    public struct RotateInput : IComponentData
    {
        public Direction Value;

        public enum Direction
        {
            Left = -1,
            Right = 1
        }
    }
}