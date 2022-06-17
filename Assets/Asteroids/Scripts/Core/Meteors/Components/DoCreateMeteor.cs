using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public struct DoCreateMeteor : IComponentData
    {
        public Meteor.MeteorSize Size;
        public float2 Position;
    }
}