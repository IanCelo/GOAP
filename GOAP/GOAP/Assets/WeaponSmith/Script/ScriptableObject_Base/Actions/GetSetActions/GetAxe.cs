using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/GetAxe", fileName = "GetAxeScriptable")]
public class GetAxe : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;

		Character.SetTool(1);

		return true;
	}
}

}

