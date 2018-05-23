using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour {

    public Text[] texts;
    public int[] durations;
    int i = 0;
    float timer;
    public int sceneIndex;

	// Use this for initialization
	void Start () {

        // Set texts initial colors to black
		foreach(Text t in texts) {
            t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
        }

        // set timer to the first text duration
        timer = durations[0];
	}
	
	// Update is called once per frame
	void Update () {

        // Update timer
        timer -= Time.deltaTime;

        // Update texts color
        for (int j = 0; j < texts.Length; j++)
        {
            // If text is current, fade to white
            if (i == j && texts[j].color.a < 1.0f)
            {
                Color newColor = texts[j].color;
                newColor.a += Time.deltaTime;
                texts[j].color = newColor;
            }
            // Other texts, fade to black
            else if (texts[j].color.a > 0.0f)
            {
                Color newColor = texts[j].color;
                newColor.a -= Time.deltaTime;
                texts[j].color = newColor;
            }
        }

        // Show next text if timer is finished or a key is pressed
        if (timer < 0 || Input.anyKeyDown)
        {
            i++;

            // Set next text timer or load to next scene
            if(i < texts.Length) {
                timer = durations[i];
            }
            else {
                SceneManager.LoadScene(sceneIndex);
            }
        }
	}
}
