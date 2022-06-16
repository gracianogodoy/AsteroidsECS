using Unity.Entities;

namespace Asteroids.Core
{
    public class WarpSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.WithAll<Warpable>().ForEach((ref Position position) =>
            {
                if (position.Value.x > ScreenHelper.MaxBoundX)
                {
                    position.Value.x = ScreenHelper.MinBoundX;
                }

                if (position.Value.x < ScreenHelper.MinBoundX)
                {
                    position.Value.x = ScreenHelper.MaxBoundX;
                }

                if (position.Value.y < ScreenHelper.MinBoundY)
                {
                    position.Value.y = ScreenHelper.MaxBoundY;
                }

                if (position.Value.y > ScreenHelper.MaxBoundY)
                {
                    position.Value.y = ScreenHelper.MinBoundY;
                }
            }).Schedule();
        }
    }
}