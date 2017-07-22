using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {


    public float explosionArea;


    private void OnEnable() {

        StartCoroutine(SelfDestruct());

    }

    IEnumerator SelfDestruct() {

        yield return new WaitForSeconds(2);


        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionArea);

        foreach (var col in colliders) {

            if (col.tag == "Enemy") {
                Debug.Log("D");
            }

        }



        Destroy(this.gameObject);
    }


    private void OnDrawGizmosSelected() {

        //Gizmos.DrawSphere(transform.position, explosionArea);

    }

}
