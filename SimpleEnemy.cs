using UnityEngine;

public class SimpleEnemy : MonoBehaviour {


    public float speed;
    private Vector3 targetDir;

    void Start() {

        speed = speed * Random.Range(0.7f, 1.2f);

        GetComponent<TrailRenderer>().time = Random.Range(0.125f, 0.3f);

        targetDir = (FindObjectOfType<MovementController>().transform.position - transform.position).normalized;

        Destroy(this.gameObject, 3f);

    }

    void Update() {
        transform.position += targetDir * speed * Time.deltaTime;

    }


}
