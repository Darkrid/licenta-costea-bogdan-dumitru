using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject responseLine;
    [SerializeField] public InputField input;
    [SerializeField] public ScrollRect scrollRect;
    [SerializeField] public GameObject msgList;
    [SerializeField] public Interpreter interpreter;

    private void Start()
    {
        input.ActivateInputField();
        input.Select();

        interpreter = GetComponent<Interpreter>();
    }

    // GUI Events
    private void OnGUI()
    {
        if (input.isFocused && input.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            string userInput = input.text;
            AddLine("");
            AddLine("> " + userInput);  
            input.text = "";

            int lines = AddInterpreterLines(interpreter.Interpret(userInput));

            // Scroll to bottom
            scrollRect.verticalNormalizedPosition = 0;

            input.ActivateInputField();
            input.Select();
        }
    }

    public void AddLine(string userInput)
    {
        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
        msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 19.0f);

        GameObject msg = Instantiate(responseLine, msgList.transform);
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);

        msg.GetComponentsInChildren<Text>()[0].text = userInput;
        scrollRect.verticalNormalizedPosition = 0;
    }

    public int AddInterpreterLines(List<string> interpretation)
    {
        for (int i = 0; i < interpretation.Count; i++)
        {
            GameObject response = Instantiate(responseLine, msgList.transform);

            response.transform.SetAsLastSibling();

            Vector2 listSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(listSize.x, listSize.y + 19.0f);

            response.GetComponentInChildren<Text>().text = interpretation[i];
        }

        return interpretation.Count;
    }
}
