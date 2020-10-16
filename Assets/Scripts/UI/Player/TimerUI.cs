using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myTimerText;

    private int myPrevDisplayTime = -1;

    void Update()
    {
        if (Mathf.FloorToInt(Time.time) != myPrevDisplayTime)
        {
            myPrevDisplayTime = Mathf.FloorToInt(Time.time);

            float duration = Time.time - StageManager.ourInstance.myStageStartTime;

            int minutes = Mathf.FloorToInt(duration / 60.0f);
            int seconds = Mathf.FloorToInt(duration) % 60;

            myTimerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }
}
