using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileSpawnDataComponent : IComponentData
{
	public float Speed;
	public float Lifetime;
	public float3 Force;
    public float3 DeltaPosition;
    public float MaxDamage;
	public float Radius;
	public int Health;
	public Entity Prefab;
}
