using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class TankInputSystem : ComponentSystem
{
    private float[] LastFireTime = { 0, 0 };
    const float FireThreshold = 1.0f;

    void UpdateInput<T>(float movement, float turn, float fire, int index)
    {
        Entities.WithAll<TankInputComponent, Translation, Rotation, T>().ForEach(
             (Entity entity, ref TankInputComponent input, ref Translation translation, ref Rotation rotation) =>
             {
                 input.MovementInputValue = movement;
                 input.TurnInputValue = turn;

                 if (fire > 0)
                 {
                     if (Time.time - LastFireTime[index] > FireThreshold)
                     {
                         LastFireTime[index] = Time.time;
                         var translationValue = translation.Value;
                         var rotationValue = rotation.Value;

                         Entities.WithAll<ProjectileSpawnDataComponent>().ForEach(
                             (Entity projectileEntity, ref ProjectileSpawnDataComponent spawnData) =>
                             {
                                 var entityManager = World.Active.EntityManager;

                                 var instance = entityManager.Instantiate(spawnData.Prefab);

                                 entityManager.SetComponentData(instance, new Translation { Value = translationValue + spawnData.DeltaPosition });
                                 entityManager.SetComponentData(instance, new Rotation { Value = rotationValue });
                                 entityManager.AddComponentData(instance, new ProjectileSpeedComponent { MetersPerSecond = spawnData.Speed });
                                 entityManager.AddComponentData(instance, new ProjectileLifetimeComponent { Seconds = spawnData.Lifetime });
                                 entityManager.AddComponentData(instance, new ProjectileDamageComponent
                                 {
                                     Force = spawnData.Force,
                                     MaxDamage = spawnData.MaxDamage,
                                     Radius = spawnData.Radius,
                                     Health = spawnData.Health
                                 });

                                 entityManager.AddComponent<T>(instance);
                             });
                     }
                 }
             });
    }

    protected override void OnUpdate()
    {
        UpdateInput<Tank1>(Input.GetAxis("Vertical1"), Input.GetAxis("Horizontal1"), Input.GetAxis("Fire1"), 0);
        UpdateInput<Tank2>(Input.GetAxis("Vertical2"), Input.GetAxis("Horizontal2"), Input.GetAxis("Fire2"), 1);
    }
}
