﻿using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class FocusAttackScroll : SpellScroll
    {
        [Constructable]
        public FocusAttackScroll()
            : this(1)
        {
        }

        [Constructable]
        public FocusAttackScroll(int amount)
            : base(500, 0x1F65, amount)
        {
            Hue = 48;
            Name = "Focus Attack Scroll";
        }

        public FocusAttackScroll(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }


    }
}
