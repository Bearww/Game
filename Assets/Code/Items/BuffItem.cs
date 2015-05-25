using UnityEngine;
using System.Collections;

public class BuffItem : MonoBehaviour {

	public float itemDuration;
	public ItemEffect itemEffect;
}

public enum ItemEffect
{
	Move,
	Role,
	GameIime,
	Map
}