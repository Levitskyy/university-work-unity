using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] private GameObject liftButton;
    [SerializeField] private float maxHeight;
    [SerializeField] private float liftSpeed;
    private LiftButton liftButtonController;
    private float currentHeight = 0;

    void Start() {
        liftButtonController = liftButton.GetComponent<LiftButton>();
    }
    void FixedUpdate() {
        if (liftButtonController.isPressed) {
            if (currentHeight < maxHeight) {
                currentHeight += liftSpeed;
                transform.Translate(Vector2.up * liftSpeed);
            }
        }
        else {
            if (currentHeight > 0) {
                currentHeight -= liftSpeed;
                transform.Translate(Vector2.down * liftSpeed);
            }
        }
    }

}
