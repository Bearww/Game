using UnityEngine;
using System.Collections;

public class BuffItem : Item {

	public float  itemDuration;
	public ItemEffect itemEffect;	

	void Start () {
	
	}

	void Update () {
	
	}
}

public enum ItemEffect
{
	Move,
	Role,
	time,
	Map
}