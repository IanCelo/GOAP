using UnityEngine;


namespace GOAP_PlannerExo
{

[CreateAssetMenu(menuName = "Scriptable/ActionsExo/HealSelf", fileName = "HealSelfScriptable")]
public class HealSelf : Action
{
	public override bool Act(ref States _WorldState)
	{
		if (!IsValid(_WorldState))
			return false;

		ApplyWorldChanges(ref _WorldState);

		return true;
	}
}

}
