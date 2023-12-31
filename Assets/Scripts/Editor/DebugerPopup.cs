using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Drawing.Printing;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using PlasticGui;

/// <summary>
/// Debug class for testing the game more easily
/// </summary>
public class DebugerPopup : EditorWindow
{
    [MenuItem("DetectiveGame/Debuger")]
    static void Init()
    {
        DebugerPopup wnd = GetWindow<DebugerPopup>();
        wnd.titleContent = new GUIContent("DetectiveDebuger");
    }

    private TextField fieldStory;
    private TextField fieldFollowers;

    /// <summary>
    /// Creates the GUI
    /// </summary>
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        root.Add(new Label("Set the variables, then reload the map"));

        Label label = new Label("STORY");
        root.Add(label);

        fieldStory = new TextField();
        fieldStory.name = "STORY";
        fieldStory.SetValueWithoutNotify("0");
        root.Add(fieldStory);

        Label labelFollowers = new Label("Followers (;)");
        root.Add(labelFollowers);

        fieldFollowers = new TextField();
        fieldFollowers.name = "Followers";
        fieldFollowers.SetValueWithoutNotify("");
        root.Add(fieldFollowers);

        Button button = new Button();
        button.name = "Accept";
        button.text = "Accept";
        root.Add(button);
        button.clicked += Click;

    }

    /// <summary>
    /// Restarts the map using the parameters specified
    /// </summary>
    public void Click()
    {
        GameManager.instance.SetSaveItem("STORY", fieldStory.text);

        if (!fieldFollowers.text.Equals(""))
        {
            string[] split = fieldFollowers.text.Split(";");

            foreach (string str in split)
            {
                GameManager.instance.Save_AddFollower(str);
            }
        }

        GameManager.instance.LoadMap(SceneManager.GetActiveScene().name);
    }
}