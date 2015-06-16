using UnityEngine;
using System.Collections;

public class NodeSort {

	private ArrayList nodes = new ArrayList();

	// Return length of arrayList
	public int Length {
		get { return this.nodes.Count; }
	}

	// Return arrayList contains the node
	public bool Contains(object node) {
		for (int cnt = 0; cnt < nodes.Count; cnt++) {
			if(((Node)node).position == ((Node)nodes[cnt]).position)
				return true;
		}
		return false;
	}

	// Get the first node
	public Node First() {
		if (this.nodes.Count > 0)
			return (Node)this.nodes [0];
		return null;
	}

	// Add a new node
	public void Push(Node node) {
		this.nodes.Add (node);
		this.nodes.Sort ();
	}

	// Remove the node
	public void Remove(Node node) {
		this.nodes.Remove (node);
		this.nodes.Sort ();
	}
}
