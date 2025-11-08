using UnityEngine;


namespace GOAP_PlannerExo
{

[CreateAssetMenu(menuName = "Scriptable/ActionsExo/MoveToEnemy", fileName = "MoveToEnemyScriptable")]
public class MoveToEnemy : Action
{
	private GameObject m_Target;

	public override bool IsValid(States _WorldState)
	{
		return m_Target is not null && base.IsValid(_WorldState);
	}

	public override bool Act(ref States _WorldState)
	{
		if (!IsValid(_WorldState))
			return false;

		ApplyWorldChanges(ref _WorldState);

		return true;
	}
}

}
