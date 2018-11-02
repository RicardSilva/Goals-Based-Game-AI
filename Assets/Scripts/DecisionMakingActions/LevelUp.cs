using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;
using Assets.Scripts.GameManager;

namespace Assets.Scripts.DecisionMakingActions
{
    public class LevelUp : Action
    {
        public AutonomousCharacter Character { get; private set; }

        public LevelUp(AutonomousCharacter character) : base("LevelUp")
        {
            this.Character = character;
        }

        public override void ApplyActionEffects(WorldModel WorldModel)
        {
            int maxHP = (int)WorldModel.GetProperty(Properties.MAXHP);
            int level = (int)WorldModel.GetProperty(Properties.LEVEL);

            WorldModel.SetProperty(Properties.LEVEL, level + 1);
            WorldModel.SetProperty(Properties.MAXHP, maxHP + 10);
            WorldModel.SetProperty(Properties.HP, maxHP + 10);
        }

        public override bool CanExecute()
        {
            var level = this.Character.GameManager.characterData.Level;
            var xp = this.Character.GameManager.characterData.XP;

            if(level == 1)
            {
                return xp >= 10;
            }
            else if(level == 2)
            {
                return xp >= 30;
            }

            return false;
        }
        

        public override bool CanExecute(WorldModel WorldModel)
        {
            int xp = (int)WorldModel.GetProperty(Properties.XP);
            int level = (int)WorldModel.GetProperty(Properties.LEVEL);

            if (level == 1)
            {
                return xp >= 10;
            }
            else if (level == 2)
            {
                return xp >= 30;
            }

            return false;
        }

        public override void Execute()
        {
            this.Character.GameManager.LevelUp();
        }

        public override float GetDuration()
        {
            return 0.0f;
        }

        public override float GetDuration(WorldModel WorldModel)
        {
            return 0.0f;
        }

        public override float GetGoalChange(Goal goal)
        {
            return 0.0f;
        }

        public override float getHValue(WorldModel WorldModel)
        {
            int xp = (int)WorldModel.GetProperty(Properties.XP);
            int level = (int)WorldModel.GetProperty(Properties.LEVEL);

            if (level == 1 && xp >= 10)
            {
                return 0;
            }
            else if (level == 2 && xp >= 30)
            {
                return 0;
            }

            return 100;
        }
    }
}
