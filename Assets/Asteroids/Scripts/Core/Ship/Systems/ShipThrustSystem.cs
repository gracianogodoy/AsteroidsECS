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
            var shipQuery = GetEntityQuery(typeof(Ship));

            if (shipQuery.IsEmpty)
            {
                if (!moveFowardQuery.IsEmpty)
                    EntityManager.DestroyEntity(moveFowardQuery.GetSingletonEntity());

                return;
            }

            if (moveFowardQuery.IsEmpty)
                return;

            var shipEntity = shipQuery.GetSingletonEntity();

            var velocity = GetComponent<Velocity>(shipEntity);
            var facingDirection = GetComponent<FacingDirection>(shipEntity);

            var finalAcceleration = facingDirection.Value * shipSettings.Acceleration * deltaTime;

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
            var normalizedDirection = new float2(value.x / magnitude, value.y / magnitude);

            if (magnitude >= maxSpeed)
            {
                value = normalizedDirection * maxSpeed;
            }

            return value;
        }
    }
}