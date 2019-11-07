using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

public class TankInputSystem : JobComponentSystem
{
	[BurstCompile]
	struct TankInputJob : IJobForEach<TankInputComponent>
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
		var job = new TankInputJob
		{
			DeltaTime = Time.deltaTime,
			MovementInputValue = Input.GetAxis("Vertical1"),
			TurnInputValue = Input.GetAxis("Horizontal1")
		};

		inputDeps = job.Schedule(this, inputDeps);
		return inputDeps;
	}
}
