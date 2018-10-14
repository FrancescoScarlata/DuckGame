using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Classe che gestisce i button generici come il dado o esci
public class ButtonManager : MonoBehaviour {
    public Transform GameHandler;
    public Text ScrittaDelDado;
    float time = 0;
    bool appenatirato = false;
    
    //Fa tornare al menù
    public  void BackToMenu()
    {
        //Torna allo SplashScreen
        SceneManager.LoadScene("SplashScreen");
    }


    public void Update()
    {
        if (appenatirato) 
            if (time < 5)
            {
                  time += Time.deltaTime;
            }
            else
            {
                ScrittaDelDado.gameObject.SetActive(false);
                time = 0;
                appenatirato = false;
            }
           

    }

    //Tira i dadi
    public void Dices()
    {
       
        if (GameHandler.GetComponent<GameHandler>().DomandaInCorso)
            return;
        // parte 1 -> invece dell'animazione del dado, si ha una scritta con il punteggio del dado

        //   Debug.Log("Sto tirando i dadi");
        int dado = Random.Range(1, 6);
        
        ScriviPunteggio(dado);
        GameHandler.GetComponent<GameHandler>().ProssimaDomanda(dado);

    }

    //Scrive il punteggio del dado sul testo inserito
    void ScriviPunteggio(int dado)
    {
        ScrittaDelDado.gameObject.SetActive(true);
        ScrittaDelDado.GetComponent<Text>().text = " Il punteggio del dado è : " + dado;
        appenatirato = true;
        time = 0;
    }

    //Porta alla valutazione
    public void ToTheVauation()
    {
        SceneManager.LoadScene("Valuation");
    }
}
