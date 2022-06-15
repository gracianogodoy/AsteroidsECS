using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    [InternalBufferCapacity(8)]
    public struct Points : IBufferElementData
    {
        public float2 Value;
    }
}