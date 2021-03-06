﻿using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class SwordAttack : WalkToTargetAndExecuteAction
    {
        private int hpChange;
        private int xpChange;

        public SwordAttack(AutonomousCharacter character, GameObject target) : base("SwordAttack",character,target)
        {
           
            if (target.tag.Equals("Skeleton"))
            {
                this.hpChange = -5;
                this.xpChange = 5;
            }
            else if (target.tag.Equals("Orc"))
            {
                this.hpChange = -10;
                this.xpChange = 10;
            }
            else if (target.tag.Equals("Dragon"))
            {
                this.hpChange = -20;
                this.xpChange = 20;
            }
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.SURVIVE_GOAL)
            {
                change += -this.hpChange;
            }
            else if (goal.Name == AutonomousCharacter.GAIN_XP_GOAL)
            {
                change += -this.xpChange;
            }
            
            return change;
        }


        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.SwordAttack(this.Target);
        }

        public override void ApplyActionEffects(WorldModel WorldModel)
        {
            base.ApplyActionEffects(WorldModel);

            var xpValue = WorldModel.GetGoalValue(AutonomousCharacter.GAIN_XP_GOAL);
            WorldModel.SetGoalValue(AutonomousCharacter.GAIN_XP_GOAL,xpValue-this.xpChange); 

            var surviveValue = WorldModel.GetGoalValue(AutonomousCharacter.SURVIVE_GOAL);
            WorldModel.SetGoalValue(AutonomousCharacter.SURVIVE_GOAL,surviveValue-this.hpChange);

            var hp = (int)WorldModel.GetProperty(Properties.HP);
            WorldModel.SetProperty(Properties.HP,hp + this.hpChange);
            var xp = (int)WorldModel.GetProperty(Properties.XP);
            WorldModel.SetProperty(Properties.XP, xp + this.xpChange);
           

            //disables the target object so that it can't be reused again
            WorldModel.SetProperty(this.Target.name,false);
        }

        public override float getHValue(WorldModel WorldModel)
        {
            
            int fututeHp = ((int) WorldModel.GetProperty(Properties.HP)) + hpChange;
            if (fututeHp <= 0)
                return 100; 
            return base.getHValue(WorldModel);
        }
    }
}
