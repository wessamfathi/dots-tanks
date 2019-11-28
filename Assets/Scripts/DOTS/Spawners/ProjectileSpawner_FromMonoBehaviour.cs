using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ProjectileSpawner_FromMonoBehaviour : MonoBehaviour
{
	public GameObject Prefab;
	public AudioSource SFX;
	public ParticleSystem VFX;

	private void Start()
	{
		// Create entity prefab from the game object hierarchy once
		var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
		var entityManager = World.Active.EntityManager;

		// Efficiently instantiate a bunch of entities from the already converted entity prefab
		var instance = entityManager.Instantiate(prefab);

		// Place the instantiated entity in a grid with some noise
		var position = transform.TransformPoint(new float3(1.3F, noise.cnoise(new float2(1, 1) * 0.21F) * 2, 1.3F));
		entityManager.SetComponentData(instance, new Translation { Value = position });
		entityManager.AddComponentData(instance, new ProjectileSpeedComponent { MetersPerSecond = 1.0f });
		entityManager.AddComponentData(instance, new ProjectileLifetimeComponent { Seconds = 2.0f });
		entityManager.AddComponentData(instance, new ProjectileDamageComponent { Force = new float3(1.0f), MaxDamage = 2.0f, Radius = 2.0f, Health = 100 });

		entityManager.AddSharedComponentData(instance, new ProjectileExplosionAudioComponent { m_ExplosionAudio = SFX });
		entityManager.AddSharedComponentData(instance, new ProjectileExplosionParticleComponent { m_ExplosionParticles = VFX });
	}
}
