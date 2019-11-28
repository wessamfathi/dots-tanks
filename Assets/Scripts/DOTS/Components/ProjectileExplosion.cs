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

public struct ProjectileExplosionEffectsComponent : ISharedComponentData, IEquatable<ProjectileExplosionEffectsComponent>
{
	public AudioSource m_ExplosionAudio;
	public ParticleSystem m_ExplosionParticles;

	public bool Equals(ProjectileExplosionEffectsComponent other)
	{
		return (m_ExplosionAudio == other.m_ExplosionAudio) && (m_ExplosionParticles == other.m_ExplosionParticles);
	}

	public override int GetHashCode()
	{
		return m_ExplosionAudio.GetHashCode() & m_ExplosionParticles.GetHashCode();
	}
}
