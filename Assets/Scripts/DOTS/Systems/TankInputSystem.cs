using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

public class TankInputSystem : JobComponentSystem
{
	[BurstCompile]
	[RequireComponentTag(typeof(Tank1))]
	struct Tank1InputJob : IJobForEach<TankInputComponent>
	{
		public float DeltaTime;
		public float MovementInputValue;
		public float TurnInputValue;

		public void Execute(ref TankInputComponent input)
		{
			input.MovementInputValue = MovementInputValue;
			input.TurnInputValue = TurnInputValue;
		}
	}

	[BurstCompile]
	[RequireComponentTag(typeof(Tank2))]
	struct Tank2InputJob : IJobForEach<TankInputComponent>
	{
		public float DeltaTime;
		public float MovementInputValue;
		public float TurnInputValue;

		public void Execute(ref TankInputComponent input)
		{
			input.MovementInputValue = MovementInputValue;
			input.TurnInputValue = TurnInputValue;
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var job1 = new Tank1InputJob
		{
			DeltaTime = Time.deltaTime,
			MovementInputValue = Input.GetAxis("Vertical1"),
			TurnInputValue = Input.GetAxis("Horizontal1")
		};

		var job2 = new Tank2InputJob
		{
			DeltaTime = Time.deltaTime,
			MovementInputValue = Input.GetAxis("Vertical2"),
			TurnInputValue = Input.GetAxis("Horizontal2")
		};

		inputDeps = job1.Schedule(this, inputDeps);
		inputDeps = job2.Schedule(this, inputDeps);
		return inputDeps;
	}
}
