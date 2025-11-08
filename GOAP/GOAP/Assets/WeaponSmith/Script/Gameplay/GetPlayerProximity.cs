using UnityEngine;


public class GetPlayerProximity : MonoBehaviour
{
	[SerializeField] private Character Player;
	[SerializeField] private Transform Target;
	[SerializeField] private StateKey  ProximityStateAffected;

	private float m_UpdateTimer;
	
	private void Update()
	{
		m_UpdateTimer += Time.deltaTime;

		if (m_UpdateTimer < 0.2f)
			return;

		m_UpdateTimer = 0f;

		Player.ChangeState(ProximityStateAffected, Vector3.Distance(Player.transform.position, Target.position) <= 0.01f);
	}
}
