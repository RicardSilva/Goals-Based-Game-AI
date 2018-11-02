using Assets.Scripts.DecisionMakingActions;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    public class FutureStateWorldModel : WorldModel
    {

        public const bool BestInRandom = true;
        protected GameManager GameManager { get; set; }
        protected int NextPlayer { get; set; }
        protected Action NextEnemyAction { get; set; }
        protected Action[] NextEnemyActions { get; set; }

        public FutureStateWorldModel(GameManager gameManager, List<Action> actions) : base(actions)
        {
            this.GameManager = gameManager;
            this.NextPlayer = 0;
        }

        public FutureStateWorldModel(FutureStateWorldModel parent) : base(parent)
        {
            this.GameManager = parent.GameManager;
        }

        public GameObject getChestProtector(GameObject chest)
        {
            return this.GameManager.chestProtector[chest];
        }

        public override WorldModel GenerateChildWorldModel()
        {
            return new FutureStateWorldModel(this);
        }

        public override Action getNextRandomAction(System.Random RandomGenerator)
        {
            if (this.NextPlayer == 1)
            {
                var actions = this.NextEnemyActions;
                if (actions.Length > 0)
                {
                    return actions[RandomGenerator.Next(actions.Length)];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var actions = base.GetExecutableActions();
                if (actions.Length > 0)
                {
                    return actions[RandomGenerator.Next(actions.Length)];
                }
                else
                {
                    return null;
                }
            }
        }

        public override Action getNextBiasRandomAction(System.Random RandomGenerator, WorldModel WorldModel)
        {
            if (this.NextPlayer == 1)
            {
                var actions = this.NextEnemyActions;
                if (actions.Length > 0)
                {
                    return actions[RandomGenerator.Next(actions.Length)];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                var actions = base.GetExecutableActions();
                float[] actionsAccumulatedValues = new float[actions.Length];
                float[] actionsValues = new float[actions.Length];
                float accumulation = 0;
                if (actions.Length > 0)
                {
                    for (int actionIndex = 0; actionIndex < actions.Length; actionIndex++)
                    {
                        var actionValue = actions[actionIndex].getHValue(WorldModel);

                        
                        accumulation += 10 / (actionValue + 1);

                        actionsValues[actionIndex] = actionValue; 
                        //actionsAccumulatedValues[actionIndex] = 100 / (actionValue + 1);
                        actionsAccumulatedValues[actionIndex] = accumulation;

                    }

                    var roundUp = UnityEngine.Mathf.CeilToInt(accumulation);
                    int random = RandomGenerator.Next(roundUp);
                    float smallestValue = Mathf.Infinity;
                    int index = 0;
                    for (int actionIndex = 0; actionIndex < actions.Length; actionIndex++)
                    {
                        var value = actionsAccumulatedValues[actionIndex];
                        if (random <= value) {
                            
                            index = actionIndex;
                            //best in random
                            /*if(smallestValue > actionsValues[actionIndex])
                            {
                                index = actionIndex;
                                smallestValue = actionsValues[actionIndex];
                            }
                            
                            if (random + 1 <= value)*/
                                break;
                        }
                    }
                    if (actions[index].Name.Contains("PickUpChest"))
                    {
                        int i = index;
                        var value = actionsAccumulatedValues[i];
                    }               
                    return actions[index];
                }
                else
                {
                    return null;
                }
            }
        }

        public override bool IsTerminal()
        {
            int HP = (int)this.GetProperty(Properties.HP);
            float time = (float)this.GetProperty(Properties.TIME);
            int money = (int)this.GetProperty(Properties.MONEY);

            return HP <= 0 ||  time >= 200 || money == 25;
        }

        public override float GetScore()
        {
            int money = (int)this.GetProperty(Properties.MONEY);

            if (money == 25)
            {
                return 1.0f;
            }
            else return 0.0f;
        }

        public override int GetNextPlayer()
        {
            return this.NextPlayer;
        }

        public override void CalculateNextPlayer()
        {
            Vector3 position = (Vector3)this.GetProperty(Properties.POSITION);
            bool enemyEnabled;

            //basically if the character is close enough to an enemy, the next player will be the enemy.
            foreach (var enemy in this.GameManager.enemies)
            {
                enemyEnabled = (bool) this.GetProperty(enemy.name);
                if (enemyEnabled && (enemy.transform.position - position).sqrMagnitude <= 400)
                {
                    this.NextPlayer = 1;
                    this.NextEnemyAction = new SwordAttack(this.GameManager.autonomousCharacter, enemy);
                    this.NextEnemyActions = new Action[] { this.NextEnemyAction };
                    return; 
                }
            }
            this.NextPlayer = 0;
            //if not, then the next player will be player 0
        }

        public override Action GetNextAction()
        {
            Action action;
            if (this.NextPlayer == 1)
            {
                action = this.NextEnemyAction;
                this.NextEnemyAction = null;
                return action;
            }
            else return base.GetNextAction();
        }

        public override Action[] GetExecutableActions()
        {
            if (this.NextPlayer == 1)
            {
                return this.NextEnemyActions;
            }
            else return base.GetExecutableActions();
        }

    }
}
