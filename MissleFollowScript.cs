using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleFollowScript : MonoBehaviour {

    public float speed;
    private Vector3 targetDir;
    public float followTime;

    void OnEnable() {

        //Destroy(this.gameObject, 15f);

        if (GameManager.instance.Enemies.Count >= 1) {
            return;
        }

        GameManager.instance.Enemies.Add(this.gameObject);

        gameObject.AddComponent<MainEnemy>();
        gameObject.AddComponent<Rigidbody2D>();
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.gravityScale = 0;

    }


    void Update() {

        if (followTime > 0 && FindObjectOfType<MovementController>() != null) {
            followTime -= Time.deltaTime;
            targetDir = (FindObjectOfType<MovementController>().transform.position - transform.position).normalized;
        }

        transform.position += targetDir * speed * Time.deltaTime;

    }

}
