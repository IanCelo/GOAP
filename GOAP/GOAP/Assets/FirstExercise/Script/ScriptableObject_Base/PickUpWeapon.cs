using UnityEngine;


namespace GOAP_PlannerExo
{

[CreateAssetMenu(menuName = "Scriptable/ActionsExo/PickUpWeapon", fileName = "PickUpWeaponScriptable")]
public class PickUpWeapon : Action
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
