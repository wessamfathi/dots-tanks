using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ProjectileSpawner_FromMonoBehaviour : MonoBehaviour
{
	public GameObject Prefab;
	public AudioSource SFX;
	public ParticleSystem VFX;

	public float Speed;
	public float Lifetime;
	public float MaxDamage;
	public float Radius;
	public int Health;
	public float3 Force;

	private void Start()
	{
		// Create entity prefab from the game object hierarchy once
		var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
		var entityManager = World.Active.EntityManager;

		var instance = entityManager.CreateEntity();

		entityManager.AddComponentData(instance, new ProjectileSpawnDataComponent { 
			Speed = Speed,
			Lifetime = Lifetime, 
			MaxDamage = MaxDamage, 
			Radius = Radius, 
			Force = Force, 
			Health = Health,
			Prefab = prefab
		});

        entityManager.AddSharedComponentData(instance, new ProjectileExplosionAudioComponent { m_ExplosionAudio = SFX });
        entityManager.AddSharedComponentData(instance, new ProjectileExplosionParticleComponent { m_ExplosionParticles = VFX });
    }
}
