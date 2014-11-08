using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
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

            if (this.PowerComp.connectParent != null) {
                var pawn = PawnGenerator.GeneratePawn("Mecha_Centipede", Faction.OfColony);
                //pawn.age = 1; 
                /*var source = new List<ThingDef>();
                source.Add(ThingDef.Named("Gun_ChargeBlaster"));
                source.Add(ThingDef.Named("Gun_InfernoCannon"));
                source.Add(ThingDef.Named("Gun_Minigun"));
                var weapon = (Equipment) ThingMaker.MakeThing(GenLinq.RandomElement<ThingDef>(source));
                pawn.equipment.AddEquipment(weapon);*/
                GenSpawn.Spawn(pawn, this.Position);
                this.DeSpawn();
                PowerNetManager.Notify_ConnectorDespawned(this);
                Find.MapDrawer.MapChanged(this.Position, MapChangeType.PowerGrid, true, false);
            }
        }
    }
}
