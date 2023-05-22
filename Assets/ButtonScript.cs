using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Collider[] colliderObj;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (colliderObj == null)
        {
            colliderObj = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.moving == false)
        {
            switch (name)
            {
                case "Up":
                    
                case "Down":

                case "Left":

                case "Right":
                    break;
            }
            
            Debug.Log("Pressed " + name);
        }
    }
}
