using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
    internal class Mecha_Corpse : Corpse {
        public Mecha_Corpse() : base() {
        }

        public override void SpawnSetup() {
            base.SpawnSetup();
            this.DeSpawn();
        }
    }
}
