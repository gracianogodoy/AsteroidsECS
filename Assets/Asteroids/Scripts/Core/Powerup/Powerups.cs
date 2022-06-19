using System;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public static class Powerups
    {
        public static PowerupConfig[] Configs =
            {
                new PowerupConfig()
                {
                    ComponentType = typeof(MultiShootPowerup),
                    ColorID = 5,
                    Points= new float2[]
                    {
                       new float2(-0.2f, 0.5f) * 0.3f,
                       new float2(0.2f, 0.5f) * 0.3f,
                       new float2(0.2f, 0.2f) * 0.3f,
                       new float2(0.5f, 0.2f) * 0.3f,
                       new float2(0.5f, -0.2f) * 0.3f,
                       new float2(0.2f, -0.2f) * 0.3f,
                       new float2(0.2f, -0.5f) * 0.3f,
                       new float2(-0.2f, -0.5f) * 0.3f,
                       new float2(-0.2f, -0.2f) * 0.3f,
                       new float2(-0.5f, -0.2f) * 0.3f ,
                       new float2(-0.5f, 0.2f) * 0.3f,
                       new float2(-0.2f, 0.2f) * 0.3f
                    }
                },
                 new PowerupConfig()
                {
                    ComponentType = typeof(ShieldPowerup),
                    ColorID = 5,
                    Points= new float2[]
                    {
                        new float2(-0.19f, 0.5f) * 0.3f,
                        new float2(0.19f, 0.5f) * 0.3f,
                        new float2(0.5f, 0.19f) * 0.3f,
                        new float2(0.5f, -0.19f) * 0.3f,
                        new float2(0.19f, -0.5f) * 0.3f,
                        new float2(-0.19f, -0.5f) * 0.3f,
                        new float2(-0.5f, -0.19f) * 0.3f,
                        new float2(-0.5f, 0.19f) * 0.3f,
                    }
                }
        };

        public struct PowerupConfig
        {
            public Type ComponentType;
            public float2[] Points;
            public byte ColorID;
        }
    }
}