using Assets.Scripts.GameManager;
using System;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
/**/
namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSBiasedPlayout : MCTS
    {
        GOB.Action lastAction;
        float lastActionValue;
        public const bool BiasSelection = true;
        public const bool BestSelection = false;

        List<GOB.Action> lastestActions;

        public MCTSBiasedPlayout(CurrentStateWorldModel currentStateWorldModel) : base(currentStateWorldModel)
        {
            if (BestSelection)
                lastestActions = new List<GOB.Action>();
        }
        public override void InitializeMCTSearch()
        {
            base.InitializeMCTSearch();
            lastAction = null;
            lastActionValue = UnityEngine.Mathf.Infinity;
            if(BestSelection)
                lastestActions.Clear();
        }
        protected override Reward Playout(WorldModel initialPlayoutState)
        {
            FutureStateWorldModel currentState = initialPlayoutState.GenerateChildWorldModel() as FutureStateWorldModel;
            GOB.Action randomAction;
            int currentDepth = 0;
            while (!currentState.IsTerminal())
            {
                randomAction = currentState.getNextBiasRandomAction(base.RandomGenerator, currentState);
                randomAction.ApplyActionEffects(currentState);
                currentState.CalculateNextPlayer();
                currentDepth++;
            }
            if (currentDepth > this.MaxPlayoutDepthReached) this.MaxPlayoutDepthReached = currentDepth;
            return new Reward() { PlayerID = currentState.GetNextPlayer(), Value = currentState.GetScore() };
        }
        protected override MCTSNode Selection(MCTSNode initialNode)
        {
            GOB.Action nextAction = initialNode.State.GetNextAction();
            var currentWorld = initialNode.State;
            if (BiasSelection)
            { 
                if (lastAction != null)
                {
                    //vai estabilizando o mcts para valores mais baixos de HValues
                    while(nextAction!=null &&  nextAction.getHValue(currentWorld) > lastActionValue)
                    {
                        nextAction = initialNode.State.GetNextAction();
                    }
                    //senao houver HValues mais baixos faz reset
                    lastAction = nextAction;
                    if (lastAction != null)
                        lastActionValue = nextAction.getHValue(currentWorld);
                }
                else
                {
                    lastAction = nextAction;
                    if (lastAction != null)
                        lastActionValue = nextAction.getHValue(currentWorld);
                }
            }
            else if (BestSelection)
            {
                var actions = currentWorld.GetExecutableActions();
                var smallestNumber = UnityEngine.Mathf.Infinity;
                GOB.Action smallestAction = null;
                if (actions != null) { 
                    foreach (GOB.Action a in actions)
                    {
                        var value = a.getHValue(currentWorld);
                        if (smallestNumber > value && !lastestActions.Contains(a))
                        {
                            smallestNumber = value;
                            smallestAction = a;
                        }
                    }
                    lastestActions.Add(smallestAction);
                    nextAction = smallestAction;
                }
            }

            //nextAction = currentNode.State.GetExecutableActions()[RandomGenerator.Next(currentNode.State.GetExecutableActions().Length)];
            /*
            while v is nonterminal do
                    if v not fully expanded then
                        return Expand(v)
                    else
                        v < -BestChild(v)
            return v
            */
            MCTSNode currentNode = initialNode;
            while (!currentNode.State.IsTerminal())
            {

                if (nextAction != null)
                {
                    return Expand(currentNode, nextAction);
                }
                else
                {
                    currentNode = BestUCTChild(currentNode);
                    nextAction = currentNode.State.GetNextAction();
                }

            }

            return currentNode;
        }

        /*if(nextAction != null) {
                var actions = initialNode.State.GetExecutableActions();
                float smallestValue = UnityEngine.Mathf.Infinity;
                int index = 0;
                for (int actionIndex = 0; actionIndex < actions.Length; actionIndex++)
                {
                    var value = actions[actionIndex].getHValue(initialNode.State);
                    if (smallestValue > value)
                    {
                        index = actionIndex;
                        smallestValue = value;
                    }
                }
                nextAction = actions[index];
            }*/
        /*if(nextAction != null) { 
            if (lastActionValue != UnityEngine.Mathf.Infinity)
            {
                //vai estabilizando o mcts para valores mais baixos de HValues

                var nextActionValue = nextAction.getHValue(currentWorld);
                while (nextActionValue > lastActionValue )
                {
                    nextAction = initialNode.State.GetNextAction();
                    if (nextAction == null)
                        break;
                    nextActionValue = nextAction.getHValue(currentWorld);
                }

                //senao houver HValues mais baixos faz reset
                lastActionValue = nextActionValue;
            }
            else
            {
                while(nextAction!= null && nextAction.getHValue(initialNode.State) >= 100)
                    nextAction = initialNode.State.GetNextAction();

                lastActionValue = nextAction.getHValue(currentWorld);
            }*/
    }
}
/**/