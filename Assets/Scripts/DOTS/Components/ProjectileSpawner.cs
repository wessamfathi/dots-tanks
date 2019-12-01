using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileSpawnerComponent : IComponentData
{
	public float Speed;
	public float Lifetime;
	public float3 Force;
	public float MaxDamage;
	public float Radius;
	public int Health;
	public Entity Prefab;
}