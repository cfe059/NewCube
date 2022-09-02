using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScean : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceanAsync(1));
    }
    IEnumerator LoadSceanAsync(int sceanIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceanIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            yield return null;
        }
    }
    
}
