using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class ShipHyperspaceSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var shipQuery = GetEntityQuery(typeof(Ship));
            var hyperspaceInputQuery = GetEntityQuery(typeof(HypertravelInput));

            if (shipQuery.IsEmpty)
            {
                if (!hyperspaceInputQuery.IsEmpty)
                    EntityManager.DestroyEntity(hyperspaceInputQuery.GetSingletonEntity());

                return;
            }

            if (hyperspaceInputQuery.IsEmpty)
                return;

            var x = UnityEngine.Random.Range(ScreenHelper.MinBoundX, ScreenHelper.MaxBoundX);
            var y = UnityEngine.Random.Range(ScreenHelper.MinBoundY, ScreenHelper.MaxBoundY);

            EntityManager.SetComponentData(shipQuery.GetSingletonEntity(), new Position() { Value = new float2(x, y) });

            EntityManager.DestroyEntity(hyperspaceInputQuery.GetSingletonEntity());
        }
    }
}