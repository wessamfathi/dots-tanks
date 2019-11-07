using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateAfter(typeof(TankInputSystem))]
public class TankMovementSystem : JobComponentSystem
{
	[BurstCompile]
	struct TankMovementJob : IJobForEach<TankInputComponent, TankSpeedComponent, Translation, Rotation>
	{
		public float DeltaTime;

		public void Execute(ref TankInputComponent input, ref TankSpeedComponent speed, ref Translation translation, ref Rotation rotation)
		{
			float turn = input.TurnInputValue * speed.TurnSpeed * DeltaTime;
			quaternion turnRotation = quaternion.Euler(0, turn, 0);
			rotation.Value = math.mul(rotation.Value, turnRotation.value);

			translation.Value += math.forward(rotation.Value) * input.MovementInputValue * speed.Speed * DeltaTime;
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		var job = new TankMovementJob
		{
			DeltaTime = Time.deltaTime
		};

		inputDeps = job.Schedule(this, inputDeps);
		return inputDeps;
	}
}
