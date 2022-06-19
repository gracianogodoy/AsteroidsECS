using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public struct DoCreatePowerup : IComponentData
    {
        public int ID;
        public float2 Position;
        public float2 Velocity;
    }
}