using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class Fireball : WalkToTargetAndExecuteAction
    {
        private int xpChange;

        public Fireball(AutonomousCharacter character, GameObject target) : base("Fireball",character,target)
        {
            if (target.tag.Equals("Skeleton"))
            {
                this.xpChange = 5;
            }
            else if (target.tag.Equals("Orc"))
            {
                this.xpChange = 10;
            }
            else if (target.tag.Equals("Dragon"))
            {
                this.xpChange = 0;
            }
        }

        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);

            if (goal.Name == AutonomousCharacter.GAIN_XP_GOAL)
            {
                change += -this.xpChange;
            }

            return change;
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute()) return false;
            return this.Character.GameManager.characterData.Mana > 5;
        }

        public override bool CanExecute(WorldModel WorldModel)
        {
            if (!base.CanExecute(WorldModel)) return false;
            var mana = (int)WorldModel.GetProperty(Properties.MANA);
            return mana > 5;
        }

        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.Fireball(this.Target);
        }
        

        public override void ApplyActionEffects(WorldModel WorldModel)
        {
            base.ApplyActionEffects(WorldModel);

            var xpValue = WorldModel.GetGoalValue(AutonomousCharacter.GAIN_XP_GOAL);
            WorldModel.SetGoalValue(AutonomousCharacter.GAIN_XP_GOAL, xpValue - this.xpChange);
            
            var xp = (int)WorldModel.GetProperty(Properties.XP);
            WorldModel.SetProperty(Properties.XP, xp + this.xpChange);

            var mana = (int)WorldModel.GetProperty(Properties.MANA);
            WorldModel.SetProperty(Properties.MANA, mana - 5);

            //disables the target object so that it can't be reused again
            if (!Target.tag.Equals("Dragon"))
                WorldModel.SetProperty(this.Target.name, false);
        }

        public override float getHValue(WorldModel WorldModel)
        {
            var mana = (int)WorldModel.GetProperty(Properties.MANA);
            if (Target.tag.Equals("Dragon") || mana < 5)
                return 100;

            else if (Target.tag.Equals("Orc"))
                return base.getHValue(WorldModel) + 1;

            else if (Target.tag.Equals("Skeleton"))
                return base.getHValue(WorldModel) + 5;

            else
                return base.getHValue(WorldModel);
        }
    }
}
