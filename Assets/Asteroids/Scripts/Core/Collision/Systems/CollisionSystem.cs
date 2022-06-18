using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Core
{
    public class CollisionSystem : SystemBase
    {
        private EntityCommandBufferSystem commandBufferSystem;

        protected override void OnCreate()
        {
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var collidables = GetEntityQuery(typeof(Collidable)).ToEntityArray(Allocator.TempJob);
            var bufferFromEntity = GetBufferFromEntity<Points>();
            var commandBuffer = commandBufferSystem.CreateCommandBuffer();

            Job.WithCode(() =>
            {
                for (int i = 0; i < collidables.Length; i++)
                {
                    var entity = collidables[i];
                    var entityPointsBuffer = bufferFromEntity[entity];
                    var position = GetComponent<Position>(entity);

                    for (int o = 0; o < collidables.Length; o++)
                    {
                        var otherEntity = collidables[o];
                        if (entity == otherEntity)
                            continue;

                        var otherEntityPointsBuffer = bufferFromEntity[otherEntity];
                        var otherPosition = GetComponent<Position>(otherEntity);

                        for (int w = 0; w < entityPointsBuffer.Length; w++)
                        {
                            var intersectionAmount = 0;
                            var pointWorldPosition = position.Value + entityPointsBuffer[w].Value;

                            for (int l = 0; l < otherEntityPointsBuffer.Length; l++)
                            {
                                var currentPoint = otherEntityPointsBuffer[l].Value + otherPosition.Value;
                                float2 nextPoint;

                                if (l == otherEntityPointsBuffer.Length - 1)
                                    nextPoint = otherEntityPointsBuffer[0].Value + otherPosition.Value;
                                else
                                    nextPoint = otherEntityPointsBuffer[l + 1].Value + otherPosition.Value;

                                var intersect = IsLineIntersecting(pointWorldPosition, new float2(100, pointWorldPosition.y),
                                     currentPoint, nextPoint);

                                if (intersect)
                                    intersectionAmount++;
                            }

                            if (intersectionAmount % 2 != 0)
                            {
                                commandBuffer.AddComponent(entity, new IsColliding() { OtherEntity = otherEntity });
                                commandBuffer.AddComponent(otherEntity, new IsColliding() { OtherEntity = entity });
                            }
                        }
                    }
                }
            }).Schedule();

            this.CompleteDependency();

            collidables.Dispose();
        }

        public static bool IsLineIntersecting(float2 p1start, float2 p1end, float2 p2start, float2 p2end)
        {
            var p = p1start;
            var r = p1end - p1start;
            var q = p2start;
            var s = p2end - p2start;
            var qminusp = q - p;

            float cross_rs = CrossProduct2D(r, s);

            if (Approximately(cross_rs, 0f))
            {
                if (Approximately(CrossProduct2D(qminusp, r), 0f))
                {
                    float rdotr = math.dot(r, r);
                    float sdotr = math.dot(s, r);
                    float t0 = math.dot(qminusp, r / rdotr);
                    float t1 = t0 + sdotr / rdotr;
                    if (sdotr < 0)
                    {
                        Swap(ref t0, ref t1);
                    }

                    if (t0 <= 1 && t1 >= 0)
                    {
                        float t = math.lerp(math.max(0, t0), math.min(1, t1), 0.5f);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                float t = CrossProduct2D(qminusp, s) / cross_rs;
                float u = CrossProduct2D(qminusp, r) / cross_rs;
                if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static bool Approximately(float a, float b, float tolerance = 1e-5f)
        {
            return math.abs(a - b) <= tolerance;
        }

        public static float CrossProduct2D(float2 a, float2 b)
        {
            return a.x * b.y - b.x * a.y;
        }
    }
}