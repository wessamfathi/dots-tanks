using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;

// This system applies an impulse to any dynamic that collides with a Repulsor.
// A Repulsor is defined by a PhysicsShapeAuthoring with the `Raise Collision Events` flag ticked and a
// CollisionEventImpulse behaviour added.
[UpdateBefore(typeof(EndFramePhysicsSystem))]
public class ProjectileCollisionSystem : JobComponentSystem
{
	BuildPhysicsWorld m_BuildPhysicsWorldSystem;
	StepPhysicsWorld m_StepPhysicsWorldSystem;
	EndSimulationEntityCommandBufferSystem m_endSimEcbSystem;

	protected override void OnCreate()
	{
		m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
		m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();

		m_endSimEcbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
	}

	[BurstCompile]
	struct CollisionEventImpulseJob : ITriggerEventsJob
	{
		public ComponentDataFromEntity<ProjectileDamageComponent> ProjectileDamageGroup;
		public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;
		public ComponentDataFromEntity<TankHealthComponent> TankHealthGroup;
        [ReadOnly] public ComponentDataFromEntity<Tank1> Tank1CollisionGroup;
        [ReadOnly] public ComponentDataFromEntity<Tank2> Tank2CollisionGroup;

        public void Execute(TriggerEvent triggerEvent)
		{
			Entity entityA = triggerEvent.Entities.EntityA;
			Entity entityB = triggerEvent.Entities.EntityB;

			bool isBodyADynamic = PhysicsVelocityGroup.Exists(entityA);
			bool isBodyBDynamic = PhysicsVelocityGroup.Exists(entityB);

			bool isBodyAProjectile = ProjectileDamageGroup.Exists(entityA);
			bool isBodyBProjectile = ProjectileDamageGroup.Exists(entityB);

            bool canCollide = (Tank1CollisionGroup.Exists(entityA) && Tank2CollisionGroup.Exists(entityB)) ||
                (Tank2CollisionGroup.Exists(entityA) && Tank1CollisionGroup.Exists(entityB));

            if (canCollide)
            {
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

                    // Explode projectile
                    projectileComponent.Health = 0;
                    ProjectileDamageGroup[entityA] = projectileComponent;
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

                    // Explode projectile
                    projectileComponent.Health = 0;
                    ProjectileDamageGroup[entityB] = projectileComponent;
                }
            }
		}
    }

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		JobHandle jobHandle = new CollisionEventImpulseJob
		{
			ProjectileDamageGroup = GetComponentDataFromEntity<ProjectileDamageComponent>(),
			PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
			TankHealthGroup = GetComponentDataFromEntity<TankHealthComponent>(),
            Tank1CollisionGroup = GetComponentDataFromEntity<Tank1>(),
            Tank2CollisionGroup = GetComponentDataFromEntity<Tank2>(),
		}.Schedule(m_StepPhysicsWorldSystem.Simulation,
					ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);

		return jobHandle;
	}
}