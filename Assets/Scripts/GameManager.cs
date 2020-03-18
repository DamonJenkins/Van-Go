using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	bool timeRunning = false;
	bool timerVisible =  false;
	float timerVal = 0.0f;
	Text timerText;

    // Start is called before the first frame update
    void Start()
    {
		DontDestroyOnLoad(gameObject);

		timerVisible = PlayerPrefs.GetInt("showTimer", 0) == 1;

		timerText = GameObject.Find("TimerText").GetComponent<Text>();

		if (!timerVisible) {
			timerText.gameObject.SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (timeRunning){
			timerVal += Time.deltaTime;
		}

		if (timerVisible) {
			timerText.text = string.Format("{0:00:00.00}s", timerVal).Replace(".", ":");
		}
    }

	public void StartTimer() {
		timerVal = 0.0f;
		timeRunning = true;
	}

	public void StopTimer()
	{
		timeRunning = false;
	}

	public float GetTimer() {
		return timerVal;
	}
}
