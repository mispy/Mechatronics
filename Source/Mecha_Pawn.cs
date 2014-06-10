using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse.AI;

namespace Verse
{
    internal class Mecha_Pawn : Pawn
    {
        public static Graphic_Apparel CentipedeGraphic = new Graphic_Apparel("Things/Pawn/Mechanoid/Centipede", new Color(1, 1, 1));

        private bool mechaReady = false;

        public Mecha_Pawn() : base() {
        }

        public override void SpawnSetup() {
            base.SpawnSetup();
        }
        
        // Override everything to be more mecha-friendly
        // Many of these cannot be defined in defs yet (or if they can, cause conflicts with colonist-specific code)
        public void MechaSetup() {            
            // We need graphics to be initialized before we can meddle with them
            if (this.drawer == null || this.drawer.renderer == null || this.drawer.renderer.graphics == null)
                return;
            
            // dehumanoidize
            this.drawer.renderer.graphics.headGraphic = null;
            //this.apparel = null;

            this.drawer.renderer.graphics.nakedGraphic = CentipedeGraphic;
            if (this.apparel.WornApparelInOrder.Count() > 0) {
                this.apparel.WornApparelInOrder.First().graphic = CentipedeGraphic;
            }

            this.story.childhood.title = "None";
            this.story.childhood.baseDesc = "This mechanoid entered service immediately after production, and so never had a childhood. Alternatively, you may view it as being a big dumb happy mecha baby for eternity.";
            this.story.childhood.skillGains = new Dictionary<string, int>();
            this.story.childhood.skillGainsResolved = new Dictionary<SkillDef, int>();
            this.story.childhood.workDisables = WorkTags.None;

            this.story.adulthood.title = "Mechanoid";
            this.story.adulthood.titleShort = "Mechanoid";
            this.story.adulthood.baseDesc = "A mechanoid reverse-engineered from marauding self-replicators. Tough and completely tireless, but slow-moving and incapable of sophisticated tasks.";
            this.story.adulthood.skillGains = new Dictionary<string, int>();           
            this.story.adulthood.skillGainsResolved = new Dictionary<SkillDef, int>();
            this.story.adulthood.ResolveReferences();
            this.story.adulthood.workDisables = WorkTags.Artistic | WorkTags.Caring | WorkTags.Cooking | WorkTags.Crafting | WorkTags.Intellectual | WorkTags.ManualSkilled | WorkTags.PlantWork | WorkTags.Social;

            // Mechanoids don't have any traits
            this.story.traits.allTraits = new List<Trait>();

            this.playerController.workSettings.InitialSetupFromSkills();
            
            foreach (var skill in this.skills.skills) {
                if (skill.level < 8)
                    skill.level = 8;
            }

            this.gender = Gender.Sexless;
            this.story.name.first = "Model C";

            this.RaceDef.isFlesh = false;
            this.RaceDef.corpseDef = ThingDef.Named("Mecha_Corpse");
            
            this.psychology.Loyalty.curLevel = 100f;
            this.psychology.Happiness.curLevel = 100f;
            this.psychology.Fear.curLevel = 0f;
            this.food.Food.curLevel = 100f;
            this.rest.Rest.curLevel = 100f;
            this.psychology.thoughts.GainThought(ThoughtDef.Named("HappyRobot"));

            foreach (var skill in this.skills.skills) {
                skill.passion = Passion.None;
            }

            this.mechaReady = true;
        }

        public override void Tick()
        {
            if (this.mechaReady == false) {
                MechaSetup();
                // These two initial ticks are necessary to initialize data somewhere
                this.food.FoodTick();
                this.rest.RestTick();
            }

            // This hackery is to force the autoundrafter timer to reset
            // So drafted mechanoids stay like that forever
            if (this.playerController.Drafted) {
                var curjob = this.jobs.curJob;
                this.jobs.curJob = null;
                this.playerController.Drafted = false;
                this.playerController.drafter.DrafterTick();
                this.playerController.Drafted = true;
                this.jobs.curJob = curjob;
            }

            var comp = this.GetComp<CompExplosive>();

            if (comp.wickStarted) {
                comp.CompTick();
            } else if (this.health <= 0) {
                Explosion.DoExplosion(this.Position, 1.9f, DamageTypeDefOf.Bomb, this);
            } else if (this.health < 50) {
                comp.StartWick();
            }

            if (this.health < 50) {
                this.health = 49;
            } 

            if (Find.TickManager.tickCount % 100 == 0) {
                this.healthTracker.Heal(1);
            }

            if (!this.stances.FullBodyBusy)
                this.pather.PatherTick();
            this.drawer.DrawTrackerTick();
            this.healthTracker.HealthTick();
            this.stances.StanceTrackerTick();
            if (this.equipment != null)
                this.equipment.EquipmentTrackerTick();
            if (this.jobs != null)
                this.jobs.JobTrackerTick();
            if (this.carryHands != null)
                this.carryHands.CarryHandsTick();
            /*if (this.talker != null)
                this.talker.TalkTrackerTick();
            if (this.psychology != null)
                this.psychology.PsychologyTick();
            if (this.food != null)
                this.food.FoodTick();
            if (this.rest != null)
                this.rest.RestTick();*/
            if (this.prisoner != null)
                this.prisoner.PrisonerTrackerTick();
            /*if (this.skills != null)
                this.skills.SkillsTick();*/
            if (this.thinker != null)
                this.thinker.MindTick();
            if (this.playerController == null)
                return;
            this.playerController.PlayerControllerTick();
        }
    }
}
