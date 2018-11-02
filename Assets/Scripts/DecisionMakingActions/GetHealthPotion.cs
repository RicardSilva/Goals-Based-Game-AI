using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class GetHealthPotion : WalkToTargetAndExecuteAction
    {
        public GetHealthPotion(AutonomousCharacter character, GameObject target) : base("GetHealthPotion",character,target)
        {
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute()) return false;
            return this.Character.GameManager.characterData.HP < 10;
        }

        public override bool CanExecute(WorldModel WorldModel)
        {
            if (!base.CanExecute(WorldModel)) return false;

            var hp = (int)WorldModel.GetProperty(Properties.HP);
            return hp < 10;
        }

        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.GetHealthPotion(this.Target);
        }

        public override void ApplyActionEffects(WorldModel WorldModel)
        {
            base.ApplyActionEffects(WorldModel);
            WorldModel.SetProperty(Properties.HP, 10);
            //disables the target object so that it can't be reused again
            WorldModel.SetProperty(this.Target.name, false);
        }
        public override float getHValue(WorldModel WorldModel)
        {
            var hp = (int)WorldModel.GetProperty(Properties.HP);
            var maxHp = (int)WorldModel.GetProperty(Properties.MAXHP);
            var halfHealth = maxHp * 0.5;
            
            if (hp == maxHp)
                return 100;

            else if (hp >= halfHealth)
                return base.getHValue(WorldModel) + 5;

            else if (hp < halfHealth)
                return base.getHValue(WorldModel) + 1;

            else
                return base.getHValue(WorldModel);
        }
    }
}
