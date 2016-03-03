using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells.Druid
{
    public class FoudreSpell : DruidSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Colere du Ciel", "Flor Act Mos Tar",
				269,
				9020,
            Reagent.DestroyingAngel
			);

        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(3.0); } }
      public override double RequiredSkill{ get{ return 110.0; } }
      public override int RequiredMana{ get{ return 30; } }
		public FoudreSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
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
			else if ( CheckHSequence( m ) )
			{
				m.BoltEffect( 0x480 );

				SpellHelper.Turn( Caster, m );

				double damage = ((Caster.Skills[SkillName.Magery].Value + Caster.Skills[SkillName.AnimalLore].Value) / 2);

				if ( Core.AOS )
				{
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 0, 0, 0, 75 );//100
				}
				else
				{
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage );
				}
			}

			FinishSequence();
		}


		private class InternalTarget : Target
		{
			private FoudreSpell m_Owner;

			public InternalTarget( FoudreSpell owner ) : base( 12, false, TargetFlags.Harmful )
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
