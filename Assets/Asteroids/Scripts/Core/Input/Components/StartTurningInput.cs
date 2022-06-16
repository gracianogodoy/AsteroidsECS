using Unity.Entities;

namespace Asteroids.Core
{
    public struct StartTurningInput : IComponentData
    {
        public TurningDirection Direction;

        public enum TurningDirection
        {
            Left = -1,
            Right = 1
        }
    }
}