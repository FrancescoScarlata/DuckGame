using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NumGiocatoriController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    //Metodo usato dai pulsanti all'interno della scena per scegliere il numero di giocatori
    public void Giocatori(int numeroGIocatori)
    {
        switch(numeroGIocatori)
        {
            case 1:
                PlayerPrefs.SetInt("NumPlayersLength", 1);
                break;

            case 2:
                PlayerPrefs.SetInt("NumPlayersLength", 2);
                break;

            case 3:
                PlayerPrefs.SetInt("NumPlayersLength", 3);
                break;
            default:
                Debug.Log("Non dovrebbe spuntare");
                return;
        }

        SceneManager.LoadScene("Game");
        
    }

    //Metodo usato per tornare dalla scelta del numero di personaggi allo SplashScreen
    public void GoBack()
    {
        SceneManager.LoadScene("SPlashScreen");
    }

}
