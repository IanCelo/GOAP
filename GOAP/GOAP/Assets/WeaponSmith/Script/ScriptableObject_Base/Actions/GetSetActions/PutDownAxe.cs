using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/PutDownAxe", fileName = "PutDownAxeScriptable")]
public class PutDownAxe : Action
{
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;

		Character.SetTool(0);

		return true;
	}
}

}

