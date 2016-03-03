﻿using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class EnemyOfOneScroll : SpellScroll
    {
        [Constructable]
        public EnemyOfOneScroll()
            : this(1)
        {
        }

        [Constructable]
        public EnemyOfOneScroll(int amount)
            : base(205, 0x1F65, amount)
        {
            Hue = 45;
            Name = "Enemy Of One scroll";
        }

        public EnemyOfOneScroll(Serial serial)
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


