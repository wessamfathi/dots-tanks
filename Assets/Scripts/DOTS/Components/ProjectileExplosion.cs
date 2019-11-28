using System;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct ProjectileDamageComponent : IComponentData
{
	public float3 Force;
	public float MaxDamage;
	public float Radius;
	public int Health;
}

public struct ProjectileExplosionAudioComponent : ISharedComponentData, IEquatable<ProjectileExplosionAudioComponent>
{
	public AudioSource m_ExplosionAudio;

	public bool Equals(ProjectileExplosionAudioComponent other)
	{
		return (m_ExplosionAudio == other.m_ExplosionAudio);
	}

	public override int GetHashCode()
	{
		return m_ExplosionAudio.GetHashCode();
	}
}

public struct ProjectileExplosionParticleComponent : ISharedComponentData, IEquatable<ProjectileExplosionParticleComponent>
{
	public ParticleSystem m_ExplosionParticles;

	public bool Equals(ProjectileExplosionParticleComponent other)
	{
		return m_ExplosionParticles = other.m_ExplosionParticles;
	}

	public override int GetHashCode()
	{
		return m_ExplosionParticles.GetHashCode();
	}
}
