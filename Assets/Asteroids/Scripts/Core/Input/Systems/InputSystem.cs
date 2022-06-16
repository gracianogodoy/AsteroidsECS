using Unity.Entities;
using UnityEngine;

namespace Asteroids.Core
{
    [AlwaysUpdateSystem]
    public class InputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Input.GetKey(KeyCode.W))
            {
                EntityManager.CreateEntity(typeof(MoveFowardInput));
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                var e = EntityManager.CreateEntity(typeof(StartTurningInput));
                EntityManager.SetComponentData(e, new StartTurningInput() { Direction = StartTurningInput.TurningDirection.Right });
            }
            else
            if (Input.GetKeyDown(KeyCode.A))
            {
                var e = EntityManager.CreateEntity(typeof(StartTurningInput));
                EntityManager.SetComponentData(e, new StartTurningInput() { Direction = StartTurningInput.TurningDirection.Left });
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                EntityManager.CreateEntity(typeof(StopTurningInput));
            }

            if (Input.GetKey(KeyCode.Space))
            {
                EntityManager.CreateEntity(typeof(ShootInput));
            }
        }
    }
}