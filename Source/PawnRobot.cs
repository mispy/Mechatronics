using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
    internal class PawnRobot : Pawn
    {

        public static Graphic_Apparel CentipedeGraphic = new Graphic_Apparel("Things/Pawn/Mechanoid/Centipede", new Color(1, 1, 1));

        private bool robotReady = false;

        public PawnRobot() : base() {
        }

        public override void SpawnSetup() {
            base.SpawnSetup();
        }
        
        // Override everything to be more robot-friendly
        public void RobotSetup() {            
            // We need graphics to be initialized before we can meddle with them
            if (this.drawer == null || this.drawer.renderer == null || this.drawer.renderer.graphics == null)
                return;
            
            // dehumanoidize
            this.drawer.renderer.graphics.headGraphic = null;
            //this.apparel = null;
            this.apparel.SetApparel()
            this.drawer.renderer.graphics.nakedGraphic = CentipedeGraphic;

            this.psychology.Loyalty.curLevel = 100f;
            this.psychology.Happiness.curLevel = 100f;
            this.psychology.Fear.curLevel = 0f;
            this.food.Food.curLevel = 100f;
            this.rest.Rest.curLevel = 100f;

            this.story.childhood.title = "DIY Mechanoid";
            this.story.childhood.baseDesc = "This mechanoid entered service immediately after production, and so never had a childhood. Alternatively, you may view it as being a big dumb happy mecha baby for eternity.";
            this.story.childhood.skillGains = new Dictionary<string, int>();
            this.story.childhood.skillGainsResolved = new Dictionary<SkillDef, int>();
            this.story.childhood.workDisables = WorkTags.None;

            this.story.adulthood.title = "DIY Mechanoid";
            this.story.adulthood.baseDesc = "A DIY Mechanoid";
            this.story.adulthood.skillGains = new Dictionary<string, int>();
            this.story.adulthood.skillGainsResolved = new Dictionary<SkillDef, int>();
            this.story.adulthood.skillGains["Mining"] = 5;
            this.story.adulthood.skillGains["Shooting"] = 5;
            this.story.adulthood.ResolveReferences();
            this.story.adulthood.workDisables = WorkTags.Artistic | WorkTags.Caring | WorkTags.Cooking | WorkTags.Crafting | WorkTags.Intellectual | WorkTags.ManualSkilled | WorkTags.PlantWork | WorkTags.Social;


            this.psychology.thoughts.GainThought(ThoughtDef.Named("HappyRobot"));

            this.robotReady = true;
        }

        public override void Tick()
        {
            if (this.robotReady == false) {
                RobotSetup();
                this.food.FoodTick();
                this.rest.RestTick();
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
            if (this.skills != null)
                this.skills.SkillsTick();
            if (this.thinker != null)
                this.thinker.MindTick();
            if (this.playerController == null)
                return;
            this.playerController.PlayerControllerTick();
        }
    }
}
