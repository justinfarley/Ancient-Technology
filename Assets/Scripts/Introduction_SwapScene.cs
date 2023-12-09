using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Introduction_SwapScene : MonoBehaviour
{
    public void SwapScene()
    {
        SceneManager.LoadScene(2);
    }
}
