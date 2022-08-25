using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    // Start is called before the first frame update

    private void Update()
    {

    }
    public void LoadNextLevel()
    {
        Debug.Log("load level");
        StartCoroutine( LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        
    }
    public void LoadSpecificScene(string name)
    {
        Debug.Log("load level");
        StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }
}
