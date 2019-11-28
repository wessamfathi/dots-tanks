using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using System.Collections.Generic;

public class ProjectileExplosionSystem : ComponentSystem
{
    private List<ProjectileExplosionAudioComponent> SFXs;
    private List<ProjectileExplosionParticleComponent> VFXs;

    protected override void OnCreate()
    {
        SFXs = new List<ProjectileExplosionAudioComponent>();
        VFXs = new List<ProjectileExplosionParticleComponent>();
    }

    protected override void OnUpdate()
    {
        SFXs.Clear();
        EntityManager.GetAllUniqueSharedComponentData(SFXs);

        VFXs.Clear();
        EntityManager.GetAllUniqueSharedComponentData(VFXs);

        Entities.WithAll<ProjectileDamageComponent, Translation>().ForEach(
            (Entity entity, ref ProjectileDamageComponent projectile, ref Translation translation) =>
            {
                if (projectile.Health <= 0)
                {
                    // spawn SFX & VFX at projectile position
                    var explosionAudio = SFXs[1].m_ExplosionAudio;
                    //var explosionParticles = VFXs[1].m_ExplosionParticles;

                    explosionAudio.transform.position = new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);
                    explosionAudio.Play();

                    //explosionParticles.transform.position = new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);
                    //explosionParticles.Play();

                    PostUpdateCommands.DestroyEntity(entity);
                }
            });
    }
}