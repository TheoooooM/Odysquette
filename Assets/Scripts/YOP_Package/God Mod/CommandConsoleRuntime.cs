using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandConsoleRuntime : MonoBehaviour {
    #region VARIABLES
    
    #region INSTANCE
    private static CommandConsoleRuntime instance = null;
    public static CommandConsoleRuntime Instance => instance;
    
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);
    }
    #endregion INSTANCE

    [SerializeField] private Transform textTransformArea = null;
    [SerializeField] private GameObject textPrefab = null;
    [Space]
    [SerializeField] private Transform parameterTransform = null;
    [SerializeField] private GameObject parameterPrefab = null;
    [HideInInspector] public List<object> commandList = new List<object>();

    private bool isShowingHelp = false;
    private List<GameObject> methodList = new List<GameObject>();

    private TMP_InputField textInputField = null;
    private string textToTab = "";
    #endregion VARIABLES
    
    private void Start() {
        CommandConsole HELP = new CommandConsole("help", "help", () => { ShowHelp(textInputField, true); });
        commandList.Add(HELP);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) && textToTab != "") MakeTabulation();
    }

    
    /// <summary>
    /// Add a text to check
    /// </summary>
    /// <param name="inputField"></param>
    public void AddTextToCheck(TMP_InputField inputField) {
        if (textInputField == null) textInputField = inputField;
        CheckEndText(inputField);
    }

    /// <summary>
    /// Check the text in an input field
    /// </summary>
    /// <param name="inputField"></param>
    private void CheckEndText(TMP_InputField inputField) {
        if (textInputField == null) textInputField = inputField;
        string[] properties = inputField.text.Split(' ');

        foreach (object obj in commandList) {
            if (obj is CommandConsoleBase command && properties[0].ToUpper().Contains(command.CommandName.ToUpper())) {
                switch (properties.Length) {
                    case 1 :
                        (command as CommandConsole)?.InvokeCommand();
                        break;
                    case 2:
                        (command as CommandConsole<int>)?.InvokeCommand(int.Parse(properties[1]));
                        break;
                    case 3:
                        (command as CommandConsole<int, string>)?.InvokeCommand(int.Parse(properties[1]), properties[2]);
                        break;
                }
            }
        }

        if (properties[0].ToUpper() == "HELP") ShowHelp(inputField, true);

        inputField.text = "";
        //ResetPrefab();
    }

    /// <summary>
    /// Check for help debug when there is new letter
    /// </summary>
    /// <param name="inputField"></param>
    private void CheckForHelp(TMP_InputField inputField) {
        if (textInputField == null) textInputField = inputField;

        string textField = inputField.text;
        string[] properties = textField.Split(' ');

        if (textField.Length == 0 || textField.ToUpper() == "HELP") {
            if (textField.ToUpper() != "HELP" && !isShowingHelp) {
                ResetPrefab();
                ShowHelpText(textInputField);
            }
            return;
        }

        isShowingHelp = false;
        ResetPrefab();
        
        if (properties.Length == 1) {
            string text = properties[0].ToUpper();
            int textLength = text.Length;

            foreach (object obj in commandList) {
                if (obj is CommandConsoleBase command) {
                    string commandNameCut = textLength < command.CommandName.Length ? command.CommandName.ToUpper().Remove(textLength) : command.CommandName.ToUpper();

                    if (commandNameCut == text && commandNameCut == command.CommandName.ToUpper() && command.FormatCommand.Split(new[] {" "}, StringSplitOptions.None).Length > 1) {
                        CreateMethodPrefab(command.CommandName + " " + command.FormatCommand.Split(new[] {" "}, StringSplitOptions.None)[1]);
                    }
                    else if (commandNameCut == text) {
                        CreateMethodPrefab(command.CommandName);
                        textToTab = command.CommandName + (command.FormatCommand.Split(new[] {" "}, StringSplitOptions.None).Length > 1 ? " " : "");
                    }
                }
            }
        }
        else {
            ResetPrefab();
            string commandName = properties[0].ToUpper();

            foreach (object obj in commandList) {
                if (obj is CommandConsoleBase command && CheckString(commandName, command.CommandName.ToUpper()) && AUnderB(properties.Length, command.FormatCommand.Split(' ').Length + 1)) {
                    string[] formatProperties = command.FormatCommand.Split(new string[] {" "}, StringSplitOptions.None);
                    string textToShow = formatProperties[properties.Length - 1];
                    CreateParameterPrefab(textToShow, inputField.text.Length);
                }
            }
        }
    }

    #region CREATE PREFAB

    /// <summary>
    /// Create a prefab for the possible methods
    /// </summary>
    /// <param name="textToShow"></param>
    /// <returns></returns>
    private void CreateMethodPrefab(string textToShow) {
        GameObject gam = Instantiate(textPrefab, textTransformArea);
        CmdPrefabData data = gam.GetComponent<CmdPrefabData>();
        data.MethodText.text = textToShow;

        string TextToWrite = textToShow.Split(' ')[0] + (textToShow.Split(' ').Length > 1 ? " " : "");
        data.textToWrite = TextToWrite;

        methodList.Add(gam);
    }

    /// <summary>
    /// Create a parameter Prefab for the possible parameters
    /// </summary>
    /// <param name="textToShow"></param>
    /// <returns></returns>
    private void CreateParameterPrefab(string textToShow, int textSize) {
        GameObject gam = Instantiate(parameterPrefab, parameterTransform);
        RectTransform trans = gam.GetComponent<RectTransform>();
        CmdPrefabData data = gam.GetComponent<CmdPrefabData>();
        data.MethodText.text = textToShow;
        data.ButtonGam.interactable = false;

        string TextToWrite = textToShow.Split(' ')[0] + (textToShow.Split(' ').Length > 1 ? " " : "");
        data.textToWrite = TextToWrite;

        trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, textSize * 12, TextToWrite.Length * 11);
        trans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, -38, 35);
        trans.anchorMin = new Vector2(0, 0);
        trans.anchorMax = new Vector2(0, 0);

        methodList.Add(gam);
    }

    #endregion CREATE PREFAB

    /// <summary>
    /// Reset all the prefab that are shown actually
    /// </summary>
    public void ResetPrefab() {
        foreach (GameObject child in methodList) {
            Destroy(child);
        }

        methodList.Clear();
    }

    #region TABULATION
    /// <summary>
    /// Fill the tabulation text
    /// </summary>
    public void MakeTabulation(string text = "") {
        if (textInputField != null) {
            textInputField.text = text == "" ? textToTab : text;
            textInputField.caretPosition = textInputField.text.Length;
            textToTab = "";
            SelectInputField();
            ResetPrefab();
            CheckForHelp(textInputField);
        }
    }
    #endregion TABULATION
    
    #region HELP COMMAND

    /// <summary>
    /// Show help
    /// </summary>
    private void ShowHelp(TMP_InputField inputField, bool showAll = false) {
        if (textInputField == null) textInputField = inputField;
        ResetPrefab();

        isShowingHelp = true;
        foreach (object obj in commandList) {
            if (obj is CommandConsoleBase command) {
                if (showAll) { if(command.CommandName.ToUpper() != "HELP") CreateMethodPrefab(command.FormatCommand); }
                else { if(command.CommandName.ToUpper() != "HELP") CreateMethodPrefab(command.CommandName); }
            }
        }

        SelectInputField();
    }

    /// <summary>
    /// Show the help text
    /// </summary>
    public void ShowHelpText(TMP_InputField inputField) {
        if (textInputField == null) textInputField = inputField;
        CreateMethodPrefab("help");
        textToTab = "help";
    }

    #endregion HELP COMMAND

    #region CHECK METHODS

    /// <summary>
    /// Check if value A is under value B
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    private bool AUnderB(int A, int B) => A < B;

    /// <summary>
    /// Check if the string A is equal to the string B
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    private bool CheckString(string A, string B) => A == B;

    #endregion CHECK METHODS

    /// <summary>
    /// Select the InputField
    /// </summary>
    private void SelectInputField() {
        textInputField.Select();
        textInputField.ActivateInputField();
    }
}

