using UnityEngine;
using System;
using System.Collections;

public class Node : IComparable {

	// Current cost
	public float G_Cost;

	// Estimate cos
	public float H_Cost;

	// Node can move or not
	public bool isObstacle;
	// Record the node's parent
	public Node parent;
	// Position of the node
	public Vector3 position;

	// Initial the node
	public Node() {
		this.G_Cost = 0f;
		this.H_Cost = 0f;
		this.isObstacle = false;
		this.parent = null;
	}

	// Initial with position
	public Node(Vector3 pos) {
		this.G_Cost = 0f;
		this.H_Cost = 0f;
		this.isObstacle = false;
		this.parent = null;
		this.position = pos;
	}

	// Set the node to be an obstacle
	public void MarkAsObstacle() {
		this.isObstacle = true;
	}

	public int CompareTo(object obj) {
		Node node = (Node)obj;

		// New node's total cost is bigger, don't change
		if (this.G_Cost + this.H_Cost < node.G_Cost + node.H_Cost)
			return -1;

		// New node's total cost is smaller, change
		if (this.G_Cost + this.H_Cost > node.G_Cost + node.H_Cost)
			return 1;

		return -1;
	}
}
