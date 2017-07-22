using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour {


    private Vector3 dragStartPosition;
    private Vector3 dragCurrPosition;
    private Vector3 distance;

    public bool mouseOrKeyboard = false;

    public void ResetTransform() {

        dragStartPosition = Vector3.zero;
        dragCurrPosition = Vector3.zero;
        distance = Vector3.zero;

    }

    void Update() {

        if (!mouseOrKeyboard)
            InputHandler();
        else
            KeyBoardInput();

        //InputHandler();


        //Collision Bug

        //KeyBoardInput();

    }

    void KeyBoardInput() {

        if (Input.GetAxisRaw("Horizontal") > 0) {
            GetComponent<Rigidbody2D>().velocity += Vector2.right * 2;
        }
        if (Input.GetAxisRaw("Horizontal") < 0) {
            GetComponent<Rigidbody2D>().velocity += Vector2.left * 2;
        }
        if (Input.GetAxisRaw("Vertical") > 0) {
            GetComponent<Rigidbody2D>().velocity += Vector2.up * 2;
        }
        if (Input.GetAxisRaw("Vertical") < 0) {
            GetComponent<Rigidbody2D>().velocity += Vector2.down * 2;
        }

        Vector3 newPos = transform.position;

        Vector3 playerBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));


        newPos.x = Mathf.Clamp(newPos.x, -playerBounds.x, playerBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, -playerBounds.y, playerBounds.y);

        //make sure z is 0 else the player is behind the camera.
        newPos.z = 0;
        transform.position = newPos;


    }


    void InputHandler() {

        if (Input.GetMouseButtonDown(0)) {
            //set dragStartPosition vector to where the click happened.
            dragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //get the distance between the player position and click position.
            distance = transform.position - dragStartPosition;
        }

        if (Input.GetMouseButton(0)) {
            //get the dragging position on the screen.
            Vector3 currentDraggingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //set player position to current dragging position + distance (offset).
            dragCurrPosition = currentDraggingPos + distance;
        }

        //setting new position
        Vector3 newPos = transform.position;

        newPos.x = dragCurrPosition.x;
        newPos.y = dragCurrPosition.y;

        //Limit player's movement. Dont allow going off screen
        Vector3 playerBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        newPos.x = Mathf.Clamp(newPos.x, -playerBounds.x, playerBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, -playerBounds.y, playerBounds.y);

        //make sure z is 0 else the player is behind the camera.
        newPos.z = 0;
        transform.position = newPos;

    }

}
