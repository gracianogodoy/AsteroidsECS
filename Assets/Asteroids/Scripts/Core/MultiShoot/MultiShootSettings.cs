using UnityEngine;

namespace Asteroids.Core
{
    [CreateAssetMenu(menuName = "Custom/Settings/MultiShoot", fileName = "MultiShootSettings")]
    public class MultiShootSettings : ScriptableObject
    {
        public int MaxShootAmount = 5;
    }
}