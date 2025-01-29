using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KeybindSetter : MonoBehaviour
{

    public InputActionReference action;
    public int bindingIndex;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    public TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh.text = action.action.GetBindingDisplayString(bindingIndex);
    }

    void Update()
    {

        if (rebindOperation != null && rebindOperation.completed)
        {
            action.action.Enable();// after this you can use the new key
            textMesh.text = action.action.GetBindingDisplayString(bindingIndex);
            SaveUserRebinds();
            rebindOperation = null;
        }
    }

    public void StartInteractiveRebind()
    {
        action.action.Disable(); // critical before rebind!!!

        textMesh.text = "-";
        rebindOperation = action.action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start();
    }

    void SaveUserRebinds()
    {
        var rebinds = PlayerController.Instance.playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
