using System.Collections;
using TMPro;
using UnityEngine;
using S = UnityEngine.SerializeField;

public sealed class AnimateText : MonoBehaviour
{
    TMP_Text text;

    [S] AudioClip audioClip;

    [S] float minWait = 0.05f;
    [S] float maxWait = 0.1f;

    [S] float spaceDelay = 0.1f;
    [S] float commaDelay = 0.5f;

    Color colorVisible;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        colorVisible = text.color;
    }

    void OnEnable()
        => StartCoroutine(AnimateCoroutine());

    IEnumerator AnimateCoroutine()
    {
        int currentCharacter = 0;

        text.color = new Color32(255, 255, 255, 0);
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        yield return new WaitForSeconds(0.25f);

        int characterCount = text.textInfo.characterCount;

        while (currentCharacter < text.textInfo.characterCount)
        {
            var textInfo = text.textInfo;

            if (characterCount == 0)
            {
                yield return new WaitForSeconds(0.25f);
                continue;
            }

            int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

            var vertexColors = textInfo.meshInfo[materialIndex].colors32;

            int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

            vertexColors[vertexIndex + 0] = colorVisible;
            vertexColors[vertexIndex + 1] = colorVisible;
            vertexColors[vertexIndex + 2] = colorVisible;
            vertexColors[vertexIndex + 3] = colorVisible;

            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);


            var character = textInfo.characterInfo[currentCharacter].character;

            if (character == ',' || character == ';')
            {
                yield return new WaitForSeconds(commaDelay);
            }
            else if (character == ' ')
            {
                yield return new WaitForSeconds(spaceDelay);
            }
            else
            {
                AudioSource.PlayClipAtPoint(
                    clip: audioClip,
                    position: default
                );
            }

            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            currentCharacter += 1;
        }
    }

    public void SetNewText(string value)
    {
        text.SetText(value);
        StopAllCoroutines();
        StartCoroutine(AnimateCoroutine());
    }
}
