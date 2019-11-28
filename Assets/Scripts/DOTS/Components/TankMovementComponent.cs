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

public struct Tank1 : IComponentData
{

}

public struct Tank2 : IComponentData
{

}

public struct TankHealthComponent : IComponentData
{
	public float Health;
}