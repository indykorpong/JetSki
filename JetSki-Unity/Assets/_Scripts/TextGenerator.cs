using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGenerator : MonoBehaviour
{
    public GameObject Text;
    public Canvas canvas()
    {
        return FindObjectOfType<Canvas>();
    }


    //Want to create a text, by type a following text at any script
    //! FindObjectOfType<TextGenerator>().CreateText(tranform.position, "HelloWorld!", DisplayDamage.TextState.Red, 32);

    public void CreateText(Vector3 GamePos, string displaytext, DisplayDamage.TextState state, int Fontsize)
    {
        Vector3 ScreenPos = Camera.main.WorldToScreenPoint(GamePos);

        GameObject displaytext_I = Instantiate(Text);
        displaytext_I.transform.SetParent(canvas().transform, false);
        displaytext_I.transform.position = ScreenPos;
        displaytext_I.transform.SetAsFirstSibling();

        DisplayDamage SetText = displaytext_I.GetComponent<DisplayDamage>();

        SetText.state = state;
        SetText.InputText = displaytext.ToString();
        SetText.FontSize = Fontsize;
    }
}
