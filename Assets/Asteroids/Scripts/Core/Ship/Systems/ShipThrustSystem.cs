using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class ShipThrustSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var shipSettings = Settings.Instance.Ship;

            var moveFowardQuery = GetEntityQuery(typeof(MoveFowardInput));

            if (moveFowardQuery.IsEmpty)
                return;

            var shipEntity = GetEntityQuery(typeof(Ship)).GetSingletonEntity();

            var rotation = GetComponent<Rotation>(shipEntity);
            var velocity = GetComponent<Velocity>(shipEntity);

            var rotationValue = rotation.Value + math.radians(90);

            var direction = new float2(math.cos(rotationValue), math.sin(rotationValue));

            var finalAcceleration = direction * shipSettings.Acceleration * deltaTime;

            velocity.Value += finalAcceleration;

            velocity.Value = ClampMagnitude(velocity.Value, shipSettings.MaxSpeed);

            EntityManager.SetComponentData(shipEntity, velocity);
            EntityManager.DestroyEntity(moveFowardQuery.GetSingletonEntity());
        }

        private float2 ClampMagnitude(float2 value, float maxSpeed)
        {
            var pwrX = value.x * value.x;
            var pwrY = value.y * value.y;
            var magnitude = math.sqrt(pwrX + pwrY);
            var direction = new float2(value.x / magnitude, value.y / magnitude);

            if (magnitude >= maxSpeed)
            {
                value = direction * maxSpeed;
            }

            return value;
        }
    }
}