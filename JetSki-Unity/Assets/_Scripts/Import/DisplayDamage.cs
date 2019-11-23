using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDamage : MonoBehaviour
{
    public Text Txt;
    public Rigidbody rb;
    public string InputText;
    public int FontSize;
    public Color32 DamageColor;
    public Color32 HealColor;

    public enum TextState { Red,Green};
    public TextState state = TextState.Red;

    // Start is called before the first frame update
    void Start()
    {
        if(state == TextState.Red)
        {
            Txt.color = DamageColor;
        }
        if(state == TextState.Green)
        {
            Txt.color = HealColor;
        }
        Txt.text = InputText;
        Txt.fontSize = FontSize;

        rb.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)) * 3000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteObjet()
    {
        Destroy(gameObject);
    }
}
