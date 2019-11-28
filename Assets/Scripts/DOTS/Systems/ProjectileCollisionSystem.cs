using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

// This system applies an impulse to any dynamic that collides with a Repulsor.
// A Repulsor is defined by a PhysicsShapeAuthoring with the `Raise Collision Events` flag ticked and a
// CollisionEventImpulse behaviour added.
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ProjectileCollisionSystem : JobComponentSystem
{
	BuildPhysicsWorld m_BuildPhysicsWorldSystem;
	StepPhysicsWorld m_StepPhysicsWorldSystem;

	protected override void OnCreate()
	{
		m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
		m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
	}

	[BurstCompile]
	struct CollisionEventImpulseJob : ICollisionEventsJob
	{
		[ReadOnly] public ComponentDataFromEntity<ProjectileDamageComponent> ProjectileDamageGroup;
		public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;
		public ComponentDataFromEntity<TankHealthComponent> TankHealthGroup;

		public void Execute(CollisionEvent collisionEvent)
		{
			Entity entityA = collisionEvent.Entities.EntityA;
			Entity entityB = collisionEvent.Entities.EntityB;

			bool isBodyADynamic = PhysicsVelocityGroup.Exists(entityA);
			bool isBodyBDynamic = PhysicsVelocityGroup.Exists(entityB);

			bool isBodyAProjectile = ProjectileDamageGroup.Exists(entityA);
			bool isBodyBProjectile = ProjectileDamageGroup.Exists(entityB);

			if (isBodyAProjectile && isBodyBDynamic)
			{
				var projectileComponent = ProjectileDamageGroup[entityA];
				var velocityComponent = PhysicsVelocityGroup[entityB];
				var healthComponent = TankHealthGroup[entityB];

				// Use the impulse force to affect our tank's linear velocity
				velocityComponent.Linear = projectileComponent.Force;
				PhysicsVelocityGroup[entityB] = velocityComponent;

				// Apply damage to the tank
				healthComponent.Health -= projectileComponent.MaxDamage;
				TankHealthGroup[entityB] = healthComponent;
			}

			if (isBodyBProjectile && isBodyADynamic)
			{
				var projectileComponent = ProjectileDamageGroup[entityB];
				var velocityComponent = PhysicsVelocityGroup[entityA];
				var healthComponent = TankHealthGroup[entityA];

				// Use the impulse force to affect our tank's linear velocity
				velocityComponent.Linear = projectileComponent.Force;
				PhysicsVelocityGroup[entityA] = velocityComponent;

				// Apply damage to the tank
				healthComponent.Health -= projectileComponent.MaxDamage;
				TankHealthGroup[entityA] = healthComponent;
			}
		}
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		JobHandle jobHandle = new CollisionEventImpulseJob
		{
			ProjectileDamageGroup = GetComponentDataFromEntity<ProjectileDamageComponent>(true),
			PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
			TankHealthGroup = GetComponentDataFromEntity<TankHealthComponent>(),
		}.Schedule(m_StepPhysicsWorldSystem.Simulation,
					ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

		return jobHandle;
	}
}