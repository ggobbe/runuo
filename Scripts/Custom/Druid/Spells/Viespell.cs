using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
namespace Server.Spells.Druid
{
    public class Viespell : DruidSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Vitalite", "Mas Furr Vimas",
				269,
				9020,
				Reagent.SpringWater,
            Reagent.DestroyingAngel
			);

       public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1.0); } }
      public override double RequiredSkill{ get{ return 40.0; } }
      public override int RequiredMana{ get{ return 15; } }
		public override bool BlocksMovement{ get{ return false; } }		
		public Viespell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckBSequence( m, false ) )
			{
				SpellHelper.Turn( Caster, m );

				m.PlaySound( 0x202 );
				m.FixedParticles( 14186, 1, 62, 0x480, 3, 3, EffectLayer.Waist );
				m.FixedParticles( 14138, 1, 46, 0x481, 5, 3, EffectLayer.Waist );

				double toHeal = Caster.Skills[SkillName.Magery].Value / 0.3 + Utility.Random( 5 );

				toHeal *= DruideFocusSpell.GetScalar( Caster );

				m.Heal( (int)toHeal );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private Viespell m_Owner;

			public InternalTarget( Viespell owner ) : base( 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}