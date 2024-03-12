using System.Collections;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private float secondsBetweenChars;

    public void WriteText(string message)
    {
        StopAllCoroutines();
        StartCoroutine(WriteTextCoroutine(message));
    }

    private IEnumerator WriteTextCoroutine(string message)
    {
        text.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            text.text += message[i];
            yield return new WaitForSeconds(secondsBetweenChars);
        }
        yield return new WaitForSeconds(2);
        text.text = "";
    }
}
