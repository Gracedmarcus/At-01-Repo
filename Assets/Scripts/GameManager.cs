using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //Stores all the nodes
    [SerializeField] private Node[] nodes;
    public EventSystem Current;
    //Stores reference to the player
    public Node[] Nodes { get { return nodes; } }
    public Player Player { get { return player; } }
    [SerializeField] private Player player;
    [SerializeField] GraphicRaycaster gRaycaster;
    PointerEventData eventData;
    public List<Image> buttonlist;
    public Color colour1, colour2, colour3;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.moving == false)
        {
            Debug.Log("Click detected");
            eventData = new PointerEventData(Current);
            eventData.position = Input.mousePosition;
            Clicked(eventData);
        }
    }

    public void Clicked(PointerEventData vector)
    {
        List<RaycastResult> rayList = new List<RaycastResult>();
        gRaycaster.Raycast(vector, rayList);
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
            rayList.Clear();
        }
    }

    public void ButtonColor(string name, bool state)
    {
        if (state == false)
        {
            switch (name)
            {
                case "Up":
                    buttonlist[0].color = colour2;
                    break;
                case "Down":
                    buttonlist[1].color = colour2;
                    break;
                case "Left":
                    buttonlist[2].color = colour2;
                    break;
                case "Right":
                    buttonlist[3].color = colour2;
                    break;
            }
        }
        if (state == true)
        {
            switch (name)
            {
                case "Up":
                    buttonlist[0].color = colour3;
                    break;
                case "Down":
                    buttonlist[1].color = colour3;
                    break;
                case "Left":
                    buttonlist[2].color = colour3;
                    break;
                case "Right":
                    buttonlist[3].color = colour3;
                    break;
            }
        }
        StartCoroutine(ColorReset());
    }

    IEnumerator ColorReset()
    {
        yield return new WaitForSeconds(1);
        foreach (Image image in buttonlist)
        {
            image.color = colour1;
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
        player.enabled = false;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameScene");
    }
}
