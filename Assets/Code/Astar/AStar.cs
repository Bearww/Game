using UnityEngine;
using System.Collections;

public class AStar : MonoBehaviour {

	public static NodeSort closedList, openList;

	private static float HeuristicEstimateCost(Node curNode, Node goalNode) {
		Vector3 vectorCost = goalNode.position - curNode.position;
		return vectorCost.magnitude;
	}

	public static ArrayList FindPath(Node start, Node goal) {
		openList = new NodeSort ();
		openList.Push (start);
		start.G_Cost = 0f;
		start.H_Cost = HeuristicEstimateCost (start, goal);

		closedList = new NodeSort ();
		Node node = null;

		while (openList.Length != 0) {
			node = openList.First ();

			// Push the current node
			closedList.Push (node);
			// Remove from openList
			openList.Remove (node);

			// Check the goal node
			if(node.position == goal.position) {
				return CalculatePath(node);
			}

			// Create an ArrayList to store the neighboring nodes
			ArrayList neighbours = new ArrayList();
			NodeManager.instance.GetNeighbours(node, neighbours);
		
			Node neighbourNode;

			for(int i = 0; i < neighbours.Count; i++) {
				neighbourNode = (Node)neighbours[i];

				if(!closedList.Contains(neighbourNode)) {
					float cost;
					float totalCost;

					float neighbourNodeEstCost;

					if(!openList.Contains(neighbourNode)) {
						// G
						cost = HeuristicEstimateCost(node, neighbourNode);
						totalCost = node.G_Cost + cost;

						// H
						neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

						neighbourNode.G_Cost = totalCost;
						neighbourNode.parent = node;
						neighbourNode.H_Cost = neighbourNodeEstCost;

						openList.Push (neighbourNode);
					}
					else {
						cost = HeuristicEstimateCost(node, neighbourNode);
						totalCost = node.G_Cost + cost;
						
						if(neighbourNode.G_Cost > totalCost) {
							neighbourNode.G_Cost = totalCost;
							neighbourNode.parent = node;
						}
					}
				}
			}
		}

		if(node.position != goal.position) {
			Debug.LogError("[A*]Goal Not Found");
			return null;
		}
		return CalculatePath (node);
	}

	private static ArrayList CalculatePath(Node node) {
		ArrayList list = new ArrayList();
		
		while (node != null) {
			list.Add(node);
			node = node.parent;
		}
		list.Reverse();
		return list;
	}
}
