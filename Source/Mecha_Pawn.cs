using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse.AI;

namespace Verse
{
    internal class Mecha_Pawn : Pawn
    {
        //public static Graphic_Appearances CentipedeGraphic = new Graphic_Appearances("Things/Pawn/Mechanoid/Centipede", new Color(1, 1, 1));

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

            //this.drawer.renderer.graphics.nakedGraphic = CentipedeGraphic;
            if (this.apparel.WornApparelInOrder.Count() > 0) {
                var apparel = this.apparel.WornApparelInOrder.First();
                // apparel.Graphic = CentipedeGraphic;
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
            //this.story.traits.allTraits = new List<Trait>();

            //this.playerController.workSettings.InitialSetupFromSkills();
            
            foreach (var skill in this.skills.skills) {
                if (skill.level < 8)
                    skill.level = 8;
            }

            //this.gender = Gender.Sexless;
            this.story.name.first = "Model C";

            this.RaceProps.isFlesh = false;
            this.RaceProps.corpseDef = ThingDef.Named("Mecha_Corpse");

            this.psychology.mood.CurLevel = 50f;
            this.food.Food.CurLevel = 100f;
            this.rest.Rest.CurLevel = 100f;
            this.psychology.thoughts.TryGainThought(ThoughtDef.Named("HappyRobot"));

            foreach (var skill in this.skills.skills) {
                skill.passion = Passion.None;
            }

            this.mechaReady = true;
        }
    }
}
