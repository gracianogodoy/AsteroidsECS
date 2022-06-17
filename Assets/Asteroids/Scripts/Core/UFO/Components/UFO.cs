using Unity.Entities;

namespace Asteroids.Core
{
    public struct UFO : IComponentData
    {
        public UFOType Type;

        public enum UFOType
        {
            Big,
            Small
        }
    }
}