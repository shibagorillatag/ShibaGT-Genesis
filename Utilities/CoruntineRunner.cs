using UnityEngine;
using Mono;
using System.Collections;

public class CoroutineRunner : MonoBehaviour // Thanks to ShibaGT for helping with the coroutines
{
    public static CoroutineRunner instance = null;

    private void Awake()
    {
        instance = this;
    }

    public static Coroutine RunCoroutine(IEnumerator enumerator)
    {
        return instance.StartCoroutine(enumerator);
    }

    public static void EndCoroutine(Coroutine enumerator)
    {
        instance.StopCoroutine(enumerator);
    }
}