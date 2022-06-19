using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class CreateMultiShootSystem : SystemBase
    {
        private MultiShootSettings settings;

        protected override void OnUpdate()
        {
            settings = Settings.Instance.MultiShoot;

            var doCreatePowerupQuery = GetEntityQuery(typeof(DoCreatePowerup));

            if (doCreatePowerupQuery.IsEmpty)
                return;

            var doCreateEntity = doCreatePowerupQuery.GetSingletonEntity();
            var doCreate = GetComponent<DoCreatePowerup>(doCreateEntity);
            Create(doCreate);

            EntityManager.DestroyEntity(doCreatePowerupQuery.GetSingletonEntity());
        }

        public void Create(DoCreatePowerup doCreatePowerup)
        {
            var entity = EntityManager.CreateEntity(typeof(Powerup), typeof(MultiShoot), typeof(Velocity), typeof(MustDestroyOutsideScreen), typeof(Collidable));

            EntityManager.SetComponentData(entity, new Velocity() { Value = doCreatePowerup.Velocity });

            var sizeFactor = settings.sizeFactor;

            EntityCreationHelper.AddBaseComponents(entity,
               EntityManager,
               doCreatePowerup.Position,
               settings.ColorID,
               new float2(-0.2f, 0.5f) * sizeFactor,
               new float2(0.2f, 0.5f) * sizeFactor,
               new float2(0.2f, 0.2f) * sizeFactor,
               new float2(0.5f, 0.2f) * sizeFactor,
               new float2(0.5f, -0.2f) * sizeFactor,
               new float2(0.2f, -0.2f) * sizeFactor,
               new float2(0.2f, -0.5f) * sizeFactor,
               new float2(-0.2f, -0.5f) * sizeFactor,
               new float2(-0.2f, -0.2f) * sizeFactor,
               new float2(-0.5f, -0.2f) * sizeFactor,
               new float2(-0.5f, 0.2f) * sizeFactor,
               new float2(-0.2f, 0.2f) * sizeFactor
               );
        }
    }
}