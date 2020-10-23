using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public static GameManager instance;
    public UIManager uIManager;

    public int numActive;
    public int maxNumActive = 19;

    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> orbs = new List<GameObject>();

    public GameObject ground;
    [HideInInspector] public float arenaX;
    [HideInInspector] public float arenaZ;

    public int highestScore = 100;
    public GameObject winningPlayer;

    public GameObject mainCam;

    //public GameObject startButton;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        arenaX = ground.transform.lossyScale.x * 5;
        arenaZ = ground.transform.lossyScale.x * 5;
    }

    public void CheckCrown(OrbStorage player, int score)
    {
        if(score > highestScore)
        {
            if(winningPlayer != null)
                winningPlayer.GetComponent<OrbStorage>().crown.SetActive(false);

            player.crown.SetActive(true);
            winningPlayer = player.gameObject;
            highestScore = score;
        }
    }

    public void AddObject(GameObject obj)
    {
        if(obj.tag == "Player")
        {
            players.Add(obj);
        }
        else if (obj.tag == "Enemy")
        {
            enemies.Add(obj);
        }
        else if (obj.tag == "Collect")
        {
            orbs.Add(obj);
        }

        numActive = players.Count + enemies.Count;
    }

    public void RemoveObject(GameObject obj)
    {
        if (obj.GetComponent<PlayerControl>())
        {
            players.Remove(obj);
        }
        else if (obj.GetComponent<EnemyControl>())
        {
            enemies.Remove(obj);
        }
        else if (obj.tag == "Collect")
        {
            orbs.Remove(obj);
        }

        numActive = players.Count + enemies.Count;
    }
}
