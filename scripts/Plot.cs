using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject turretContainer;

    [Header("Attributes")]
    [SerializeField] private int xPos;
    [SerializeField] private int yPos;
    [SerializeField] private bool isBlocked;
    [SerializeField] private bool isSupercharged;

    private GameObject tower;

    private void Start()
    {
        this.name = "PlotPos: " + xPos.ToString() + ", " + yPos.ToString();
    }

    public int BuildTower(string towerName)
    {
        if (tower != null)
        {
            Debug.Log("ALREADY OCCUPIED");
            return -1;
        }
        else if (isBlocked == true)
        {
            return -2;
        }
        {
            int towerId = -1;
            if (towerName == "sickle")
                towerId = 0;
            else if (towerName == "slugger")
                towerId = 1;
            else if (towerName == "tenderizer")
                towerId = 2;
            else if (towerName == "disruptor")
                towerId = 3;
            else
                return -3;

            tower = BuildManager.main.BuildSelectedTower(towerId, transform.position, Quaternion.identity);
            if (isSupercharged == true & tower != null)
            {
                if (towerId != 3)
                {
                    tower.GetComponent<TurretScript>().rotationSpeed += 0.25f;
                    tower.GetComponent<TurretScript>().rotationSpeed += 0.5f;
                    tower.GetComponent<TurretScript>().rotationSpeed += 0.25f;
                }
                else
                {
                    tower.GetComponent<DisruptorScript>().targetingRange += 0.25f;
                    tower.GetComponent<DisruptorScript>().aps -= 0.08f;
                    tower.GetComponent<DisruptorScript>().freezeTime += 0.2f;
                }
                
            }
            return 1;
        }
        
    }

    public int DeleteTower()
    {
        if (tower == null)
        {
            return -1;
            Debug.Log("already null");
        }
        else
        {
            Destroy(tower);
            return 1;
        }
    }
}
