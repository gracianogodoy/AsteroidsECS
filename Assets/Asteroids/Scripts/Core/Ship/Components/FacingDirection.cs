using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public struct FacingDirection : IComponentData
    {
        public float2 Value;
    }
}