using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells.Seventh;
using Server.Gumps;

namespace Server.Spells.Cleric
{
	public class AngelicFaithSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Divine Morph", "Deus Servitate",
				212,
				9041
			);
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(5.0); } }
		public override int RequiredTithing{ get{ return 1000; } }
		public override double RequiredSkill{ get{ return 140.0; } }
		public override int RequiredMana{ get{ return 110; } }
        public override SpellCircle Circle { get { return SpellCircle.Eighth; } }
		private static Hashtable m_Table = new Hashtable();

		public AngelicFaithSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public static bool HasEffect( Mobile m )
		{
			return ( m_Table[m] != null );
		}

		public static void RemoveEffect( Mobile m )
		{
			object[] mods = (object[])m_Table[m];

			if ( mods != null )
			{
				m.RemoveStatMod( ((StatMod)mods[0]).Name );
				m.RemoveStatMod( ((StatMod)mods[1]).Name );
				m.RemoveStatMod( ((StatMod)mods[2]).Name );
				m.RemoveSkillMod( (SkillMod)mods[3] );
				m.RemoveSkillMod( (SkillMod)mods[4] );
				m.RemoveSkillMod( (SkillMod)mods[5] );
			}

			m_Table.Remove( m );

			m.EndAction( typeof( AngelicFaithSpell ) );

			m.BodyMod = 0;
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
			{
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( AngelicFaithSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 );
				return false;
			}
			else if ( TransformationSpellHelper.UnderTransformation( Caster ) )
			{
				Caster.SendMessage( "Vous ne pouvez rien faire dans cet etat." );
				return false;
			}
			else if ( DisguiseGump.IsDisguised( Caster ) )
			{
				Caster.SendMessage( "Vous ne pouvez rien faire dans cet etat.." );
				return false;
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendMessage( "Vous ne pouvez rien faire dans cet etat." );
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) )
			{
				Caster.SendMessage( "Vous ne pouvez rien faire dans cet etat.." );
				return false;
			}

			return true;
		}		

		public override void OnCast()
		{
			if ( !Caster.CanBeginAction( typeof( AngelicFaithSpell ) ) )
			{
				Caster.SendLocalizedMessage( 1005559 );
			}
            else if (TransformationSpellHelper.UnderTransformation(Caster))
			{
				Caster.SendMessage( "Vous ne pouvez rien faire dans cet etat.." );
			}
			else if ( DisguiseGump.IsDisguised( Caster ) )
			{
				Caster.SendMessage( "Vous ne pouvez rien faire dans cet etat.." );
			}
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendMessage( "Vous ne pouvez rien faire dans cet etat.." );
			}
			else if ( !Caster.CanBeginAction( typeof( PolymorphSpell ) ) )
			{
				Caster.SendMessage( "Vous ne pouvez rien faire dans cet etat.." );
			}
			else if ( CheckSequence() )
			{
				object[] mods = new object[]
				{
					new StatMod( StatType.Str, "[Cleric] Str Offset", 30, TimeSpan.Zero ),
					new StatMod( StatType.Dex, "[Cleric] Dex Offset", 30, TimeSpan.Zero ),
					new StatMod( StatType.Int, "[Cleric] Int Offset", 30, TimeSpan.Zero ),
					new DefaultSkillMod( SkillName.Macing, true, 20 ),
					new DefaultSkillMod( SkillName.SpiritSpeak, true, 20 ),
					new DefaultSkillMod( SkillName.MagicResist, true, 60 )
				};

				m_Table[Caster] = mods;

				Caster.AddStatMod( (StatMod)mods[0] );
				Caster.AddStatMod( (StatMod)mods[1] );
				Caster.AddStatMod( (StatMod)mods[2] );
				Caster.AddSkillMod( (SkillMod)mods[3] );
				Caster.AddSkillMod( (SkillMod)mods[4] );
				Caster.AddSkillMod( (SkillMod)mods[5] );

				double span = 5.0 * DivineFocusSpell.GetScalar( Caster );
				new InternalTimer( Caster, TimeSpan.FromMinutes( (int)span ) ).Start();

				IMount mount = Caster.Mount;

				if ( mount != null )
					mount.Rider = null;

				Caster.BodyMod = 123;
				Caster.BeginAction( typeof( AngelicFaithSpell ) );
				Caster.PlaySound( 0x165 );
				Caster.FixedParticles( 0x3728, 1, 13, 0x480, 92, 3, EffectLayer.Head );
			}
		}


		private class InternalTimer : Timer
		{
			private Mobile m_Owner;
			private DateTime m_Expire;

			public InternalTimer( Mobile owner, TimeSpan duration ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Owner = owner;
				m_Expire = DateTime.Now + duration;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					AngelicFaithSpell.RemoveEffect( m_Owner );
					Stop();
				}
			}
		}
	}
}
