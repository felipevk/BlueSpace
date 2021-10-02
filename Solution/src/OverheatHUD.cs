using System;

using Blue.Core;
using Blue.ECS;
using Microsoft.Xna.Framework;

namespace BlueSpace
{
	[RequiresComponentData(typeof( SoundComponentData ))]
	public class OverheatHUDComponentData : ComponentData
	{
		public int Width;
		public int Height;
		public String playerWeaponId;
		public String hudTextId;
		public PlayerWeaponComponentData weaponData;
		public SoundComponentData soundData;
		public TextComponentData hudTextData;
	}

	public class OverheatHUDComponentSystem : ComponentSystem
	{
		protected override void Start( string gameObjectId, ComponentData data )
		{
			OverheatHUDComponentData overheatData = data as OverheatHUDComponentData;
			overheatData.weaponData = GetComponentData<PlayerWeaponComponentData>( overheatData.playerWeaponId );
			overheatData.soundData = GetComponentData<SoundComponentData>( gameObjectId );
			overheatData.hudTextData = GetComponentData<TextComponentData>( overheatData.hudTextId );
			overheatData.hudTextData.enabled = false;
		}

		protected override void Update( string gameObjectId, ComponentData data )
		{
			OverheatHUDComponentData overheatData = data as OverheatHUDComponentData;

			if ( overheatData.weaponData.isOverheat )
			{
				if ( !overheatData.soundData.isPlaying )
				{
					SoundComponentSystem.Play( gameObjectId, overheatData.soundData );
					overheatData.hudTextData.enabled = true;
					GetComponentData<TextBlinkComponentData>( overheatData.hudTextId ).enabled = true;
				}
			}
			else
			{
				if ( overheatData.soundData.isPlaying )
				{
					SoundComponentSystem.Stop( gameObjectId, overheatData.soundData );
					overheatData.hudTextData.enabled = false;
					GetComponentData<TextBlinkComponentData>( overheatData.hudTextId ).enabled = false;
				}
			}
		}

		protected override void Render( string gameObjectId, ComponentData data )
		{
			OverheatHUDComponentData overheatData = data as OverheatHUDComponentData;

			Rectangle borderRectangle = new Rectangle( 0, 0, overheatData.Width, overheatData.Height )
			{
				X = (int)GetGameObject( gameObjectId ).GetGlobalPosition().X,
				Y = (int)GetGameObject( gameObjectId ).GetGlobalPosition().Y
			};
			Color borderColor = overheatData.weaponData.isOverheat ? Color.Red : Color.Green;
			Blue.Game.Instance.GameRenderer.PrepareToDrawRectangle(
				borderRectangle,
				borderColor,
				false );

			Rectangle fillRectangle = borderRectangle;
			fillRectangle.X += (int)( fillRectangle.X * 0.2f );
			fillRectangle.Y += (int)( fillRectangle.Y * 0.005f );
			fillRectangle.Width -= (int)( fillRectangle.Width * 0.02f );
			fillRectangle.Height -= (int)( fillRectangle.Height * 0.2f );
			int fullWidth = fillRectangle.Width;
			float overheatProgress = overheatData.weaponData.currentOverheat / overheatData.weaponData.overheatDuration;
			fillRectangle.Width = (int)( ( overheatProgress ) * fullWidth );
			// from yellow(255, 255, 0) to red(255, 0, 0)
			Color fillColor = new Color
			{
				R = 255,
				G = (byte) ( 255 * ( 1 - overheatProgress ) ),
				B = 0,
				A = 255
			};
			Blue.Game.Instance.GameRenderer.PrepareToDrawRectangle(
				fillRectangle,
				fillColor,
				true );

			Rectangle shootIndicatorRectangle = fillRectangle;
			fillRectangle.X += (int)( overheatData.weaponData.overheatCanShoot * fullWidth );
			fillRectangle.Y = borderRectangle.Y;
			fillRectangle.Width = 1;
			fillRectangle.Height = borderRectangle.Height;
			Blue.Game.Instance.GameRenderer.PrepareToDrawRectangle(
				fillRectangle,
				Color.White,
				true );
		}
	}
}
