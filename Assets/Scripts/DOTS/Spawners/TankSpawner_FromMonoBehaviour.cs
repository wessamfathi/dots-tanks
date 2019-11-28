using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TankSpawner_FromMonoBehaviour : MonoBehaviour
{
	public GameObject Prefab;
	public float Health;
	public float Speed;
	public float TurnSpeed;

	void Start()
	{
		// Create entity prefab from the game object hierarchy once
		var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
		var entityManager = World.Active.EntityManager;

		// Efficiently instantiate a bunch of entities from the already converted entity prefab
		var instance = entityManager.Instantiate(prefab);

		// Place the instantiated entity in a grid with some noise
		var position = transform.TransformPoint(new float3(1.3F, noise.cnoise(new float2(1, 1) * 0.21F) * 2, 1.3F));
		entityManager.SetComponentData(instance, new Translation { Value = position });
		entityManager.AddComponent(instance, typeof(TankInputComponent));
		entityManager.AddComponentData(instance, new TankSpeedComponent { Speed = Speed, TurnSpeed = TurnSpeed });
		entityManager.AddComponentData(instance, new TankHealthComponent { Health = Health });
	}
}
