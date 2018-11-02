using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using UnityEngine;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;

namespace Assets.Scripts.DecisionMakingActions
{
    public abstract class WalkToTargetAndExecuteAction : Action
    {
        protected AutonomousCharacter Character { get; set; }

        protected GameObject Target { get; set; }

        protected WalkToTargetAndExecuteAction(string actionName, AutonomousCharacter character, GameObject target) : base(actionName+"("+target.name+")")
        {
            this.Character = character;
            this.Target = target;
        }

        public override float GetDuration()
        {
            return this.GetDuration(this.Character.Character.KinematicData.position);
        }

        public override float GetDuration(WorldModel WorldModel)
        {
            var position = (Vector3)WorldModel.GetProperty(Properties.POSITION);
            return this.GetDuration(position);
        }

        private float GetDuration(Vector3 currentPosition)
        {
            var distance = this.Character.AStarPathFinding.Heuristic.H(currentPosition, this.Target.transform.position);
            return distance / this.Character.Character.MaxSpeed;
        }

        public override float GetGoalChange(Goal goal)
        {
            if (goal.Name == AutonomousCharacter.BE_QUICK_GOAL)
            {
                return this.GetDuration();
            }
            else return 0;
        }

        public override bool CanExecute()
        {
            return this.Target != null;
        }

        public override bool CanExecute(WorldModel WorldModel)
        {
            if (this.Target == null) return false;
            var targetEnabled = (bool)WorldModel.GetProperty(this.Target.name);
            return targetEnabled;
        }

        public override void Execute()
        {
            this.Character.StartPathfinding(this.Target.transform.position);
        }


        public override void ApplyActionEffects(WorldModel WorldModel)
        {
            var duration = this.GetDuration(WorldModel);

            var quicknessValue = WorldModel.GetGoalValue(AutonomousCharacter.BE_QUICK_GOAL);
            WorldModel.SetGoalValue(AutonomousCharacter.BE_QUICK_GOAL, quicknessValue + duration*0.1f);

            var time = (float)WorldModel.GetProperty(Properties.TIME);
            WorldModel.SetProperty(Properties.TIME, time + duration);

            WorldModel.SetProperty(Properties.POSITION, Target.transform.position);
        }

        public override float getHValue(WorldModel WorldModel)
        {
  
            var distance = this.Character.AStarPathFinding.Heuristic.H((Vector3)WorldModel.GetProperty(Properties.POSITION), this.Target.transform.position);
            //var distance = this.Character.AStarPathFinding.Heuristic.H(Character.gameObject.transform.position, this.Target.transform.position);
            //o primeiro numero vai limitar o range de valores possiveis neste caso 0 - 10
            //o dividendo vai regular a precisao da escala
            //o divisor vai vai regular o inicio da escala : (10 - 10/1+0) = 0 

            //best
            return 1 - 50 / (50 + distance);

            //return 10 - 10 / (1 + distance);
            //return 5 - 280 / (60 + distance);
            //return 1 - 1000 / (1000 + x)

        }


    }
}
