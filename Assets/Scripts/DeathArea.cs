using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathArea : MonoBehaviour {
    private string player_tag = "Player";
    bool die = false;
    float health;


    void OnTriggerEnter2D(Collider2D other)
    {
        //Se atingir um objeto com tag "Player", definir vida do objeto para 0
        if (other.gameObject.tag.Equals(player_tag))
        {
            GameObject.FindGameObjectWithTag("PlayerLife").GetComponent<PlayerLife>().health = 0;
            if (!die) die = true;
        }
    }
}
