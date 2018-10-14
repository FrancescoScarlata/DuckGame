using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Metodo per tornare indietro dai ringraziamenti
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("SplashScreen");
    }
}
