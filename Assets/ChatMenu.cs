using System;
using System.Linq;
using TMPro;
using UnityEngine;
using S = UnityEngine.SerializeField;

sealed class ChatMenu : MonoBehaviour
{
    [S] AnimateText animateText;
    [S] TextMeshProUGUI menuText;

    [Multiline(lines: 12)]
    [S] string additionalLines;

    string[] lines = { "hello" };
    string[] linesFrozen = Array.Empty<string>();

    bool visible = false;

    void Awake()
        => menuText.enabled = visible;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            visible ^= true;
            linesFrozen = lines;

            menuText.enabled = visible;
            menuText.text = string.Join(
                separator: "\n",
                values: linesFrozen.Select(
                    (x, i) => $"<b>{i + 1}</b>| {x}"
                )
            );
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
            AnimateText(1);

        if (Input.GetKeyDown(KeyCode.Keypad2))
            AnimateText(2);

        if (Input.GetKeyDown(KeyCode.Keypad3))
            AnimateText(3);

        if (Input.GetKeyDown(KeyCode.Keypad4))
            AnimateText(4);

        if (Input.GetKeyDown(KeyCode.Keypad5))
            AnimateText(5);

        if (Input.GetKeyDown(KeyCode.Keypad6))
            AnimateText(6);

        if (Input.GetKeyDown(KeyCode.Keypad7))
            AnimateText(7);

        if (Input.GetKeyDown(KeyCode.Keypad8))
            AnimateText(8);

        if (Input.GetKeyDown(KeyCode.Keypad9))
            AnimateText(9);

        if (Input.GetKeyDown(KeyCode.Backspace))
            animateText.StopAnimating();

        void AnimateText(int index)
        {
            animateText.AnimateNewText(linesFrozen[index - 1]);
            visible = false;
            menuText.enabled = visible;
        }
    }

    public void SetLines(string[] lines)
        => this.lines = additionalLines
            .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Concat(lines)
            .ToArray();
}
