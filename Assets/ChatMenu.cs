using System;
using System.Linq;
using TMPro;
using UnityEngine;
using S = UnityEngine.SerializeField;

sealed class ChatMenu : MonoBehaviour
{
    [S] AnimateText animateText;
    [S] TextMeshPro menuText;

    string[] texts = Array.Empty<string>();
    string[] textsFrozen = Array.Empty<string>();

    bool visible = false;

    void Awake()
        => menuText.gameObject.SetActive(visible);
    
    public void Update()
    {
        if (Input.GetKey(KeyCode.Keypad0))
        {
            visible ^= true;
            textsFrozen = texts;

            menuText.gameObject.SetActive(visible);
            menuText.text = string.Join(
                separator: "\n",
                values: textsFrozen.Select(
                    (x, i) => $"<b>{i + 1}</b>|{x}"
                )
            );
        }

        if (Input.GetKey(KeyCode.Keypad1))
            AnimateText(1);

        if (Input.GetKey(KeyCode.Keypad2))
            AnimateText(2);

        if (Input.GetKey(KeyCode.Keypad3))
            AnimateText(3);

        if (Input.GetKey(KeyCode.Keypad4))
            AnimateText(4);

        if (Input.GetKey(KeyCode.Keypad5))
            AnimateText(5);

        if (Input.GetKey(KeyCode.Keypad6))
            AnimateText(6);

        if (Input.GetKey(KeyCode.Keypad7))
            AnimateText(7);

        if (Input.GetKey(KeyCode.Keypad8))
            AnimateText(8);

        if (Input.GetKey(KeyCode.Keypad9))
            AnimateText(9);

        void AnimateText(int index)
        {
            animateText.AnimateNewText(textsFrozen[index - 1]);
            visible = false;
        }
    }

    public void SetTexts(string[] texts)
        => this.texts = texts;
}
