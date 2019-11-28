using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class ProjectileExplosionSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<ExplodingProjectileComponent, Translation>().ForEach(
            (Entity entity, ref Translation translation) =>
            {
                // TODO: spawn FX at translation
            });
    }
}