using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "Enemy") {
            if (GameManager.instance.shieldUp) {
                if (GameObject.FindGameObjectWithTag("Shield") != null) {
                    return;
                }
                //deactivate shield
                GameManager.instance.shieldUp = false;
                return;
            }

            GameManager.instance.GameOver();
        } else if (collision.tag == "Shield") {

            GameManager.instance.shieldUp = true;
            GameManager.instance.shieldGO.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Shield"));
            AudioManager.instance.PlaySound("Shield");

        }
    }



}
