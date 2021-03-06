﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float displayWaveUIDuration = 5f;
    public GameObject zombie;
    public GameObject dog;
    public Text waveUI;
    public int nextLevel;
    public bool bossDead = false;
    [HideInInspector]
    public static GameManager instance;

    private List<GameObject> zombies = new List<GameObject>();
    private float zombieSpawnTimer = 0;
    private float waveUiTimer;
    private int numberOfZombies = 0;
    private int wave = 0;
    public int numberOfWaves = 5;


    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfWaves > 0)
        {
            if (numberOfZombies > 0 && zombieSpawnTimer <= 0)
            {
                if (Random.value < 0.2f)
                {
                    zombies.Add(Instantiate(dog));
                }
                else
                {
                    zombies.Add(Instantiate(zombie));
                }
                numberOfZombies--;
                zombieSpawnTimer = Random.Range(0.3f, 1.0f);
            }
            for (int i = 0; i < zombies.Count; i++)
            {
                if (zombies[i].GetComponent<Zombie>() == null || zombies[i].GetComponent<Zombie>().health <= 0)
                {
                    zombies.RemoveAt(i);
                }
            }
            if (zombies.Count <= 0 && numberOfZombies == 0)
            {
                // new wave
                waveUI.enabled = true;
                wave++;
                if (wave > numberOfWaves)
                {
                    SceneManager.LoadScene(nextLevel);
                }
                waveUI.text = "Wave " + wave;
                numberOfZombies = wave * 5;
                waveUiTimer = displayWaveUIDuration;
            }
            if (waveUiTimer <= 0)
            {
                waveUI.enabled = false;
            }
            waveUiTimer -= Time.deltaTime;
            zombieSpawnTimer -= Time.deltaTime;
        }
        else
        {
            // Boss part
            if(bossDead)
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0.1f;
        waveUiTimer = 1000f;
        waveUI.text = "Game Over";
        waveUI.enabled = true;
        Invoke("LoadLevel", .5f);
    }

    void LoadLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
