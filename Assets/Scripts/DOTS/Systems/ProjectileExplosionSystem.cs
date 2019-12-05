using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using System.Collections.Generic;

public class ProjectileExplosionSystem : ComponentSystem
{
    private List<ProjectileExplosionAudioComponent> ExplosionSEffects;
    private List<ProjectileExplosionParticleComponent> ExplosionVEffects;

    protected override void OnCreate()
    {
        ExplosionSEffects = new List<ProjectileExplosionAudioComponent>();
        ExplosionVEffects = new List<ProjectileExplosionParticleComponent>();
    }

    protected override void OnUpdate()
    {
        ExplosionSEffects.Clear();
        EntityManager.GetAllUniqueSharedComponentData(ExplosionSEffects);

        ExplosionVEffects.Clear();
        EntityManager.GetAllUniqueSharedComponentData(ExplosionVEffects);

        Entities.WithAll<ProjectileLifetimeComponent, ProjectileDamageComponent, Translation>().ForEach(
            (Entity entity, ref ProjectileLifetimeComponent lifetime, ref ProjectileDamageComponent projectile, ref Translation translation) =>
            {
                if (projectile.Health <= 0 || lifetime.Seconds <= 0)
                {
                    // spawn SFX & VFX at projectile position
                    var explosionAudio = ExplosionSEffects[1].m_ExplosionAudio;
                    var explosionParticles = ExplosionVEffects[1].m_ExplosionParticles;

                    explosionAudio.transform.position = new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);
                    explosionAudio.Play();

                    explosionParticles.transform.position = new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);
                    explosionParticles.Play();

                    PostUpdateCommands.DestroyEntity(entity);
                }
            });
    }
}