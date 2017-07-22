using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEnemy : MonoBehaviour {


    public Vector3 upScale = new Vector3(0.2f, 0.2f, 0.2f);

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "Enemy" && collision.GetComponent<MissleFollowScript>() != null) {

            //Destroy(collision.gameObject);

            //gameObject.transform.localScale += upScale;

        }

    }

}
