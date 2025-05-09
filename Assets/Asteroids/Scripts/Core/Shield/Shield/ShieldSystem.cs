using Unity.Entities;

namespace Asteroids.Core
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial class ShieldSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var shieldQuery = GetEntityQuery(typeof(Shield));
            var shipQuery = GetEntityQuery(typeof(Ship));

            if (shipQuery.IsEmpty)
                return;

            if (shieldQuery.IsEmpty)
                return;

            var shieldEntity = shieldQuery.GetSingletonEntity();

            var shipPosition = GetComponent<Position>(shipQuery.GetSingletonEntity());

            EntityManager.SetComponentData(shieldEntity, new Position { Value = shipPosition.Value });
        }
    }
}