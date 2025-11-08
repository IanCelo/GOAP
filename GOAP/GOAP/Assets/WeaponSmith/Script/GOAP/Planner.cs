using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GOAP_Planner
{

public static class Planner
{
	public  static List<Node> GetPlan(States _DefaultWorldState, List<Action> _Actions, Goal _Goal)
	{
		Node       parent = new(null, _DefaultWorldState, null);

		List<Node> leaves = new();
		int        smallestCost = int.MaxValue;

		float currentTime = Time.realtimeSinceStartup;
		BuildGraph(parent, ref leaves, _Actions, _Goal.GoalStates, ref smallestCost);

		Debug.Log($"Plan for goal {_Goal.name} took {Time.realtimeSinceStartup - currentTime} seconds");
		
		return GetBestPlan(ref leaves);
	}

	private static List<Node> GetBestPlan(ref List<Node> _Leaves)
	{
		if (_Leaves.Count == 0)
			return null;
		
		_Leaves.Sort((_Lhs, _Rhs) => _Lhs.Cost.CompareTo(_Rhs.Cost));
		Node endNode = _Leaves.First();
		
		Node currentNode = endNode;
		
		List<Node> chosenPlan = new();
		
		while (currentNode.Parent is not null)
		{
			chosenPlan.Insert(0, currentNode);
			currentNode = currentNode.Parent;
		}

		return chosenPlan;
	}
	
	private static void BuildGraph(Node _Parent, ref List<Node> _Leaves, List<Action> _AvailableActions, States _Goal, ref int _SmallestCost, bool _WasMoveAction = false)
	{
		foreach (Action action in _AvailableActions)
		{
			if (!action.IsValid(_Parent.WorldState)) continue;
			
			// Avoid doing to move actions in a row (because it's useless) to reduce recursion count
			if (action.IsMoveAction && _WasMoveAction) continue;

				States newWorldState = new(_Parent.WorldState);
			action.ApplyWorldChanges(ref newWorldState);

			Node thisNode = new(_Parent, newWorldState, action);

			// Pruning with heuristic
			if (thisNode.Cost > _SmallestCost)
				return;
			
			if (_Goal.Compare(newWorldState))
			{
				_Leaves.Add(thisNode);
				_SmallestCost = thisNode.Cost;
			}
			else
			{
				List<Action> actionsSubSet = new(_AvailableActions);
				actionsSubSet.Remove(action);
					
				BuildGraph(thisNode, ref _Leaves, actionsSubSet, _Goal, ref _SmallestCost, action.IsMoveAction);
			}
		}
	}
}

public class Node
{
	public Node Parent       { get; }
	public States WorldState { get; }
	public Action Action     { get; }

	internal readonly int Cost;

	public Node(Node _Parent, States _NewWorldState, Action _ActionToDo)
	{
		Parent     = _Parent;
		WorldState = _NewWorldState;
		Action     = _ActionToDo;

		if (_Parent is not null && _ActionToDo is not null)
			Cost = Parent.Cost + Action.GetCost();
	}
}

}
