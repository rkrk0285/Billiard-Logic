using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AugmentClick : MonoBehaviour
{
    public PhysicsMaterial2D pm;

    public void OnClick(int code)
    {
        switch(code)
        {
            case 0:
                PlayerPrefs.SetFloat("Power", PlayerPrefs.GetFloat("Power", 0) + 5.0f);
                break;
            case 1:
                PlayerPrefs.SetFloat("Drag", PlayerPrefs.GetFloat("Drag", 0) - 0.1f);
                break;
            case 2:
                PlayerPrefs.SetFloat("Bounciness", PlayerPrefs.GetFloat("Bounciness", 0) - 0.1f);
                pm.bounciness = 0.9f;
                pm.bounciness = Mathf.Max(0, pm.bounciness + PlayerPrefs.GetFloat("Bounciness", 0));
                break;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
