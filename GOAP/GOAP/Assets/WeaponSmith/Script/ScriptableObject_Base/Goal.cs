using System;
using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Goal", fileName = "GoalDefault")]
public class Goal : ScriptableObject
{
	public States GoalStates;
	internal int  Priority = 0;

	private void OnValidate()
	{
		GoalStates.Update();
	}
}

}
