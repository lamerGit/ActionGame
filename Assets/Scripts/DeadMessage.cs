using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadMessage : MonoBehaviour
{
    Button button;


    private void Awake()
    {
        button=GetComponentInChildren<Button>();
        button.onClick.AddListener(StageReset);
        gameObject.SetActive(false);
    }


    void StageReset()
    {
        SceneManager.LoadScene("Act01");
    }

}
