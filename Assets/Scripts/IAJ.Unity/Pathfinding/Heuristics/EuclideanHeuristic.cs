﻿using RAIN.Navigation.Graph;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.Heuristics
{
    public class EuclidianHeuristic : IHeuristic
    {
        public float H(NavigationGraphNode node, NavigationGraphNode goalNode) { 
            return Vector3.Distance(goalNode.LocalPosition, node.LocalPosition);
        }

        public float H(Vector3 startPosition, Vector3 goalPosition)
        {

            return Vector3.Distance(startPosition, goalPosition);
        }
    }
}
