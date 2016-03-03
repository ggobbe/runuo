﻿using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BackstabScroll : SpellScroll
    {
        [Constructable]
        public BackstabScroll()
            : this(1)
        {
        }

        [Constructable]
        public BackstabScroll(int amount)
            : base(505, 0x1F65, amount)
        {
            Hue = 48;
            Name = "Backstab Scroll";
        }

        public BackstabScroll(Serial serial)
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
