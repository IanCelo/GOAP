using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/State", fileName = "DefaultState")]
public class StateKey : ScriptableObject
{
	public string Key;
	public int    Hash;
	
	private void OnValidate()
	{
		Key  = ToString().Split(' ')[0];
		Hash = Key.GetHashCode();
	}
}
