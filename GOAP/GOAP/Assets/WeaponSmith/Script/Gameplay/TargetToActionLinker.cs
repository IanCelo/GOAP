using System.Collections.Generic;
using UnityEngine;
using Action = GOAP_Planner.Action;

public class TargetToActionLinker : MonoBehaviour
{
    [SerializeField] private List<Action>    Actions;
    [SerializeField] private List<Transform> ActionTarget;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < Mathf.Min(Actions.Count, ActionTarget.Count); i++)
        {
            Actions[i].Target = ActionTarget[i];
        }
    }
}
