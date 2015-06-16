using UnityEngine;
using System.Collections;

public class NodeManager : MonoBehaviour {

	// NodeManager Singleton
	private static NodeManager s_Instance = null;

	public static NodeManager instance {
		get {
			if(s_Instance == null) {
				s_Instance = FindObjectOfType(typeof(NodeManager)) as NodeManager;

				if(s_Instance == null)
					Debug.Log ("[NodeManager]Create error");
			}
			return s_Instance;
		}
	}

	// The number of row and column
	public int numOfRows;
	public int numOfColumns;

	// The grid cell size
	public float gridCellSize;

	// Record nodes
	public Node[,] nodes { get; set; }

	// Get the current node's position
	public Vector3 GetNodePosition(int col, int row) {
		Vector3 cellPosition = new Vector3 ();
		cellPosition.x = col * gridCellSize + gridCellSize / 2.0f;
		cellPosition.y = row * gridCellSize + gridCellSize / 2.0f;
		return cellPosition;
	}

	// Get the current node's row
	public int GetNodeRow(Vector3 position) {
		return (int)((2 * position.y - gridCellSize) / (2 * gridCellSize));
	}

	// Get the current node's column
	public int GetNodeColumn(Vector3 position) {
		return (int)((2 * position.x - gridCellSize) / (2 * gridCellSize));
	}

	// Return a closest node position
	public Vector3 GetNodeCenter(Vector3 position) {
		for (int col = 0; col < numOfColumns; col++) {
			for(int row = 0; row < numOfRows; row++) {
				if(col * gridCellSize <= position.x &&
				   position.x < (col + 1) * gridCellSize &&
				   row * gridCellSize <= position.y &&
				   position.y < (row + 1) * gridCellSize) {
					return GetNodePosition (col, row);
				}
			}
		}
		return Vector3.zero;
	}

	// Create the node
	public void CreateNodes() {
		nodes = new Node[numOfColumns, numOfRows];

		for (int col = 0; col < numOfColumns; col++) {
			for(int row = 0; row < numOfRows; row++) {
				Vector3 pos = GetNodePosition(col, row);
				Node node = new Node(pos);
				nodes[col, row] = node;
			}
		}
	}

	public void CreateNodes(int columns, int rows) {
		numOfColumns = columns;
		numOfRows = rows;
		CreateNodes ();
	}

	public void SetObstacel(int col, int row) {
		nodes[col, row].MarkAsObstacle();
	}

	// Get the current node's neighbors
	public void GetNeighbours(Node node, ArrayList neighbors) {
		Vector3 neighborPos = node.position;
		int row = GetNodeRow (neighborPos);
		int column = GetNodeColumn(neighborPos);

		// Bottom
		int leftNodeRow = row - 1;
		int leftNodeColumn = column;
		AssignNeighbour (leftNodeRow, leftNodeColumn, neighbors);
		// Top
		leftNodeRow = row + 1;
		leftNodeColumn = column;
		AssignNeighbour (leftNodeRow, leftNodeColumn, neighbors);
		// Right
		leftNodeRow = row;
		leftNodeColumn = column + 1;
		AssignNeighbour (leftNodeRow, leftNodeColumn, neighbors);
		// Left
		leftNodeRow = row;
		leftNodeColumn = column - 1;
		AssignNeighbour (leftNodeRow, leftNodeColumn, neighbors);
	}

	// Put neighbors to an ArrayList
	void AssignNeighbour(int row, int column, ArrayList neighbors) {
		if (row != -1 && column != -1 &&
			row < numOfRows && column < numOfColumns) {
			Node nodeToAdd = nodes[column, row];
			if(!nodeToAdd.isObstacle) {
				neighbors.Add (nodeToAdd);
			}
		}
	}
}
