using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void switchScene(int scene) {
        SceneManager.LoadScene(scene);
    }
}
