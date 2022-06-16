using Unity.Mathematics;
using UnityEngine;

namespace Asteroids.Core
{
    [CreateAssetMenu(menuName = "Custom/Settings/Ship", fileName = "ShipSettings")]
    public class ShipSettings : ScriptableObject
    {
        public float2 StartingPosition;

        public byte ColorID = 0;

        public float SizeFactor = 0.5f;

        public float RotationSpeed = 30;

        public float Acceleration = 20;

        public float MaxSpeed = 30;

        public float ShootCooldown = 0.2f;

        public float BulletSpeed = 10;

        public byte BulletColorID = 3;
    }
}