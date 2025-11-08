using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/GetPickaxe", fileName = "GetPickaxeScriptable")]
public class GetPickaxe : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;

		Character.SetTool(2);

		return true;
	}
}

}

