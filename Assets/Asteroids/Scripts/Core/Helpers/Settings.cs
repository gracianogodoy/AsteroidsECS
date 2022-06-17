using UnityEngine;

namespace Asteroids.Core
{
    public class Settings : MonoBehaviour
    {
        public ShipSettings Ship;
        public MeteorSettings Meteor;
        public UFOSettings UFO;

        private static Settings instance;

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<Settings>();

                return instance;
            }
        }
    }
}