#region HANDLE COMMANDS

/// <summary>
/// Base of the console command class
/// </summary>
public class CommandConsoleBase {
    private string commandName;
    private string formatCommand;

    public string CommandName => commandName;
    public string FormatCommand => formatCommand;

    protected CommandConsoleBase(string commandName, string formatCommand) {
        this.commandName = commandName;
        this.formatCommand = formatCommand;
    }
}



/// <summary>
/// Function to handle the commands
/// </summary>
/// <typeparam name="T1"></typeparam>
public class CommandConsole : CommandConsoleBase {
    private Action actionEvent = null;

    public CommandConsole(string commandName, string formatCommand, Action actionEvent) : base(commandName, formatCommand) {
        this.actionEvent = actionEvent;
    }

    public void InvokeCommand() => actionEvent.Invoke();
}

/// <summary>
/// Function to handle the commands based on a variable
/// </summary>
/// <typeparam name="T1"></typeparam>
public class CommandConsole<T1> : CommandConsoleBase {
    private Action<T1> actionEvent = null;

    public CommandConsole(string commandName, string formatCommand, Action<T1> actionEvent) : base(commandName, formatCommand) {
        this.actionEvent = actionEvent;
    }

    public void InvokeCommand(T1 value) => actionEvent.Invoke(value);
}

/// <summary>
/// Function to handle the commands based on a variable
/// </summary>
/// <typeparam name="T1"></typeparam>
public class CommandConsole<T1, T2> : CommandConsoleBase {
    private Action<T1, T2> actionEvent = null;

    public CommandConsole(string commandName, string formatCommand, Action<T1, T2> actionEvent) : base(commandName, formatCommand) {
        this.actionEvent = actionEvent;
    }

    public void InvokeCommand(T1 value, T2 value2) => actionEvent.Invoke(value, value2);
}

#endregion HANDLE COMMANDS