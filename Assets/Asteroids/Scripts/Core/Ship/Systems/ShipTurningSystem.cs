using Unity.Entities;

namespace Asteroids.Core
{
    public class ShipTurningSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var shipSettings = Settings.Instance.Ship;

            var startTurningInputQuery = GetEntityQuery(typeof(StartTurningInput));
            var stopTurningInputQuery = GetEntityQuery(typeof(StopTurningInput));

            var shipEntity = GetEntityQuery(typeof(Ship)).GetSingletonEntity();

            if (!stopTurningInputQuery.IsEmpty)
            {
                EntityManager.RemoveComponent<RotateSpeed>(shipEntity);

                var stopTurningInputEntity = stopTurningInputQuery.GetSingletonEntity();
                EntityManager.DestroyEntity(stopTurningInputEntity);
            }

            if (!startTurningInputQuery.IsEmpty && !HasComponent<RotateSpeed>(shipEntity))
            {
                var startTurningInputEntity = startTurningInputQuery.GetSingletonEntity();
                var turningInput = GetComponent<StartTurningInput>(startTurningInputEntity);

                var rotateAngle = shipSettings.RotationSpeed * -(int)turningInput.Direction;
                EntityManager.AddComponentData(shipEntity, new RotateSpeed() { Value = rotateAngle });

                EntityManager.DestroyEntity(startTurningInputEntity);
            }
        }
    }
}