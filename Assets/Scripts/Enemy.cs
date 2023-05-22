using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    [SerializeField] private Node currentNode, rootNode;
    private Vector3 currentDir;
    private bool playerCaught;
    public Player player;
    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };
    public bool toggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle = false;
        playerCaught = false;
        targetFound = false;
        InitializeAgent();
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if (node.Children.Length > 2 && node.Parents.Length == 0)
            {
                currentNode = node;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCaught == false)
        {
            if (currentNode != null) //current node exists
            {
                //move until within distance of currentnode.
                if (Vector3.Distance(transform.position, currentNode.transform.position) > 0.25f)
                {
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                else 
                {
                    if (toggle == false)
                    {
                        Debug.Log("Depth Search Started");
                        searchNode = rootNode;
                        targetFound = false;
                        toggle = true; //DFS on
                        StartCoroutine(DepthFirst());
                    }
                }
            }
            else
            {
                Debug.LogWarning($"{name} - No current node");
            }
            Debug.DrawRay(transform.position, currentDir, Color.cyan, 2f);
        }
    }

    //Called when a collider enters this object's trigger collider. Must have rigidbody to work.
    private void OnTriggerEnter(Collider other)
    {
        if (playerCaught == false)
        {
            if (other.CompareTag("Player"))
            {
                playerCaught = true;
                Debug.Log("Player caught");
                GameOverEvent.Invoke(); //invoke the game over event
            }
        }
    }

    /// <summary>
    /// Sets the current node to the first in the Game Managers node list.
    /// Sets the current movement direction to the direction of the current node.
    /// </summary>
    void InitializeAgent()
    {
        currentNode = GameManager.Instance.Nodes[0];
        currentDir = currentNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }
    void DirectionSet()
    {
        currentNode = searchNode;
        currentDir = currentNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }

    private bool targetFound;
    private Stack<Node> unsearchedNodes = new Stack<Node>();
    [SerializeField] private Node searchNode;

    private IEnumerator DepthFirst()
    {
        while(targetFound == false)
        {
            foreach (Node node in searchNode.Children)
            {
                unsearchedNodes.Push(node);
                Debug.DrawLine(searchNode.transform.position, node.transform.position, Color.yellow, 1f);
                Debug.Log("Checking " + searchNode + " to " + node + " adding.");
                yield return new WaitForSeconds(0.1f);
            }
            if ((searchNode == player.TargetNode) | (searchNode == player.CurrentNode)) //checks player
            {
                Debug.Log("Player found at " + searchNode);
                currentNode = searchNode;
                DirectionSet();
                targetFound = true;
                toggle = false; //DFS off
                unsearchedNodes.Clear();
            }
            else
            {
                Debug.Log("No player at " + searchNode);
                searchNode = unsearchedNodes.Pop();
            }
        }
        Debug.Log("Coroutine finish");
    }
}
