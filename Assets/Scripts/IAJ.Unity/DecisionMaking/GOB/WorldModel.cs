using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
{
    public class WorldModel
    {
        private object[] Properties { get; set; }
        private float[] Goals { get; set; }

        private List<Action> Actions { get; set; }
        protected IEnumerator<Action> ActionEnumerator { get; set; }

        protected WorldModel Parent { get; set; }

        public WorldModel(List<Action> actions)
        {
            this.Properties = new object[22];
            this.Goals = new float[4];
            this.Actions = actions;
            this.ActionEnumerator = actions.GetEnumerator();
        }

        public WorldModel(WorldModel parent)
        {
            this.Properties = new object[22];
            this.Properties[0] = parent.GetProperty("HP");
            this.Properties[1] = parent.GetProperty("MAXHP");
            this.Properties[2] = parent.GetProperty("Mana");
            this.Properties[3] = parent.GetProperty("XP");
            this.Properties[4] = parent.GetProperty("Money");
            this.Properties[5] = parent.GetProperty("Level");
            this.Properties[6] = parent.GetProperty("Time");
            this.Properties[7] = parent.GetProperty("Position");
            this.Properties[8] = parent.GetProperty("Dragon");
            this.Properties[9] = parent.GetProperty("Orc");
            this.Properties[10] = parent.GetProperty("Orc (1)");
            this.Properties[11] = parent.GetProperty("Skeleton (2)");
            this.Properties[12] = parent.GetProperty("Skeleton (3)");
            this.Properties[13] = parent.GetProperty("ManaPotion");
            this.Properties[14] = parent.GetProperty("ManaPotion (1)");
            this.Properties[15] = parent.GetProperty("HealthPotion");
            this.Properties[16] = parent.GetProperty("HealthPotion (1)");
            this.Properties[17] = parent.GetProperty("Chest");
            this.Properties[18] = parent.GetProperty("Chest (1)");
            this.Properties[19] = parent.GetProperty("Chest (2)");
            this.Properties[20] = parent.GetProperty("Chest (3)");
            this.Properties[21] = parent.GetProperty("Chest (4)");



            this.Goals = new float[4];
            this.Goals[0] = parent.GetGoalValue("Survive");
            this.Goals[1] = parent.GetGoalValue("GainXP");
            this.Goals[2] = parent.GetGoalValue("BeQuick");
            this.Goals[3] = parent.GetGoalValue("GetRich");

            this.Actions = parent.Actions;
            this.ActionEnumerator = this.Actions.GetEnumerator();
        }

        public virtual object GetProperty(string propertyName)
        {
            if (propertyName.Equals("HP")) return this.Properties[0];
            if (propertyName.Equals("MAXHP")) return this.Properties[1];
            if (propertyName.Equals("Mana")) return this.Properties[2];
            if (propertyName.Equals("XP")) return this.Properties[3];
            if (propertyName.Equals("Money")) return this.Properties[4];
            if (propertyName.Equals("Level")) return this.Properties[5];
            if (propertyName.Equals("Time")) return this.Properties[6];
            if (propertyName.Equals("Position")) return this.Properties[7];
            if (propertyName.Equals("Dragon")) return this.Properties[8];
            if (propertyName.Equals("Orc")) return this.Properties[9];
            if (propertyName.Equals("Orc (1)")) return this.Properties[10];
            if (propertyName.Equals("Skeleton (2)")) return this.Properties[11];
            if (propertyName.Equals("Skeleton (3)")) return this.Properties[12];
            if (propertyName.Equals("ManaPotion")) return this.Properties[13];
            if (propertyName.Equals("ManaPotion (1)")) return this.Properties[14];
            if (propertyName.Equals("HealthPotion")) return this.Properties[15];
            if (propertyName.Equals("HealthPotion (1)")) return this.Properties[16];
            if (propertyName.Equals("Chest")) return this.Properties[17];
            if (propertyName.Equals("Chest (1)")) return this.Properties[18];
            if (propertyName.Equals("Chest (2)")) return this.Properties[19];
            if (propertyName.Equals("Chest (3)")) return this.Properties[20];
            if (propertyName.Equals("Chest (4)")) return this.Properties[21];
            return null;
        }

        public virtual void SetProperty(string propertyName, object value)
        {
            if (propertyName.Equals("HP")) this.Properties[0] = value;
            if (propertyName.Equals("MAXHP")) this.Properties[1] = value;
            if (propertyName.Equals("Mana")) this.Properties[2] = value;
            if (propertyName.Equals("XP")) this.Properties[3] = value;
            if (propertyName.Equals("Money")) this.Properties[4] = value;
            if (propertyName.Equals("Level")) this.Properties[5] = value;
            if (propertyName.Equals("Time")) this.Properties[6] = value;
            if (propertyName.Equals("Position")) this.Properties[7] = value;
            if (propertyName.Equals("Dragon")) this.Properties[8] = value;
            if (propertyName.Equals("Orc")) this.Properties[9] = value;
            if (propertyName.Equals("Orc (1)")) this.Properties[10] = value;
            if (propertyName.Equals("Skeleton (2)")) this.Properties[11] = value;
            if (propertyName.Equals("Skeleton (3)")) this.Properties[12] = value;
            if (propertyName.Equals("ManaPotion")) this.Properties[13] = value;
            if (propertyName.Equals("ManaPotion (1)")) this.Properties[14] = value;
            if (propertyName.Equals("HealthPotion")) this.Properties[15] = value;
            if (propertyName.Equals("HealthPotion (1)")) this.Properties[16] = value;
            if (propertyName.Equals("Chest")) this.Properties[17] = value;
            if (propertyName.Equals("Chest (1)")) this.Properties[18] = value;
            if (propertyName.Equals("Chest (2)")) this.Properties[19] = value;
            if (propertyName.Equals("Chest (3)")) this.Properties[20] = value;
            if (propertyName.Equals("Chest (4)")) this.Properties[21] = value;
        }

        public virtual float GetGoalValue(string goalName)
        {
            if (goalName.Equals("Survive")) return this.Goals[0];
            if (goalName.Equals("GainXP")) return this.Goals[1];
            if (goalName.Equals("BeQuick")) return this.Goals[2];
            if (goalName.Equals("GetRich")) return this.Goals[3];
            return 0;
        }

        public virtual void SetGoalValue(string goalName, float value)
        {
            var limitedValue = value;
            if (value > 10.0f)
            {
                limitedValue = 10.0f;
            }

            else if (value < 0.0f)
            {
                limitedValue = 0.0f;
            }

            if (goalName.Equals("Survive")) this.Goals[0] = limitedValue;
            if (goalName.Equals("GainXP")) this.Goals[1] = limitedValue;
            if (goalName.Equals("BeQuick")) this.Goals[2] = limitedValue;
            if (goalName.Equals("GetRich")) this.Goals[3] = limitedValue;
        }

        public virtual WorldModel GenerateChildWorldModel()
        {
            return new WorldModel(this);
        }

        public float CalculateDiscontentment(List<Goal> goals)
        {
            var discontentment = 0.0f;

            foreach (var goal in goals)
            {
                var newValue = this.GetGoalValue(goal.Name);

                discontentment += goal.GetDiscontentment(newValue);
            }

            return discontentment;
        }

        public virtual Action GetNextAction()
        {
            Action action = null;
            //returns the next action that can be executed or null if no more executable actions exist
            if (this.ActionEnumerator.MoveNext())
            {
                action = this.ActionEnumerator.Current;
            }

            while (action != null && !action.CanExecute(this))
            {
                if (this.ActionEnumerator.MoveNext())
                {
                    action = this.ActionEnumerator.Current;
                }
                else
                {
                    action = null;
                }
            }

            return action;
        }

        public virtual Action getNextRandomAction(System.Random RandomGenerator)
        {
            var actions = GetExecutableActions();
            if (actions.Length > 0)
            {
                return actions[RandomGenerator.Next(actions.Length)];
            }
            else
            {
                return null;
            }
        }
        public void sortActions()
        {
            List<Action> actions = new List<Action>();
            actions.AddRange(this.Actions);
            actions.Sort((item1, item2) => item1.getHValue(this) < item2.getHValue(this) ? 1 : 0);
            this.Actions = actions;
        }

        public virtual Action getNextBiasRandomAction(System.Random RandomGenerator, WorldModel WorldModel)
        {
            var actions = GetExecutableActions();
            float[] actionsAccumulatedValues = new float[actions.Length];
            float accumulation = 0;
            if (actions.Length > 0)
            {
                for (int actionIndex = 0; actionIndex < actions.Length; actionIndex++)
                {
                    var actionValue = actions[actionIndex].getHValue(WorldModel);

                    accumulation += 10 / (actionValue + 1);

                    actionsAccumulatedValues[actionIndex] = accumulation;

                }

                var roundUp = UnityEngine.Mathf.CeilToInt(accumulation);
                int random = RandomGenerator.Next(roundUp);
                int index = 0;
                for (int actionIndex = 0; actionIndex < actions.Length; actionIndex++)
                {
                    var value = actionsAccumulatedValues[actionIndex];
                    if (random <= value)
                    {

                        index = actionIndex;
                        break;
                    }
                }
                return actions[index];
            }
            else
            {
                return null;
            }
        }

        public virtual Action[] GetExecutableActions()
        {
            return this.Actions.Where(a => a.CanExecute(this)).ToArray();
        }

        public virtual bool IsTerminal()
        {
            return true;
        }


        public virtual float GetScore()
        {
            return 0.0f;
        }

        public virtual int GetNextPlayer()
        {
            return 0;
        }

        public virtual void CalculateNextPlayer()
        {
        }

    }
}
