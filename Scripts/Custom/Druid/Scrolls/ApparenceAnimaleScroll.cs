using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
   public class ApparenceAnimaleScroll : SpellScroll
   {
      [Constructable]
      public ApparenceAnimaleScroll() : this( 1 )
      {
      }

      [Constructable]
      public ApparenceAnimaleScroll( int amount ) : base( 726, 0xE39 )
      {
         Name = "Apparence Animale";
         Hue = 0x58B;
      }

      public ApparenceAnimaleScroll( Serial serial ) : base( serial )
      {
      		ItemID=0xE39;
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
      //   return base.Dupe( new ApparenceAnimaleScroll( amount ), amount );
      //}
      public override void OnDoubleClick( Mobile from )
		{
            if (from is PlayerMobile)// && ((PlayerMobile)from).Druidisme )
			{
				base.OnDoubleClick(from);
			}
			else
			{
				from.SendMessage ("Vous devez �tre druide pour pouvoir lancer cela");
				return;
			}
		}
   }
}
