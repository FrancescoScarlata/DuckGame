using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class SplashController : MonoBehaviour {

    //Metodo che avvia la nuova partita. Porta al secondo livello dove chiede il numero di giocatori
    public void newGame()
    {
        SceneManager.LoadScene("NumGiocatori");
     }

    //Metodo per inserire le Opzioni. Metodo a parte o finestra?
    public void Options()
    {
        SceneManager.LoadScene("Options");
    }

    //Metodo per Fare vedere i crediti. Metodo a parte o finestra?
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }


}
