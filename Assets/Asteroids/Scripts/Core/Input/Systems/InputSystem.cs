using Unity.Entities;
using UnityEngine;

namespace Asteroids.Core
{
    [AlwaysUpdateSystem]
    public class InputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var shipQuery = GetEntityQuery(typeof(Ship));
            if (shipQuery.IsEmpty)
                return;

            if (Input.GetKey(KeyCode.W))
            {
                EntityManager.CreateEntity(typeof(MoveFowardInput), typeof(Resetable));
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                var e = EntityManager.CreateEntity(typeof(StartTurningInput), typeof(Resetable));
                EntityManager.SetComponentData(e, new StartTurningInput() { Direction = StartTurningInput.TurningDirection.Right });
            }
            else
            if (Input.GetKeyDown(KeyCode.A))
            {
                var e = EntityManager.CreateEntity(typeof(StartTurningInput), typeof(Resetable));
                EntityManager.SetComponentData(e, new StartTurningInput() { Direction = StartTurningInput.TurningDirection.Left });
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                EntityManager.CreateEntity(typeof(StopTurningInput), typeof(Resetable));
            }

            if (Input.GetKey(KeyCode.Space))
            {
                EntityManager.CreateEntity(typeof(ShootInput), typeof(Resetable));
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                EntityManager.CreateEntity(typeof(HypertravelInput), typeof(Resetable));
            }
        }
    }
}