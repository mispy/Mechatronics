using System;
using UnityEngine;
using Verse.AI;

namespace Verse
{
    internal class Mecha_BuildingCentipede : Building
    {
        public Mecha_BuildingCentipede() : base() {
        }

        public override void Tick()
        {
            base.Tick();

            if (this.connectedToTransmitter != null) {
                var pawn = PawnGenerator.GeneratePawn("Mecha_Centipede", Faction.OfColony);
                pawn.age = 1;
                GenSpawn.Spawn(pawn, this.Position);
                this.DeSpawn();
            }
        }
    }
}
