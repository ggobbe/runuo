using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Spells.Cleric
{
	public class ResutousSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Todas Vivantis", "Deus Favoritate",
				-1,
				9002
			);
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3.0); } }
		public override double RequiredSkill{ get{ return 110.0; } }
		public override int RequiredMana{ get{ return 80; } }
		public override int RequiredTithing{ get{ return 200; } }
		//public override int MantraNumber{ get{ return 1060725; } } // Dium Prostra
		public override bool BlocksMovement{ get{ return false; } }
        public override SpellCircle Circle { get { return SpellCircle.Seventh; } }
		public ResutousSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				ArrayList targets = new ArrayList();

				foreach ( Mobile m in Caster.GetMobilesInRange( 6 ) ) // TODO: Validate range
				{
					if ( m is BaseCreature && ((BaseCreature)m).IsAnimatedDead )
						continue;

					if ( Caster != m && Caster.CanBeBeneficial( m, false, true ) && !(m is Golem) )
						targets.Add( m );
					if(m is PlayerMobile&&!m.Alive)	
					{					
				Caster.PlaySound( 0x108 );
				Caster.FixedParticles( 0x375a, 1, 30, 9965, 5, 7, EffectLayer.Waist );
				Caster.FixedParticles( 0x376A, 1, 30, 9502, 5, 3, EffectLayer.Waist );
							m.SendGump( new ResurrectGump( m ) );	
						}					
				}

				/*Caster.PlaySound( 0x108 );
				Caster.FixedParticles( 0x375a, 1, 30, 9965, 5, 7, EffectLayer.Waist );
				Caster.FixedParticles( 0x376A, 1, 30, 9502, 5, 3, EffectLayer.Waist );
							m.SendGump( new ResurrectGump( m, Caster ) );
				/* Attempts to Resurrect, Cure and Heal all targets in a radius around the caster.
				 * If any target is successfully assisted, the Paladin's current
				 * Hit Points, Mana and Stamina are set to 1.
				 * Amount of damage healed is affected by the Caster's Karma, from 8 to 24 hit points.
				 */

				//bool sacrifice = false;

				/*int toHeal = 16 + (Caster.Karma / 1250) + (int)(Caster.Skills[SkillName.Magery].Value / 40.0);

				if ( toHeal < 8 )
					toHeal = 8;
				else if ( toHeal > 24 )
					toHeal = 24;

				double resChance = 1.1 + (0.9 * ((double)Caster.Karma / 1000));

				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];

					if ( !m.Alive )
					{
						if ( resChance > Utility.RandomDouble() )
						{
							m.FixedParticles( 0x375A, 1, 15, 5005, 5, 3, EffectLayer.Head );
							m.SendGump( new ResurrectGump( m, Caster ) );
							//sacrifice = true;
						}
					}
					else
					{
						bool sendEffect = false;

						if ( m.Poisoned && m.CurePoison( Caster ) )
						{
							Caster.DoBeneficial( m );

							if ( Caster != m )
								Caster.SendLocalizedMessage( 1010058 ); // You have cured the target of all poisons!

							m.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.
							sendEffect = true;
							//sacrifice = true;
						}

						if ( m.Hits < m.HitsMax )
						{
							Caster.DoBeneficial( m );
							m.Heal( toHeal );
							sendEffect = true;
							//sacrifice = true;
						}

						if ( sendEffect )
							m.FixedParticles( 0x375A, 1, 15, 5005, 5, 3, EffectLayer.Head );
					}
				}

				/*if ( sacrifice )
				{
					Caster.PlaySound( 0x423 );
					Caster.Hits = 1;
					Caster.Stam = 1;
					Caster.Mana = 1;
				}*/
			}

			FinishSequence();
		}
	}
}
