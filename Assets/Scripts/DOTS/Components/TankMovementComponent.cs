using Unity.Entities;

public struct TankSpeedComponent : IComponentData
{
	public float Speed;
	public float TurnSpeed;
}

public struct TankInputComponent : IComponentData
{
	public float MovementInputValue;
	public float TurnInputValue;
}

public struct TankHealthComponent : IComponentData
{
	public float Health;
}