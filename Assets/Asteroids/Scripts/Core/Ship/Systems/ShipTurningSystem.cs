using Unity.Entities;

namespace Asteroids.Core
{
    public class ShipTurningSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var shipSettings = Settings.Instance.Ship;
            var deltaTime = Time.DeltaTime;

            var rotateInputQuery = GetEntityQuery(typeof(RotateInput));

            if (rotateInputQuery.IsEmpty)
                return;

            var rotateInputEntity = rotateInputQuery.GetSingletonEntity();
            var rotateInput = GetComponent<RotateInput>(rotateInputEntity);

            var shipEntity = GetEntityQuery(typeof(Ship)).GetSingletonEntity();
            var rotateAngle = shipSettings.RotationSpeed * (int)rotateInput.Value * deltaTime;
            EntityManager.AddComponentData(shipEntity, new Rotate() { Degrees = rotateAngle });

            EntityManager.DestroyEntity(rotateInputEntity);
        }
    }
}