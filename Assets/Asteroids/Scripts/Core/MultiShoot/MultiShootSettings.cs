using UnityEngine;

namespace Asteroids.Core
{
    [CreateAssetMenu(menuName = "Custom/Settings/MultiShoot", fileName = "MultiShootSettings")]
    public class MultiShootSettings : ScriptableObject
    {
        public byte ColorID = 0;
        public float sizeFactor = 1;
        public int MaxShootAmount = 5;
    }
}