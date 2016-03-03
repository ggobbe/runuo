using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1f2d, 0x1f2e )]
	public class SacredBoonScroll : SpellScroll
	{
		[Constructable]
		public SacredBoonScroll() : this( 1 )
		{
			
		}

		[Constructable]
		public SacredBoonScroll( int amount ) : base( 808, 0x1f2d, amount )
		{
		         Name = "Guerison Divine";
		         Hue = 0x53;			
		}

		public SacredBoonScroll( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

        //public override Item Dupe( int amount )
        //{
        //    return base.Dupe( new SacredBoonScroll( amount ), amount );
        //}
	}
}