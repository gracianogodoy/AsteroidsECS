using UnityEngine;

namespace Asteroids.Core
{
    [CreateAssetMenu(menuName = "Custom/Settings/Meteor", fileName = "MeteorSettings")]
    public class MeteorSettings : ScriptableObject
    {
        public byte ColorID = 1;

        public int SpawnAmount = 10;

        public float MinSpeed = 1;
        public float MaxSpeed = 2;

        public float SmallSizeFactor = 0.2f;
        public float MediumSizeFactor = 0.5f;
        public float BigSizeFactor = 1f;

        public int MinRotateSpeed = 60;
        public int MaxRotateSpeed = 90;
    }
}