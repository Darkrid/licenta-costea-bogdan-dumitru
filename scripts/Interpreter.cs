using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Interpreter : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public bool isLevelLoaded = false;


    [Header("References")]
    [SerializeField] public GameObject msgList;
    [SerializeField] public EnemySpawner enemySpawner;
    [SerializeField] public GameObject gameContainer;

    [SerializeField] public GameObject enemyContainer;
    [SerializeField] public GameObject turretsContainer;
    [SerializeField] public GameObject projectilesContainer;


    [Header("Levels")]
    [SerializeField] public GameObject[] plotLevels;
    [SerializeField] public GameObject[] pathLevels;

    

    Dictionary<string, string> colors = new Dictionary<string, string>()
    {
        {"black",   "#021b21"},
        {"gray",    "#555d71"},
        {"red",     "#ff5879"},
        {"yellow",  "#f2f1b9"},
        {"blue",    "#9ed9d8"},
        {"purple",  "#d926ff"},
        {"orange",  "#ef5847"}
    };

    private GameObject[] deleteObjects;

    private List<string> response = new List<string>();

    public List<string> Interpret(string userInput)
    {
        response.Clear();

        string[] args = userInput.Split();

        if (args[0] == "help")
        {
            string[] args2 = userInput.Split(' ');
            if (args2.Length == 1)
            {
                response.Add("");
                response.Add("Basic help options:");
                response.Add("-------------------");

                response.Add(ColorString("intro", colors["purple"])
                    + "        | "
                    + ColorString("Brings up rules about the program", colors["orange"]));
                response.Add(ColorString("help turrets", colors["purple"])
                    + " | "
                    + ColorString("Brings up info about turrets", colors["orange"]));
                response.Add(ColorString("help levels", colors["purple"])
                    + "  | "
                    + ColorString("Brings up info about levels", colors["orange"]));
                response.Add(ColorString("echo [text]", colors["purple"])
                    + "  | "
                    + ColorString("Rewrites your text", colors["orange"]));
                response.Add(ColorString("ascii", colors["purple"])
                    + "        | "
                    + ColorString("Displays an ASCII title", colors["orange"]));
                response.Add(ColorString("clear", colors["purple"])
                    + "        | "
                    + ColorString("Clears all terminal output", colors["orange"]));
            }
            else if (args2.Length == 2 && args2[1] == "turrets")
            {
                response.Add("");
                response.Add("Turret commands:");
                response.Add("-------------------");

                response.Add(ColorString("* turret build <type> <xPos> <yPos>", colors["purple"]));
                response.Add(ColorString("Builds turret type at location", colors["orange"]));
                response.Add(ColorString("Coordinates start at 0", colors["orange"]));
                response.Add(ColorString("* turret delete <xPos> <yPos>", colors["purple"]));
                response.Add(ColorString("Delete turret at location", colors["orange"]));
                response.Add(ColorString("Coordinates start at 0", colors["orange"]));


                response.Add("");
                response.Add("Turret types:");
                response.Add("-------------------");
                response.Add(ColorString("* sickle", colors["purple"]));
                response.Add(ColorString("Fast fire rate, low range turret", colors["orange"]));
                response.Add(ColorString("* slugger", colors["purple"]));
                response.Add(ColorString("Slow fire rate, medium range, high damage low speed turret", colors["orange"]));
                response.Add(ColorString("* tenderizer", colors["purple"]));
                response.Add(ColorString("Very high range, very slow fire rate, high damage turret", colors["orange"]));
                response.Add(ColorString("* disruptor", colors["purple"]));
                response.Add(ColorString("Utility, close range slowing turret", colors["orange"]));
            }
            else if (args2.Length == 2 && args2[1] == "levels")
            {
                response.Add("");
                response.Add("Level commands:");
                response.Add("-------------------");

                response.Add(ColorString("* level load <number>", colors["purple"]));
                response.Add(ColorString("Loads selected level.", colors["orange"]));
                response.Add(ColorString("* level unload", colors["purple"]));
                response.Add(ColorString("Unloads current level.", colors["orange"]));


                response.Add("");
                response.Add("Level number:");
                response.Add("-------------------");

                response.Add(ColorString("* 01 - RPI Mainboard", colors["purple"]));
                response.Add(ColorString("Small, introductory level, allows creativity", colors["orange"]));
                response.Add(ColorString("* 02 - Rerouter", colors["purple"]));
                response.Add(ColorString("Medium difficulty level, unique wrap-around layout", colors["orange"]));
                response.Add(ColorString("* 03 - Direct connection", colors["purple"]));
                response.Add(ColorString("Hard, up to down level, unstable", colors["orange"]));
            }
        }
        else if (args[0] == "intro")
        {
            response.Add("");
            response.Add("-------------------");
            response.Add(ColorString("Welcome to the Pro-Purgery Initiative. (P.P.I)", colors["orange"]));
            response.Add(ColorString("In this training simulation, you will learn how to setup", colors["orange"]));
            response.Add(ColorString("defenses against an active virus threat.", colors["orange"]));
            response.Add(ColorString("A good order to start training is to load a level,", colors["orange"]));
            response.Add(ColorString("defenses and start a wave.", colors["orange"]));
            response.Add(ColorString("Experiment with multiple defense designs to train yourself in", colors["orange"]));
            response.Add(ColorString("an event of Active Virus Purgery situations.", colors["orange"]));
            response.Add("");
            response.Add(ColorString("WARNING! This simulation does not include the number of waves", colors["red"]));
            response.Add(ColorString("remaining as to simulate real life scenarios.", colors["red"]));
            response.Add(ColorString("A limit to how many waves can be spawned is present to save", colors["purple"]));
            response.Add(ColorString("time,but in real life scenarios it can take hours", colors["purple"]));
            response.Add(ColorString("before a virus is purged from the system.", colors["purple"]));
            response.Add("-------------------");
        }
        else if (args[0] == "echo")
        {
            string[] args2 = userInput.Split(" ", 2);

            response.Add(args2[1]);
        }
        else if (args[0] == "ascii")
        {
            LoadTitle("ascii.txt", "red", 1);
        }
        else if (args[0] == "clear")
        {   
            deleteObjects = GameObject.FindGameObjectsWithTag("TerminalResponseLine");
            for (int i = 0; i < deleteObjects.Length; i++)
            {
                Object.Destroy(deleteObjects[i]);
            }

            Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, 19.0f);
        }
        else if (args[0] == "wave")
        {
            string[] args2 = userInput.Split(" ", 2);

            if (args2.Length == 2 && args2[1] == "start")
            {
                response.Add("");
                response.Add(ColorString("Wave starting", colors["purple"]));
                enemySpawner.StartCommand();
            }
            else
            {
                response.Add("");
                response.Add(ColorString("Unknown wave command. Check spelling", colors["red"]));
            }
        }
        else if (args[0] == "turret")
        {
            if (isLevelLoaded == false)
            {
                response.Add(ColorString("No level loaded.", colors["red"]));
            }
            else
            {
                string[] args2 = userInput.Split(' ');

                if (args2.Length == 5 && args2[1] == "build")
                {
                    GameObject targetPlot = GameObject.Find("PlotPos: " + args2[3] + ", " + args2[4]);

                    if (targetPlot == null)
                    {
                        response.Add(ColorString("Coordinates invalid.", colors["red"]));
                    }
                    else
                    {
                        int result = targetPlot.GetComponent<Plot>().BuildTower(args2[2]);
                        if (result == -1)
                        {
                            response.Add(ColorString("Plot already occupied.", colors["red"]));
                        }
                        else if (result == -2)
                        {
                            response.Add(ColorString("Plot blocked.", colors["red"]));
                        }
                        else if (result == -3)
                        {
                            response.Add(ColorString("Invalid turret name. Check spelling.", colors["red"]));
                        }
                        else
                        {
                            response.Add("");
                            response.Add(ColorString("Built turret at coordinates", colors["blue"]));
                        }
                    }
                }
                else if ((args2.Length == 4 && args2[1] == "delete"))
                {
                    GameObject targetPlot = GameObject.Find("PlotPos: " + args2[2] + ", " + args2[3]);

                    if (targetPlot == null)
                    {
                        response.Add(ColorString("Coordinates invalid.", colors["red"]));
                    }
                    else
                    {
                        int result = targetPlot.GetComponent<Plot>().DeleteTower();
                        if (result == -1)
                        {
                            response.Add(ColorString("Plot already empty.", colors["red"]));
                        }
                        else
                        {
                            response.Add("");
                            response.Add(ColorString("Turret destroyed at coordinates", colors["blue"]));
                        }
                    }
                }
                else
                {
                    response.Add(ColorString("Unknown command. Check spelling.", colors["red"]));
                }
            }
        }
        else if (args[0] == "level")
        {
            string[] args2 = userInput.Split(' ');

            if (args2.Length == 2 && args2[1] == "unload")
            {
                if (isLevelLoaded != true)
                {
                    response.Add(ColorString("No level loaded.", colors["red"]));
                }
                else
                {
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

                    gameContainer.GetComponent<LevelManager>().SetCurrency(0, 0);

                    gameContainer.GetComponent<LevelManager>().active = false;

                    gameContainer.GetComponent<EnemySpawner>().enabled = false;

                    response.Add("");
                    response.Add(ColorString("Current level unloaded.", colors["blue"]));

                    isLevelLoaded = false;
                }
            }
            else if (args2.Length == 3 && args2[1] == "load")
            {
                if (isLevelLoaded == true)
                {
                    response.Add(ColorString("A level is already loaded.", colors["red"]));
                }
                else if (args2[2] == "1" || args2[2] == "2" || args2[2] == "3")
                {
                    response.Add("");
                    response.Add(ColorString("Level loaded.", colors["blue"]));

                    Instantiate(plotLevels[int.Parse(args2[2]) - 1], gameContainer.transform);
                    Instantiate(pathLevels[int.Parse(args2[2]) - 1], gameContainer.transform);

                    gameContainer.GetComponent<EnemySpawner>().enabled = true;
                    if (args2[2] == "1")
                    {
                        gameContainer.GetComponent<LevelManager>().SetCurrency(200, 200);
                        gameContainer.GetComponent<LevelManager>().baseHealth = 5;
                        gameContainer.GetComponent<LevelManager>().active = true;
                        gameContainer.GetComponent<EnemySpawner>().nrOfWaves = 3;
                        gameContainer.GetComponent<EnemySpawner>().currentWave = 0;
                        gameContainer.GetComponent<EnemySpawner>().enemiesPerSecond = 1f;
                        gameContainer.GetComponent<EnemySpawner>().enemiesAlive = 0;
                        gameContainer.GetComponent<EnemySpawner>().enemiesLeftToSpawn = 0;
                    }
                    else if (args2[2] == "2")
                    {
                        gameContainer.GetComponent<LevelManager>().SetCurrency(150, 150);
                        gameContainer.GetComponent<LevelManager>().baseHealth = 4;
                        gameContainer.GetComponent<LevelManager>().active = true;
                        gameContainer.GetComponent<EnemySpawner>().nrOfWaves = 5;
                        gameContainer.GetComponent<EnemySpawner>().currentWave = 0;
                        gameContainer.GetComponent<EnemySpawner>().enemiesPerSecond = 1f;
                        gameContainer.GetComponent<EnemySpawner>().enemiesAlive = 0;
                        gameContainer.GetComponent<EnemySpawner>().enemiesLeftToSpawn = 0;
                    }
                    else
                    {
                        gameContainer.GetComponent<LevelManager>().SetCurrency(100, 100);
                        gameContainer.GetComponent<LevelManager>().baseHealth = 3;
                        gameContainer.GetComponent<LevelManager>().active = true;
                        gameContainer.GetComponent<EnemySpawner>().nrOfWaves = 10;
                        gameContainer.GetComponent<EnemySpawner>().currentWave = 0;
                        gameContainer.GetComponent<EnemySpawner>().enemiesPerSecond = 1.2f;
                        gameContainer.GetComponent<EnemySpawner>().enemiesAlive = 0;
                        gameContainer.GetComponent<EnemySpawner>().enemiesLeftToSpawn = 0;
                    }
                    
                    isLevelLoaded = true;
                }
                else 
                {
                    response.Add(ColorString("Level not found.", colors["red"]));
                }
            }
            else
            {
                response.Add(ColorString("Unknown command. Check spelling.", colors["red"]));
            }
            
        }
        else
        {
            response.Add("");
            response.Add(ColorString("Unknown command. Check spelling", colors["red"]));
        }

        return response;
    }

    private string ColorString(string s, string color)
    {
        string leftTag = "<color=" + color + ">";
        string rightTag = "</color>";

        return leftTag + s + rightTag;
    }

    private void LoadTitle(string path, string color, int spacing)
    {
        StreamReader file = new StreamReader(Path.Combine(Application.streamingAssetsPath, path));

        for (int i = 0; i < spacing; i++)
        {
            response.Add("");
        }

        while(!file.EndOfStream)
        {
            response.Add(ColorString(file.ReadLine(), colors[color]));
        }

        file.Close();
    }
}
