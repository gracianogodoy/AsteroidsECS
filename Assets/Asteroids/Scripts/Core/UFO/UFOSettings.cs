using UnityEngine;

namespace Asteroids.Core
{
    [CreateAssetMenu(menuName = "Custom/Settings/UFO", fileName = "UFOSettings")]
    public class UFOSettings : ScriptableObject
    {
        public byte ColorID = 3;

        public float MinSpeed = 1;
        public float MaxSpeed = 2;

        public float SmallSizeFactor = 0.2f;
        public float BigSizeFactor = 1f;

        public float SpawnCooldown = 10;
        public float ShootCooldown = 2;
        public float BulletSpeed = 10;
        public byte BulletColorID = 4;
    }
}