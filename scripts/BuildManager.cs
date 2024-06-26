using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private int[] towerCostsCpu;
    [SerializeField] private int[] towerCostsRam;
    [SerializeField] private GameObject parentContainer;


    private void Awake()
    {
        main = this;
    }

    public GameObject BuildSelectedTower(int towerId, Vector3 plotPos, Quaternion q)
    {
        if (LevelManager.main.SpendCurrency(towerCostsCpu[towerId], towerCostsRam[towerId]) == true)
        {
            return Instantiate(towerPrefabs[towerId], plotPos, Quaternion.identity, parentContainer.transform);
        }
        else
        {
            Debug.Log("NOT ENOUGH");
            return null;
        }
    }
}
