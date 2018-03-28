using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	public Camera mainCam;

	float shakeAmount = 0;
    bool repeating = false;

	void Awake()
	{
		if (mainCam == null)
			mainCam = Camera.main;
	}

	public void Shake(float amt, float length)
	{
		shakeAmount = amt;
        if (!repeating) {
            repeating = true;
            InvokeRepeating("DoShake", 0, 0.01f);
            Invoke("StopShake", length);
        }
	}

	void DoShake()
	{
		if (shakeAmount > 0)
		{
			Vector3 camPos = mainCam.transform.position;

			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
			camPos.x += offsetX;
			camPos.y += offsetY;

			mainCam.transform.position = camPos;
		}
	}

	void StopShake()
	{
        repeating = false;
        CancelInvoke("DoShake");
		//mainCam.transform.localPosition = Vector3.zero;
	}

}
