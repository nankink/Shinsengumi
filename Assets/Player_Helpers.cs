using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextFieldUI
{
    Current_State,
    Invunerability,
    CurrentTimeInState,
}

public class Player_Helpers : MonoBehaviour
{
    // Debug
    public Text currentstate;
    public Text invunerability;
    public Text currentTimeInState;

    public SkinnedMeshRenderer mesh;
    [HideInInspector] public Material meshMaterial;

    Color oldColor;

    private void Start()
    {
        meshMaterial = mesh.material;
        oldColor = meshMaterial.color;
    }

    public void ChangeColor(Color color, bool active)
    {
        if (active) meshMaterial.color = color;
        else meshMaterial.color = oldColor;
    }

    public void DisplayText(TextFieldUI type, string text)
    {
        if (type == TextFieldUI.Current_State)
        {
            currentstate.text = text;
        }
        if (type == TextFieldUI.CurrentTimeInState)
        {
            currentTimeInState.text = text;
        }
        if (type == TextFieldUI.Invunerability)
        {
            invunerability.text = text;
        }
    }

}
