using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Collider[] colliderObj;
    [SerializeField] private GameManager gameManager;
    private bool buttState;
    public static string nameB;
    // Start is called before the first frame update
    void Start()
    {
        buttState = true;
        if (colliderObj == null)
        {
            colliderObj = null;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
    /*if(colliderObj.Contains() collision.collider)
        {

        }*/
    }

    public void ButtonColour(Collider collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            buttState = true;
            ButtonSwap(nameB, buttState);
        }
        if (collision == null | !collision.gameObject.CompareTag("Node"))
        {
            buttState = false;
            ButtonSwap(nameB, buttState);
        }
    }

    public void ButtonSwap(string nameB, bool stateB)
    {
        if (stateB == false)
        { 
        switch (nameB)
            {
            case "Up":
                Debug.Log("Up test1");
                break;
            case "Down":
                Debug.Log("Down test1");
                break;
            case "Left":
                Debug.Log("Left test1");
                break;
            case "Right":
                Debug.Log("Right test1");
                break;
            }
        }
        if (stateB == true) 
        {
            switch (nameB)
            {
                case "Up":
                    Debug.Log("Up test2");
                    break;
                case "Down":
                    Debug.Log("Down test2");
                    break;
                case "Left":
                    Debug.Log("Left test2");
                    break;
                case "Right":
                    Debug.Log("Right test2");
                    break;
            }
        }
    }
}
