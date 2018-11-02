using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;

namespace Assets.Scripts.DecisionMakingActions
{
    public class GetManaPotion : WalkToTargetAndExecuteAction
    {
        public GetManaPotion(AutonomousCharacter character, GameObject target) : base("GetManaPotion",character,target)
        {
        }

        public override bool CanExecute()
        {
            if (!base.CanExecute()) return false;
            return this.Character.GameManager.characterData.Mana < 10;
        }

        public override bool CanExecute(WorldModel WorldModel)
        {
            if (!base.CanExecute(WorldModel)) return false;

            var mana = (int)WorldModel.GetProperty(Properties.MANA);
            return mana < 10;
        }

        public override void Execute()
        {
            base.Execute();
            this.Character.GameManager.GetManaPotion(this.Target);
        }


        public override void ApplyActionEffects(WorldModel WorldModel)
        {
            base.ApplyActionEffects(WorldModel);
            WorldModel.SetProperty(Properties.MANA, 10);
            //disables the target object so that it can't be reused again
            WorldModel.SetProperty(this.Target.name, false);
        }

        public override float getHValue(WorldModel WorldModel)
        {
            var mana = (int)WorldModel.GetProperty(Properties.MANA);

            if (mana == 10)
                return 100;

            else if (mana >= 5)
                return base.getHValue(WorldModel) + 5;

            else if (mana < 5)
                return base.getHValue(WorldModel) + 1;

            else
                return base.getHValue(WorldModel);
        }

    }
}
