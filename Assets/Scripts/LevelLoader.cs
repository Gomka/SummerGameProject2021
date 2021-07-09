using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private MovementController movementController;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionDelay = 1f;

    private void OnEnable()
    {
        movementController.RestartLevel+= RestartLevel;
        movementController.NextLevel += NextLevel;
    }

    private void OnDisable()
    {
        movementController.RestartLevel -= RestartLevel;
        movementController.NextLevel -= NextLevel;
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadScene(levelIndex);

    }
}
