//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
//{
//    public class WorldModel
//    {
//        private Dictionary<string, object> Properties { get; set; }
//        private List<Action> Actions { get; set; }
//        protected IEnumerator<Action> ActionEnumerator { get; set; } 

//        private Dictionary<string, float> GoalValues { get; set; } 

//        protected WorldModel Parent { get; set; }

//        public WorldModel(List<Action> actions)
//        {
//            this.Properties = new Dictionary<string, object>();
//            this.GoalValues = new Dictionary<string, float>();
//            this.Actions = actions;
//            this.ActionEnumerator = actions.GetEnumerator();
//        }

//        public WorldModel(WorldModel parent)
//        {
//            this.Properties = new Dictionary<string, object>();
//            this.GoalValues = new Dictionary<string, float>();
//            this.Actions = parent.Actions;
//            this.Parent = parent;
//            this.ActionEnumerator = this.Actions.GetEnumerator();
//        }

//        public virtual Action getNextRandomAction(System.Random RandomGenerator)
//        {
//            var actions = GetExecutableActions();
//            if (actions.Length > 0)
//            {
//                return actions[RandomGenerator.Next(actions.Length)];
//            }
//            else
//            {
//                return null;
//            }  
//        }
//        public void sortActions()
//        {
//            List<Action> actions = new List<Action>();
//            actions.AddRange(this.Actions);
//            actions.Sort((item1, item2) => item1.getHValue(this) < item2.getHValue(this) ? 1 : 0);
//            this.Actions = actions;
//        }

//        public virtual Action getNextBiasRandomAction(System.Random RandomGenerator, WorldModel WorldModel)
//        {
//            var actions = GetExecutableActions();
//            float[] actionsAccumulatedValues = new float[actions.Length];
//            float accumulation = 0;
//            if (actions.Length > 0)
//            {
//                for (int actionIndex = 0; actionIndex < actions.Length; actionIndex++)
//                {
//                    var actionValue = actions[actionIndex].getHValue(WorldModel);

//                    accumulation += 10 / (actionValue + 1);

//                    //actionsAccumulatedValues[actionIndex] = 100 / (actionValue + 1);
//                    actionsAccumulatedValues[actionIndex] = accumulation;

//                }

//                var roundUp = UnityEngine.Mathf.CeilToInt(accumulation);
//                int random = RandomGenerator.Next(roundUp);
//                //float biggestValue = Mathf.NegativeInfinity;
//                int index = 0;
//                for (int actionIndex = 0; actionIndex < actions.Length; actionIndex++)
//                {
//                    var value = actionsAccumulatedValues[actionIndex];
//                    if (random <= value)
//                    {
//                        //if (biggestValue <= value) {
//                        //biggestValue = value;
//                        index = actionIndex;
//                        break;
//                    }
//                }        
//                return actions[index];
//            }
//            else
//            {
//                return null;
//            }
//        }

//        public virtual object GetProperty(string propertyName)
//        {
//            //recursive implementation of WorldModel
//            if (this.Properties.ContainsKey(propertyName))
//            {
//                return this.Properties[propertyName];
//            }
//            else if (this.Parent != null)
//            {
//                return this.Parent.GetProperty(propertyName);
//            }
//            else
//            {
//                return null;
//            }
//        }

//        public virtual void SetProperty(string propertyName, object value)
//        {
//            this.Properties[propertyName] = value;
//        }

//        public virtual float GetGoalValue(string goalName)
//        {
//            //recursive implementation of WorldModel
//            if (this.GoalValues.ContainsKey(goalName))
//            {
//                return this.GoalValues[goalName];
//            }
//            else if (this.Parent != null)
//            {
//                return this.Parent.GetGoalValue(goalName);
//            }
//            else
//            {
//                return 0;
//            }
//        }

//        public virtual void SetGoalValue(string goalName, float value)
//        {
//            var limitedValue = value;
//            if (value > 10.0f)
//            {
//                limitedValue = 10.0f;
//            }

//            else if (value < 0.0f)
//            {
//                limitedValue = 0.0f;
//            }

//            this.GoalValues[goalName] = limitedValue;
//        }

//        public virtual WorldModel GenerateChildWorldModel()
//        {
//            return new WorldModel(this);
//        }

//        public float CalculateDiscontentment(List<Goal> goals)
//        {
//            var discontentment = 0.0f;

//            foreach (var goal in goals)
//            {
//                var newValue = this.GetGoalValue(goal.Name);

//                discontentment += goal.GetDiscontentment(newValue);
//            }

//            return discontentment;
//        }

//        public virtual Action GetNextAction()
//        {
//            Action action = null;
//            //returns the next action that can be executed or null if no more executable actions exist
//            if (this.ActionEnumerator.MoveNext())
//            {
//                action = this.ActionEnumerator.Current;
//            }

//            while (action != null && !action.CanExecute(this))
//            {
//                if (this.ActionEnumerator.MoveNext())
//                {
//                    action = this.ActionEnumerator.Current;    
//                }
//                else
//                {
//                    action = null;
//                }
//            }

//            return action;
//        }

//        public virtual Action[] GetExecutableActions()
//        {
//            return this.Actions.Where(a => a.CanExecute(this)).ToArray();
//        }

//        public virtual bool IsTerminal()
//        {
//            return true;
//        }
        

//        public virtual float GetScore()
//        {
//            return 0.0f;
//        }

//        public virtual int GetNextPlayer()
//        {
//            return 0;
//        }

//        public virtual void CalculateNextPlayer()
//        {
//        }
//    }
//}
