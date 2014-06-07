using System;
using UnityEngine;
using Verse.AI;

namespace Verse
{
    internal class BuildingCentipede : Building
    {
        public BuildingCentipede() : base() {
        }

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            Log.Message("Centipede setup!");
        }
    }
}
