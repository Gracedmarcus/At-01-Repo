using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour, IPointerClickHandler
{
    public static GameManager Instance { get; private set; }
    //Stores all the nodes
    [SerializeField] private Node[] nodes;    
    //Stores reference to the player
    public Node[] Nodes { get { return nodes; } }
    public Player Player { get { return Player; } }
    [SerializeField] GraphicRaycaster gRaycaster;
    public List<Image> buttonlist;
    [SerializeField] private Color bOn, bOff, bDflt;

    public void OnPointerClick(PointerEventData mousePos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        Clicked(eventData);
    }

    public void Clicked(PointerEventData vector)
    {
        List<RaycastResult> rayList = new List<RaycastResult>();
        gRaycaster.Raycast(vector, rayList);
        Debug.Log("Raycast");
        if (rayList != null)
        {
            foreach (RaycastResult rayRet in rayList)
            {
                if (rayRet.gameObject.TryGetComponent<Image>(out Image mouserayhit)) //store raycasy target
                {
                    if (buttonlist.Contains(mouserayhit)) //compare list contents here
                    {
                        Player.OnClick(mouserayhit.name); //get list item position
                        break;
                    }
                }
            }
            Debug.Log(rayList.Count);
        }
    }

    /// <summary>
    /// Awake is called before Start is executed for the first time.
    /// </summary>
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        FindObjectOfType<Enemy>().GameOverEvent += GameOver;
    }

    /// <summary>
    /// Triggers the Restart Game coroutine.
    /// </summary>
    private void GameOver()
    {
        StartCoroutine(RestartGame());
    }

    /// <summary>
    /// Disables the player. Re-loads the active scene after 5 second delay.
    /// </summary>
    /// <returns></returns>
    IEnumerator RestartGame()
    {
        Player.enabled = false;
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
