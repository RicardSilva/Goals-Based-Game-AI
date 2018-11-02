using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class PickUpChest : WalkToTargetAndExecuteAction
    {

        public PickUpChest(AutonomousCharacter character, GameObject target) : base("PickUpChest",character,target)
        {
        }


        public override float GetGoalChange(Goal goal)
        {
            var change = base.GetGoalChange(goal);
            if (goal.Name == AutonomousCharacter.GET_RICH_GOAL) change -= 5.0f;
            return change;
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute()) return false;
            return true;
        }

        public override bool CanExecute(WorldModel WorldModel)
        {
            if (!base.CanExecute(WorldModel)) return false;
            return true;
        }

        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.PickUpChest(this.Target);
        }

        public override void ApplyActionEffects(WorldModel WorldModel)
        {
        
            base.ApplyActionEffects(WorldModel);

            var goalValue = WorldModel.GetGoalValue(AutonomousCharacter.GET_RICH_GOAL);
            WorldModel.SetGoalValue(AutonomousCharacter.GET_RICH_GOAL, goalValue - 5.0f);

            var money = (int)WorldModel.GetProperty(Properties.MONEY);
            WorldModel.SetProperty(Properties.MONEY, money + 5);

            //disables the target object so that it can't be reused again
            WorldModel.SetProperty(this.Target.name, false);
        }

        public override float getHValue(WorldModel WorldModel)
        {
            var fw = WorldModel as FutureStateWorldModel;
            var chestProtector = fw.getChestProtector(this.Target);
            if (chestProtector != null && chestProtector.activeSelf)  
                    return 100;
            else
                return base.getHValue(WorldModel);
        }
    }
}
