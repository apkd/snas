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

    [S] AnimationCurve pitchShiftCurve;

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

            if (character == ',' || character == ';' || character == '\n')
            {
                yield return new WaitForSeconds(commaDelay);
            }
            else if (character == ' ')
            {
                yield return new WaitForSeconds(spaceDelay);
            }
            else if (currentCharacter % 2 == 0)
            {
                PlayAudio(audioClip, pitchShiftCurve.Evaluate(Random.value));
            }

            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            currentCharacter += 1;
        }
    }

    static void PlayAudio(AudioClip audioClip, float pitch)
    {
        var gameObject = new GameObject("One shot audio") { hideFlags = HideFlags.HideAndDontSave };
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = 1f;
        audioSource.pitch = pitch;
        audioSource.Play();
        Destroy(gameObject, audioClip.length * (Time.timeScale < 0.1f ? 0.01f : Time.timeScale));
    }

    public void AnimateNewText(string value)
    {
        text.SetText(value);
        StopAllCoroutines();
        StartCoroutine(AnimateCoroutine());
    }

    public void StopAnimating()
    {
        text.SetText("");
        StopAllCoroutines();
    }
}
