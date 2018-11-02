using Assets.Scripts.GameManager;
using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using Assets.Scripts.IAJ.Unity.Utils;
using System;
using System.Collections.Generic;
using Action = Assets.Scripts.IAJ.Unity.DecisionMaking.GOB.Action;
using UnityEngine;
/**/
namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTSRAVE : MCTS
    {
        protected const float b = 1;
        protected List<Pair<int, Action>> ActionHistory { get; set; }
        public MCTSRAVE(CurrentStateWorldModel worldModel) : base(worldModel)
        {
            ActionHistory = new List<Pair<int, Action>>();
        }

        protected override MCTSNode BestUCTChild(MCTSNode node)
        {
            float MCTSValue;
            float RAVEValue;
            float UCTValue;
            float bestUCT = float.MinValue;
            MCTSNode bestNode = null;
            //step 1, calculate beta and 1-beta. beta does not change from child to child. So calculate this only once
            //TODO: implement
            float beta;
            float b = 1;
            beta = node.NRAVE / (node.N + node.NRAVE + 4 * node.N * node.NRAVE * b * b);

            float beta1 = 1 - beta;



            float currentEstimation;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                MCTSValue = (node.ChildNodes[i].Q / node.ChildNodes[i].N);
                var explorationFactor = C * Mathf.Sqrt(Mathf.Log(node.N) / node.ChildNodes[i].N);
                RAVEValue = ((beta1* MCTSValue)+(beta*(node.ChildNodes[i].QRAVE / node.ChildNodes[i].NRAVE))) + explorationFactor;
                UCTValue = MCTSValue + explorationFactor;

                currentEstimation = Math.Max(MCTSValue, RAVEValue);
                currentEstimation = Math.Max(UCTValue,currentEstimation);

                if (currentEstimation > bestUCT)
                {
                    bestUCT = currentEstimation;
                    bestNode = node.ChildNodes[i];
                }
            }
            return bestNode;

            //step 2, calculate the MCTS value, the RAVE value, and the UCT for each child and determine the best one

        }


        protected override Reward Playout(WorldModel initialPlayoutState)
        {
            /*
            *while s is nonterminal do
                chose a from Actions(s) uniformly at random
                s <- Result(s,a)
                return reward for state s
            */
            FutureStateWorldModel currentState = initialPlayoutState.GenerateChildWorldModel() as FutureStateWorldModel;

            GOB.Action randomAction;
            ActionHistory.Clear();
            int currentDepth = 0;
            Pair<int, Action> par = new Pair<int, Action>(0, new Action("asdasdas"));//cria com lixo para depois ser substituido
            while (!currentState.IsTerminal())
            {

                randomAction = currentState.getNextRandomAction(this.RandomGenerator);
                randomAction.ApplyActionEffects(currentState);
                currentState.CalculateNextPlayer();
                
                par.Left = currentState.GetNextPlayer();
                par.Right = randomAction;
                ActionHistory.Add(par);
                currentDepth++;

            }
            if (currentDepth > this.MaxPlayoutDepthReached) this.MaxPlayoutDepthReached = currentDepth;
            return new Reward() { PlayerID = currentState.GetNextPlayer(), Value = currentState.GetScore() };
        }

        protected override void Backpropagate(MCTSNode node, Reward reward)
        {
            /*
                        while v is not null do
                           N(v) <- N(v) + 1
                           Q(v) <- Q(v) + r(v, Player(Parent(v)))
                           v <- parent of v
                       */
            int player = 0;
            MCTSNode currentNode = node;
            Pair<int, Action> par = new Pair<int, Action>(0,new Action("asdasdas"));
            while (currentNode != null)
            {

                currentNode.N = currentNode.N + 1;
                currentNode.Q = currentNode.Q + reward.GetRewardForNode(node);

                if (currentNode.Parent != null)
                {
                    par.Left = currentNode.Parent.PlayerID;
                    par.Right = currentNode.Action;
                    ActionHistory.Add(par);
                }

                currentNode = currentNode.Parent;

                if(currentNode != null)
                {
                    player = currentNode.PlayerID;
                    /* protected List<Pair<int, Action>> ActionHistory { get; set; }*/
                    foreach (MCTSNode child in currentNode.ChildNodes)
                    {
                        par.Left = player;
                        par.Right = child.Action;
                        if(ActionHistory.Contains(par))//Pair<player,child.Action>)
                        {
                            child.NRAVE = child.NRAVE + 1;
                            child.QRAVE = child.QRAVE + reward.GetRewardForNode(child);

                        }



                    }


                }
                /*if v is not null do
                    p < -Player(v)
                    foreach (c in Children(v))
                        if ((p, A(c)) in actionHistory) do
                            Nrave(c) < -Nrave(c) + 1
                            Qrave(c) < -Qrave(c) + r(c, p)*/
            }
        }
    }
}
/**/