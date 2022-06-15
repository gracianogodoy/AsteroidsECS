using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Asteroids.Core
{
    public class LineViewRenderer : MonoBehaviour
    {
        [SerializeField]
        private Color[] colors;

        private Material lineMaterial;
        private EntityQuery query;
        private EntityManager entityManager;

        private void Start()
        {
            SetupLineMaterial();

            var world = World.DefaultGameObjectInjectionWorld;
            entityManager = world.EntityManager;

            query = entityManager.CreateEntityQuery(typeof(Points));

            Camera.onPostRender += OnPostRenderCallback;
        }

        private void OnDestroy()
        {
            Camera.onPostRender -= OnPostRenderCallback;
        }

        private void SetupLineMaterial()
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        private void OnPostRenderCallback(Camera cam)
        {
            var entities = query.ToEntityArray(Allocator.Temp).AsReadOnly();

            DrawLines(entities);
        }

        private void DrawLines(NativeArray<Entity>.ReadOnly entities)
        {
            lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);

            GL.Begin(GL.LINES);
            foreach (var e in entities.ToArray())
            {
                var position = entityManager.GetComponentData<Position>(e);
                var colorID = entityManager.GetComponentData<ColorID>(e);
                var points = entityManager.GetBuffer<Points>(e);

                GL.Color(colors[colorID.Value]);

                if (points.Length <= 1)
                    continue;

                for (int i = 0; i < points.Length; i++)
                {
                    var currentPoint = points[i].Value + position.Value;
                    float2 nextPoint;

                    if (i == points.Length - 1)
                        nextPoint = points[0].Value + position.Value;
                    else
                        nextPoint = points[i + 1].Value + position.Value;

                    GL.Vertex(new Vector3(currentPoint.x, currentPoint.y));

                    GL.Vertex(new Vector3(nextPoint.x, nextPoint.y));
                }
            }

            GL.End();

            GL.PopMatrix();
        }
    }
}