using UnityEngine;

namespace Asteroids.Core
{
    [CreateAssetMenu(menuName = "Custom/Settings/Powerups", fileName = "PowerupsSettings")]
    public class PowerupsSettings : ScriptableObject
    {
        public float SpawnCooldown = 10;
        public float Speed = 1;
    }
}