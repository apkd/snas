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

    void Awake()
        => text = GetComponent<TMP_Text>();

    void OnEnable()
        => StartCoroutine(AnimateCoroutine());

    IEnumerator AnimateCoroutine()
    {
        text.ForceMeshUpdate();

        TMP_TextInfo textInfo = text.textInfo;
        int currentCharacter = 0;

        var colorVisible = text.color;
        text.color = new Color32(255, 255, 255, 0);
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        yield return new WaitForSeconds(0.25f);

        while (enabled)
        {
            int characterCount = textInfo.characterCount;

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

            AudioSource.PlayClipAtPoint(
                clip: audioClip,
                position: default
            );

            currentCharacter = (currentCharacter + 1) % characterCount;

            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        }
    }

    public void SetNewText(string value)
    {
        text.text = value;
        text.color = new Color32(255, 255, 255, 0);
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
