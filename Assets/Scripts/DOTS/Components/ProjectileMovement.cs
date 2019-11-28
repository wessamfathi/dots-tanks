using Unity.Entities;

public struct ProjectileSpeedComponent : IComponentData
{
	public float MetersPerSecond;
}

public struct ProjectileLifetimeComponent : IComponentData
{
	public float Seconds;
}