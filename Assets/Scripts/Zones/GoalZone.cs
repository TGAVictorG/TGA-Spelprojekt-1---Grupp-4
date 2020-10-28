using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GoalZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider anOther)
    {
        if (anOther.CompareTag("Player") && StageManager.ourInstance.myIsGoalEnabled)
        {
            StageManager.ourInstance.OnStageComplete();

            enabled = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.5f;

        Gizmos.color = color;

        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }
#endif
}
