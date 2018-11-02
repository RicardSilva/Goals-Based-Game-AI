using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Assets.Scripts.GameManager;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.MCTS
{
    public class MCTS
    {
        public const float C = 1.4f;
        public bool InProgress { get; private set; }
        public int MaxIterations { get; set; }
        public int MaxIterationsProcessedPerFrame { get; set; }
        public int MaxPlayoutDepthReached { get; set; }
        public int MaxSelectionDepthReached { get; set; }
        public float TotalProcessingTime { get; private set; }
        public MCTSNode BestFirstChild { get; set; }
        public List<GOB.Action> BestActionSequence { get; private set; }


        private int CurrentIterations { get; set; }
        private int CurrentIterationsInFrame { get; set; }
        private int CurrentDepth { get; set; }

        private CurrentStateWorldModel CurrentStateWorldModel { get; set; }
        private MCTSNode InitialNode { get; set; }
        protected System.Random RandomGenerator { get; set; }


        public MCTS(CurrentStateWorldModel currentStateWorldModel)
        {
            this.InProgress = false;
            this.CurrentStateWorldModel = currentStateWorldModel;
            this.MaxIterations = 100;
            this.MaxIterationsProcessedPerFrame = 10;
            this.RandomGenerator = new System.Random();
        }


        public virtual void InitializeMCTSearch()
        {
            this.MaxPlayoutDepthReached = 0;
            this.MaxSelectionDepthReached = 0;
            this.CurrentIterations = 0;
            this.CurrentDepth = 0;
            this.CurrentIterationsInFrame = 0;
            this.TotalProcessingTime = 0.0f;
            this.CurrentStateWorldModel.Initialize();
            this.InitialNode = new MCTSNode(this.CurrentStateWorldModel)
            {
                Action = null,
                Parent = null,
                PlayerID = 0
            };
            this.InProgress = true;
            this.BestFirstChild = null;
            this.BestActionSequence = new List<GOB.Action>();
        }

        public GOB.Action Run()
        {
            MCTSNode selectedNode;
            Reward reward;

            var startTime = Time.realtimeSinceStartup;
            this.CurrentIterationsInFrame = 0;
            
            for (; this.CurrentIterations < this.MaxIterations;)
            {
                for (; this.CurrentIterationsInFrame < this.MaxIterationsProcessedPerFrame; this.CurrentIterationsInFrame++)
                {
                    selectedNode = Selection(this.InitialNode);
                    reward = Playout(selectedNode.State);
                    Backpropagate(selectedNode, reward);
                }
                this.CurrentIterations++;
                break;

            }
            if (this.CurrentIterations >= MaxIterations)
            {
                InProgress = false;
            }
            this.BestActionSequence.Clear();
            MCTSNode bestNode = InitialNode;
            while (bestNode.ChildNodes.Count > 0)
            {
                this.BestActionSequence.Add(BestChild(bestNode).Action);
                bestNode = BestChild(bestNode);
                
            }

            this.BestFirstChild = BestChild(InitialNode);
            this.TotalProcessingTime += Time.realtimeSinceStartup - startTime;
            if (this.BestFirstChild == null)
                return null;
            return this.BestFirstChild.Action;
        }

        protected virtual MCTSNode Selection(MCTSNode initialNode)
        {
            GOB.Action nextAction = initialNode.State.GetNextAction();
            MCTSNode currentNode = initialNode;
            int currentDepth = 0;

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
                currentDepth++;
            }

            if (currentDepth > this.MaxSelectionDepthReached) this.MaxSelectionDepthReached = currentDepth;
            return currentNode;
        }

        protected virtual Reward Playout(WorldModel initialPlayoutState)
        {
            FutureStateWorldModel currentState = initialPlayoutState.GenerateChildWorldModel() as FutureStateWorldModel;
            GOB.Action randomAction;
            int currentDepth = 0;
            while (!currentState.IsTerminal())
            {
                
                randomAction = currentState.getNextRandomAction(this.RandomGenerator);
                randomAction.ApplyActionEffects(currentState);
                currentState.CalculateNextPlayer();
                currentDepth++;
            }
            if (currentDepth > this.MaxPlayoutDepthReached) this.MaxPlayoutDepthReached = currentDepth;
            return new Reward() { PlayerID = currentState.GetNextPlayer(), Value = currentState.GetScore()};
        }

        protected virtual void Backpropagate(MCTSNode node, Reward reward)
        {
            MCTSNode currentNode = node;
            while (currentNode != null)
            {

                currentNode.N = currentNode.N + 1;
                currentNode.Q = currentNode.Q + reward.GetRewardForNode(node);
                currentNode = currentNode.Parent;

            }
        }

        protected virtual MCTSNode Expand(MCTSNode parent, GOB.Action action)
        {
            FutureStateWorldModel newModel = parent.State.GenerateChildWorldModel() as FutureStateWorldModel;
            action.ApplyActionEffects(newModel);
            newModel.CalculateNextPlayer();

            MCTSNode childNode = new MCTSNode(newModel);
            childNode.Action = action;
            childNode.Parent = parent;

            parent.ChildNodes.Add(childNode);

            return childNode;
        }

        //gets the best child of a node, using the UCT formula
        protected virtual MCTSNode BestUCTChild(MCTSNode node)
        {
            float bestUCTValue = Mathf.NegativeInfinity;
            MCTSNode bestChild = null;
            float currentEstimation;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                currentEstimation = (node.ChildNodes[i].Q / node.ChildNodes[i].N) + C * Mathf.Sqrt(Mathf.Log(node.N) / node.ChildNodes[i].N);
                if (currentEstimation > bestUCTValue)
                {
                    bestUCTValue = currentEstimation;
                    bestChild = node.ChildNodes[i];
                }
            }
            return bestChild;
        }

        //this method is very similar to the bestUCTChild, but it is used to return the final action of the MCTS search, and so we do not care about
        //the exploration factor
        protected virtual MCTSNode BestChild(MCTSNode node)
        {
            float bestUCTValue = Mathf.NegativeInfinity;
            MCTSNode bestChild = null;
            float currentEstimation;
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                currentEstimation = (node.ChildNodes[i].Q / node.ChildNodes[i].N);

                if (currentEstimation > bestUCTValue)
                {
                    bestUCTValue = currentEstimation;
                    bestChild = node.ChildNodes[i];
                }
            }
            return bestChild;
        }
    }
}
