using Unity.Mathematics;
using UnityEngine;

namespace Asteroids.Core
{
    [CreateAssetMenu(menuName = "Custom/Settings/Ship", fileName = "ShipSettings")]
    public class ShipSettings : ScriptableObject
    {
        [SerializeField]
        public float2 StartingPosition;

        [SerializeField]
        public byte ColorID = 0;

        [SerializeField]
        public float SizeFactor = 0.5f;

        [SerializeField]
        public float RotationSpeed = 30;

        [SerializeField]
        public float Acceleration = 20;

        [SerializeField]
        public float MaxSpeed = 30;
    }
}