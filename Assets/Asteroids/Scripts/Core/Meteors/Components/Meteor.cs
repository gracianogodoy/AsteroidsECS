using Unity.Entities;

namespace Asteroids.Core
{
    public struct Meteor : IComponentData
    {
        public MeteorSize Size;

        public enum MeteorSize
        {
            Small,
            Medium,
            Big
        }
    }
}