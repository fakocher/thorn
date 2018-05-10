using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    public Image[] hearts;
    public Sprite heartFull;
    public Sprite heartEmpty;

    public void updateHealth(int currentHp, int hp){
        for(int i = 0; i < hearts.Length; i++) {
            if(i < currentHp) {
                hearts[i].enabled = true;
                hearts[i].sprite = heartFull;
            }
            else if(i < hp) {
                hearts[i].enabled = true;
                hearts[i].sprite = heartEmpty;
            }
            else {
                hearts[i].enabled = false;
            }
        }
    }
}
