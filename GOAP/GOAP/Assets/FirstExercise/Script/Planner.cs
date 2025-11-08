using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GOAP_PlannerExo
{

public class Planner : MonoBehaviour
{
	public  States       DefaultWorldState;
	public  List<Action> Actions;
	public  States       Goal;
	
	private          List<Node> m_Leaves     = new();

	public void Start()
	{
		Node parent = new(null, DefaultWorldState, null);
		
		BuildGraph(parent, ref m_Leaves, Actions, Goal);
		Display();

		Debug.Log("\nSelected:");
		DisplayPlan(GetBestPlan().Last());
	}

	private List<Node> GetBestPlan()
	{
		m_Leaves.Sort((_Lhs, _Rhs) => _Lhs.Cost.CompareTo(_Rhs.Cost));
		Node endNode = m_Leaves.First();
		
		Node currentNode = endNode;
		
		List<Node> chosenPlan = new();
		
		while (currentNode.Parent is not null)
		{
			chosenPlan.Insert(0, currentNode);
			currentNode = currentNode.Parent;
		}

		return chosenPlan;
	}

	private void Display()
	{
		Debug.Log($"Plan Count: {m_Leaves.Count}");

		foreach (Node node in m_Leaves)
		{
			DisplayPlan(node);
		}
	}

	private static void DisplayPlan(Node _PlanLeaf)
	{
		Node currentNode = _PlanLeaf;

		Debug.Log($"\nPlan Step: (Cost: {currentNode.Cost})");
			
		while (currentNode.Parent is not null)
		{
			Debug.Log(currentNode.Action.GetType().Name);
				
			currentNode = currentNode.Parent;
		}
	}
	
	private static void BuildGraph(Node _Parent, ref List<Node> _Leaves, List<Action> _AvailableActions, States _Goal)
	{
		foreach (Action action in _AvailableActions)
		{
			if (!action.IsValid(_Parent.WorldState)) continue;

			States newWorldState = new(_Parent.WorldState);
			action.ApplyWorldChanges(ref newWorldState);

			Node thisNode = new(_Parent, newWorldState, action);

			if (_Goal.Compare(newWorldState))
			{
				_Leaves.Add(thisNode);
			}
			else
			{
				List<Action> actionsSubSet = new(_AvailableActions);
				actionsSubSet.Remove(action);
					
				BuildGraph(thisNode, ref _Leaves, actionsSubSet, _Goal);
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
			Cost = Parent.Cost + Action.GetCost(_NewWorldState);
	}
}

}
