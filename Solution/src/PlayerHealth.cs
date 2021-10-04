using System;

using Blue.Core;
using Blue.ECS;

namespace BlueSpace
{
	[RequiresComponentData(typeof(SpriteBlinkComponentData))]
	[RequiresComponentData( typeof( BoxCollision2DComponentData ) )]
	public class PlayerHealthComponentData : ComponentData
	{
		public int health;
		public int initialHealth;
		public int maxHealth;
		public float currentHitTime;
		public float hitDuration;
		public Action onDeath;
		public String hitSound;
		public String destroySound;
	}

	public class PlayerHealthComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
			GetComponentData<SpriteBlinkComponentData>( gameObjectId ).enabled = false;
		}
		protected override void Update( string gameObjectId, ComponentData data )
		{
			PlayerHealthComponentData playerHealthData = data as PlayerHealthComponentData;

			if ( playerHealthData.currentHitTime > 0f )
			{
				playerHealthData.currentHitTime -= Time.DeltaTime;
				if ( playerHealthData.currentHitTime <= 0f )
				{
					GetComponentData<BoxCollision2DComponentData>( gameObjectId ).enabled = true;
					SpriteBlinkComponentData playerBlink = GetComponentData<SpriteBlinkComponentData>( gameObjectId );
					playerBlink.enabled = false;
					GetComponentSystem<SpriteBlinkComponentSystem>().SetBlink( gameObjectId, playerBlink, false );
				}
			}
		}
		protected override void OnCollision2DEnter( string gameObjectId, ComponentData data, string colliderId )
		{
			PlayerHealthComponentData playerHealthData = data as PlayerHealthComponentData;

			if ( playerHealthData.currentHitTime <= 0f && HasComponentData<MeteorComponentData>( colliderId ) )
			{
				MeteorComponentData meteorData = GetComponentData<MeteorComponentData>( colliderId );
				if ( meteorData.damage <= 0 )
					return;

				playerHealthData.health -= meteorData.damage;

				if ( playerHealthData.health > 0 )
				{
					SoundComponentSystem.PlayOnce( playerHealthData.hitSound, 0.7f );
					playerHealthData.currentHitTime = playerHealthData.hitDuration;
					GetComponentData<BoxCollision2DComponentData>( gameObjectId ).enabled = false;
					GetComponentData<SpriteBlinkComponentData>( gameObjectId ).enabled = true;
				}
				else
				{
					SoundComponentSystem.PlayOnce( playerHealthData.destroySound );
					GetComponentData<SpriteComponentData>( gameObjectId ).enabled = false;
					playerHealthData.onDeath?.Invoke();
				}
			}
		}
		public void Reset( String gameObjectId )
		{
			PlayerHealthComponentData playerHealth = GetComponentData<PlayerHealthComponentData>( gameObjectId );
			playerHealth.health = playerHealth.initialHealth;
			GetComponentData<SpriteComponentData>( gameObjectId ).enabled = true;
		}
	}
}
