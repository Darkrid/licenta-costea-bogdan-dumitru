using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Text topRightText;
    public CommandManager cm;
    public Interpreter it;
    public EnemySpawner es;
    public bool active = false;
    public GameObject gameContainer;
    public AudioClip winSound;
    public AudioClip loseSound;

    public Transform[] path;

    public Text currencyText;

    public int currencyCpu;
    public int currencyRam;

    public int baseHealth;

    public GameObject enemyContainer;
    public GameObject turretsContainer;
    public GameObject projectilesContainer;

    public bool unload = false;

    private void Awake()
    {
        main = this;
        currencyText.text = "CPU: " + currencyCpu + "  RAM: " + currencyRam;
        topRightText.text = "Health: " + baseHealth;
    }

    private void FixedUpdate()
    {
        topRightText.text = "Health: " + baseHealth;
    }

    private void Start()
    {
        main = this;
        currencyText.text = "CPU: " + currencyCpu + "  RAM: " + currencyRam;
        topRightText.text = "Health: " + baseHealth;
    }

    public void Update()
    {
        if (active == true && unload == true)
        {
            currencyCpu = 0;
            currencyRam = 0;
            es.GetComponent<EnemySpawner>().ResetSettings();
            active = false;
            unload = false;
        }
        if (baseHealth == 0 && active == true)
        {
            AudioSource audio = GetComponent<AudioSource>();
            topRightText.text = "Health: " + baseHealth;
            cm.AddLine("<color=#ff5879>WARNING! Sustained damage found in extractor module.</color>");
            cm.AddLine("<color=#ff5879>Forced shutdown initiated.</color>");
            audio.clip = loseSound;
            audio.Play();
            for (var i = enemyContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(enemyContainer.transform.GetChild(i).gameObject);
            }

            for (var i = turretsContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(turretsContainer.transform.GetChild(i).gameObject);
            }

            GameObject[] gos = GameObject.FindGameObjectsWithTag("LevelTag");
            gameContainer.GetComponent<LevelManager>().active = false;
            foreach (GameObject go in gos)
                Destroy(go);
            it.isLevelLoaded = false;
            active = false;
            unload = false;
        }
        if (es.GetComponent<EnemySpawner>().currentWave == es.GetComponent<EnemySpawner>().nrOfWaves + 1 && active == true)
        {
            AudioSource audio = GetComponent<AudioSource>();
            topRightText.text = "Health: " + baseHealth;
            cm.AddLine("<color=#f2f1b9>SUCCESS! The infected virus files have been purged.</color>");
            cm.AddLine("<color=#f2f1b9>Starting cleanup sequence. You will be automatically disconnected.</color>");
            cm.AddLine("<color=#f2f1b9>Thank you for your contribution!</color>");
            audio.clip = loseSound;
            audio.Play();
            for (var i = enemyContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(enemyContainer.transform.GetChild(i).gameObject);
            }

            for (var i = turretsContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(turretsContainer.transform.GetChild(i).gameObject);
            }

            GameObject[] gos = GameObject.FindGameObjectsWithTag("LevelTag");
            foreach (GameObject go in gos)
                Destroy(go);
            it.isLevelLoaded = false;
            active = false;
            unload = false;
        }
    }

    public void IncreaseCurrency(int amountCpu, int amountRam)
    {
        currencyCpu += amountCpu;
        currencyRam += amountRam;
        currencyText.text = "CPU: " + currencyCpu + "  RAM: " + currencyRam;
    }

    public void SetCurrency(int amountCpu, int amountRam)
    {
        currencyCpu = amountCpu;
        currencyRam = amountRam;
        currencyText.text = "CPU: " + currencyCpu + "  RAM: " + currencyRam;
    }

    public bool SpendCurrency(int amountCpu, int amountRam)
    {
        if (amountCpu <= currencyCpu && amountRam <= currencyRam)
        {
            // BUY
            currencyCpu -= amountCpu;
            currencyRam -= amountRam;
            currencyText.text = "CPU: " + currencyCpu + "  RAM: " + currencyRam;
            return true;
        }
        else
        {
            Debug.Log("NOT ENOUGH CURRENCY");
            return false;
        }
    }
}
