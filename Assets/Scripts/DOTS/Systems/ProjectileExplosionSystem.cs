using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using System.Collections.Generic;

public class ProjectileExplosionSystem : ComponentSystem
{
    private List<ProjectileExplosionEffectsComponent> ExplosionEffects;

    protected override void OnCreate()
    {
        ExplosionEffects = new List<ProjectileExplosionEffectsComponent>();
    }

    protected override void OnUpdate()
    {
        ExplosionEffects.Clear();
        EntityManager.GetAllUniqueSharedComponentData(ExplosionEffects);

        Entities.WithAll<ProjectileDamageComponent, Translation>().ForEach(
            (Entity entity, ref ProjectileDamageComponent projectile, ref Translation translation) =>
            {
                if (projectile.Health <= 0)
                {
                    // spawn SFX & VFX at projectile position
                    var explosionAudio = ExplosionEffects[1].m_ExplosionAudio;
                    var explosionParticles = ExplosionEffects[1].m_ExplosionParticles;

                    explosionAudio.transform.position = new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);
                    explosionAudio.Play();

                    explosionParticles.transform.position = new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);
                    explosionParticles.Play();

                    PostUpdateCommands.DestroyEntity(entity);
                }
            });
    }
}