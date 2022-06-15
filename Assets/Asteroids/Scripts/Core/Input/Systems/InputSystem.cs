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

            if (Input.GetKey(KeyCode.D))
            {
                var e = EntityManager.CreateEntity(typeof(RotateInput));
                EntityManager.SetComponentData(e, new RotateInput() { Value = RotateInput.Direction.Right });
            }

            if (Input.GetKey(KeyCode.A))
            {
                var e = EntityManager.CreateEntity(typeof(RotateInput));
                EntityManager.SetComponentData(e, new RotateInput() { Value = RotateInput.Direction.Left });
            }
        }
    }
}