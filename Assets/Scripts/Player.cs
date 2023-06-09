using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    //Define delegate types and events here
    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }
    [SerializeField] private float speed = 4;
    public bool moving = false;
    private Vector3 currentDir;
    public GameManager gameManager;
    private string nameDir;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if(node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                break;
            }
        }
    }

    public void Movement(Vector3 direction)
    {
        Debug.DrawRay(transform.position, direction * 10, Color.cyan, 2); //draws debug raycast
        RaycastHit[] result = Physics.RaycastAll(transform.position, direction * 10, 10);
        foreach(RaycastHit hit in result)//checks through saved hit list
        {
            if (hit.collider.TryGetComponent<Node>(out Node rayhit))
            {
            Node TargetNode = rayhit;
            gameManager.ButtonColor(nameDir, true);
            MoveToNode(TargetNode);
            moving = true;
            }        
        }
        if (moving == false)
        {
            gameManager.ButtonColor(nameDir, false);
            Debug.Log("Nothing here");
        }
    }

    public void OnClick(string name)
    {
        if (moving == false)
        {
            switch (name)
            {
                case "Up":
                    nameDir = "Up";
                    Movement(transform.forward);
                    break;
                case "Down":
                    nameDir = "Down";
                    Movement(-transform.forward);
                    break;
                case "Left":
                    nameDir = "Left";
                    Movement(-transform.right);
                    break;
                case "Right":
                    nameDir = "Right";
                    Movement(transform.right);
                    break;
            }
            Debug.Log("Pressed " + name);
            
        }
    }

    void Update()
    {//Implement inputs and event-callbacks here
        if (moving == false)
        {
            float axisVert = Input.GetAxis("Vertical");
            float axisHori = Input.GetAxis("Horizontal");
            if ((axisVert != 0) | (axisHori != 0))// gets forward
            {
                if (axisVert > 0)
                {
                    OnClick("Up");
                }
                if (axisVert < 0)
                {
                    OnClick("Down");
                }
                if (axisHori < 0)
                {
                    OnClick("Left");
                }
                if (axisHori > 0)
                {
                    OnClick("Right");
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                if (TargetNode == null)
                {
                    TargetNode = null;
                }
                else
                {
                    transform.Translate(currentDir * speed * Time.deltaTime); //movement towards set node
                }
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
            }
        }
    }

    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    public void MoveToNode(Node node)
    {
        if (moving == false) //checks to prevent overlapping movement
        {
            TargetNode = node;
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }
}
