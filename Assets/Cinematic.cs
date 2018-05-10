using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour {

    public Text[] texts;
    public int[] durations;
    int i = 0;
    float timer;

	// Use this for initialization
	void Start () {
		foreach(Text t in texts) {
            t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
        }
        timer = durations[0];
        StartCoroutine(FadeTextToFullAlpha(1, texts[0]));
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer < 0) {
            i++;
            if(i < texts.Length) {
                StartCoroutine(FadeTextToZeroAlpha(1f, texts[i - 1]));
                StartCoroutine(FadeTextToFullAlpha(3, texts[i]));
                timer = durations[i];
            }
            else {
                SceneManager.LoadScene(2);
            }
        }
	}

    public IEnumerator FadeTextToFullAlpha(float t, Text i) {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f) {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i) {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f) {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
