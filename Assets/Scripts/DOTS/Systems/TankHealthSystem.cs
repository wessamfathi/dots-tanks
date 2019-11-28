using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

public class TankHealthSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.WithAll<TankHealthComponent>().ForEach(
			 (Entity entity, ref TankHealthComponent tankHealth) =>
			 {
				 if (tankHealth.Health <= 0)
				 {
					 PostUpdateCommands.DestroyEntity(entity);
				 }
			 });
	}
}
