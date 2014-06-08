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
            var pawn = PawnGenerator.GeneratePawn("DIYCentipede", Faction.OfColony);
            GenSpawn.Spawn(pawn, this.Position);
        }
    }
}
