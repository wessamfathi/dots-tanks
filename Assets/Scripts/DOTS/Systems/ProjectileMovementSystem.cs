﻿using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

[UpdateAfter(typeof(TankInputSystem))]
public class ProjectileMovementSystem : JobComponentSystem
{
	[BurstCompile]
	struct ProjectileMovementJob : IJobForEach<ProjectileSpeedComponent, Translation, Rotation>
	{
		public float DeltaTime;

		public void Execute(ref ProjectileSpeedComponent speed, ref Translation translation, ref Rotation rotation)
		{
			translation.Value += math.forward(rotation.Value) * speed.MetersPerSecond * DeltaTime;
		}
	}

	[BurstCompile]
	struct ProjectileLifetimeJob : IJobForEach<ProjectileLifetimeComponent>
	{
		public float DeltaTime;

		public void Execute(ref ProjectileLifetimeComponent lifetime)
		{
			lifetime.Seconds -= DeltaTime;

			if (lifetime.Seconds <= 0)
			{
				//EntityManager.DestroyEntity();
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		inputDeps = new ProjectileMovementJob { DeltaTime = Time.deltaTime }.Schedule(this, inputDeps);
		inputDeps = new ProjectileLifetimeJob { DeltaTime = Time.deltaTime }.Schedule(this, inputDeps);

		return inputDeps;
	}
}