using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.Cleric
{
	public abstract class ClericSpell : Spell
	{
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(4.0); } }
		public abstract double RequiredSkill{ get; }
		public abstract int RequiredMana{ get; }
		public abstract int RequiredTithing{ get; }
        public abstract SpellCircle Circle { get; }
		public override SkillName CastSkill{ get{ return SkillName.Magery; } }
		public override bool ClearHandsOnCast{ get{ return false; } }


		public ClericSpell( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info )
		{
		}

		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
				return false;

			if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "Vous devez avoir " + RequiredSkill + " Magie pour invoquer" );
				return false;
			}
			else if ( Caster.TithingPoints < RequiredTithing )
			{
				Caster.SendMessage( "Vous devez avoir " + RequiredTithing + " points de foi pour inv0quer." );
				return false;
			}
			else if ( Caster.Mana < ScaleMana( GetMana() ) )
			{
				Caster.SendMessage( "Vous devez avoir " + GetMana() + " Mana pour invoquer." );
				return false;
			}

			return true;
		}

        public override int GetMana()
        {
            return 0;
        }

		public override bool CheckFizzle()
		{
			if ( !base.CheckFizzle() )
				return false;

			int tithing = RequiredTithing;
			double min, max;

			GetCastSkills( out min, out max );

			if ( AosAttributes.GetValue( Caster, AosAttribute.LowerRegCost ) > Utility.Random( 100 ) )
				tithing = 0;

			int mana = ScaleMana( GetMana() );

			if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "Vous devez avoir " + RequiredSkill + " Magie pour invoquer." );
				return false;
			}
			else if ( Caster.TithingPoints < tithing )
			{
				Caster.SendMessage( "Vous devez avoir " + tithing + " points de foi pour inv0quer." );
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendMessage( "Vous devez avoir " + mana + " Mana pour invoquer." );
				return false;
			}

			Caster.TithingPoints -= tithing;

			return true;
		}

		public override void SayMantra()
		{
			Caster.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, Info.Mantra );
			Caster.PlaySound( 0x24A );
		}

		public override void DoFizzle()
		{
			Caster.PlaySound( 0x1D6 );
			Caster.NextSpellTime = DateTime.Now;
		}

		public override void DoHurtFizzle()
		{
			Caster.PlaySound( 0x1D6 );
		}

		public override void OnDisturb( DisturbType type, bool message )
		{
			base.OnDisturb( type, message );

			if ( message )
				Caster.PlaySound( 0x1D6 );
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.FixedEffect( 0x37C4, 10, 42, 4, 3 );
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill;
			max = RequiredSkill + 20.0;
		}
        public virtual bool CheckResisted(Mobile target)
        {
            double n = GetResistPercent(target);

            n /= 100.0;

            if (n <= 0.0)
                return false;

            if (n >= 1.0)
                return true;

            int maxSkill = (1 + (int)Circle) * 10;
            maxSkill += (1 + ((int)Circle / 6)) * 25;

            if (target.Skills[SkillName.MagicResist].Value < maxSkill)
                target.CheckSkill(SkillName.MagicResist, 0.0, 120.0);

            return (n >= Utility.RandomDouble());
        }

        public virtual double GetResistPercentForCircle(Mobile target, SpellCircle circle)
        {
            double firstPercent = target.Skills[SkillName.MagicResist].Value / 5.0;
            double secondPercent = target.Skills[SkillName.MagicResist].Value - (((Caster.Skills[CastSkill].Value - 20.0) / 5.0) + (1 + (int)circle) * 5.0);

            return (firstPercent > secondPercent ? firstPercent : secondPercent) / 2.0; // Seems should be about half of what stratics says.
        }

        public virtual double GetResistPercent(Mobile target)
        {
            return GetResistPercentForCircle(target, Circle);
        }
    }
}
