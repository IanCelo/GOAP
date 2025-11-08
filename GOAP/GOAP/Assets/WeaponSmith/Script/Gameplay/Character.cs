using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GOAP_Planner;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Action = GOAP_Planner.Action;
using Random = UnityEngine.Random;


public class Character : MonoBehaviour
{
	[SerializeField] private NavMeshAgent Agent;
	
	[SerializeField] private States  WorldStates;
	
	[SerializeField] private List<Goal> Goals;
	private                  Goal       m_CurrentGoal;

	[SerializeField] private List<Action> Actions;

	[SerializeField] private Inventory             Inventory;
	private                  CharacterStateManager m_StateManager;	
	
	[SerializeField] private TMP_Text GoalPriorityDisplay;
	[SerializeField] private TMP_Text PlayDisplay;
	[SerializeField] private TMP_Text StatesDisplay;

	[SerializeField] private GameObject Axe;
	[SerializeField] private GameObject Pickaxe;
	private                  int        m_Tool;

	public int CarryCapacity => Inventory.MaxAmount;
	
	private States m_FullWorldStates;
	
	private List<Node> m_CurrentPlan;
	private float      m_UpdateTimer;
	private bool       m_ShouldExecutePlan;

	private void Start()
	{
		m_StateManager = GetComponent<CharacterStateManager>();
		
		foreach (Action action in Actions)
		{
			action.Agent     = Agent;
			action.Character = this;
			action.SetStorage();
		}
		
		m_FullWorldStates = States.MergeStates(WorldStates, WorldManager.Instance.WorldStates);
	}

	// Update is called once per frame
	private void Update()
	{
		if (m_ShouldExecutePlan)
			ExecutePlan();

		if (m_UpdateTimer < 1.1f)
		{
			m_UpdateTimer += Time.deltaTime;
			return;
		}
		
		m_UpdateTimer = 0f;
		
		m_StateManager.UpdateWorldStates(this, Inventory.Type, m_Tool);
		UpdateDisplay();
		
		if (GetGoal())
			ChangePlan(Planner.GetPlan(new States(m_FullWorldStates), Actions, m_CurrentGoal));
	}

	private void UpdateDisplay()
	{
		DisplayGoal();

		DisplayPlan();
		
		DisplayWorldState();
	}

	private void DisplayWorldState()
	{
		string fullPlanDisplay = "WorldState:\n";

		int i = 0;
		foreach (State state in m_FullWorldStates.StateList.Where(_State => _State.Value))
		{
			fullPlanDisplay += $"{{{state.KeyScriptable.name}: {state.Value}}} ";

			if (i++ % 3 == 2)
				fullPlanDisplay += "\n";
		}

		StatesDisplay.text = fullPlanDisplay;
	}

	private void DisplayPlan()
	{
		if (m_CurrentPlan == null)
		{
			PlayDisplay.text = "No Plan";

			return;
		}

		string fullPlanDisplay = $"Plan: (Cost: {m_CurrentPlan[^1].Cost})\n";

		foreach (Node step in m_CurrentPlan)
		{
			fullPlanDisplay += $"{step.Action.GetType().Name}\n";
		}

		PlayDisplay.text = fullPlanDisplay;
	}

	private void DisplayGoal()
	{
		string fullGoalDisplay = $"Goals: {m_CurrentGoal?.name}\n";

		foreach (Goal goal in Goals)
		{
			fullGoalDisplay += $"{goal.name}: {goal.Priority}\n";
		}

		GoalPriorityDisplay.text = fullGoalDisplay;
	}

	private void ChangePlan(List<Node> _NewPlane)
	{
		m_CurrentPlan = _NewPlane;
		StopAllCoroutines();
		m_ShouldExecutePlan = true;
	}

	private void RestartPlan()
	{
		List<Action> actions = m_CurrentPlan.Select(_Node => _Node.Action).ToList();

		m_StateManager.UpdateWorldStates(this, Inventory.Type, m_Tool);
		
		// Try to restart the same goal giving the same action previously used and removing if some are useless (eg. GetAxe for ChopWood can't be looped on)
		List<Node> newPlane = Planner.GetPlan(new States(m_FullWorldStates), actions, m_CurrentGoal);
		
		// If no good plan found retry a plan with complete action list
		if (newPlane is null || newPlane.Count == 0)
			newPlane = Planner.GetPlan(new States(m_FullWorldStates), Actions, m_CurrentGoal);
		
		ChangePlan(newPlane);
	}

	private void ExecutePlan()
	{
		if (m_CurrentPlan is null)
			return;

		m_ShouldExecutePlan = false;
		
		StartCoroutine(nameof(PlanExecution));
	}

	IEnumerator PlanExecution()
	{
		int i = 0;

		while (i < m_CurrentPlan.Count)
		{
			m_CurrentPlan[i].Action.Act();

			yield return null;
			
			while (!m_CurrentPlan[i].Action.FinishActing(ref m_FullWorldStates))
				yield return null;

			i++;
		}

		RestartPlan();
	}

	internal int SetInventory(ResourceType _Type, int _Amount, out bool _SetState)
	{
		int amount = SetAmounts(_Type, _Amount);

		_SetState = true;
		
		if (Inventory.Amount == 0)
		{
			Inventory.Type = ResourceType.NONE;
			_SetState = false;
		}

		return amount;
	}

	internal void SetTool(int _Tool)
	{
		m_Tool = _Tool;
		switch (_Tool)
		{
			case 0:
				Axe.SetActive(false);
				Pickaxe.SetActive(false);
				break;

			case 1:
				Axe.SetActive(true);
				Pickaxe.SetActive(false);
				break;
			
			case 2:
				Axe.SetActive(false);
				Pickaxe.SetActive(true);
				break;
		}
	}

	private int SetAmounts(ResourceType _Type, int _Amount)
	{
		switch (Inventory.Amount)
		{
			case > 0 when _Type != Inventory.Type:
				return -1;

			case 0 when _Amount < 0:
				return _Amount;

			case 0:
				Inventory.Type = _Type;

				return Inventory.PutIn(_Amount);

			default:
				return _Amount < 0 ? Inventory.Take(-_Amount) : Inventory.PutIn(_Amount);
		}
	}

	private bool GetGoal()
	{
		Goals.Sort((_Lhs, _Rhs) => _Rhs.Priority.CompareTo(_Lhs.Priority));

		if (m_CurrentGoal && Goals[0].Priority <= m_CurrentGoal.Priority)
			return false;
		
		List<Goal> GoalCandidate = Goals.Where(_Goal => _Goal.Priority == Goals[0].Priority).ToList();

		int randomGoal = Random.Range(0, GoalCandidate.Count);
		
		m_CurrentGoal = GoalCandidate[randomGoal];

		return true;
	}

	private void OnValidate()
	{
		WorldStates.Update();
	}

	public void ChangeState(StateKey _ProximityStateAffected, bool _NewValue)
	{
		m_FullWorldStates.SetState(_ProximityStateAffected.Hash, _NewValue);
	}
}
