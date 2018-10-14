using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Linq;

public class ValuationScript : MonoBehaviour {

    /*
  I text usati qui sono :
  0) Media : la media tra le risposte esatte e quelle sbagliate
   
  1) Suggerimento: In base ai punteggio ottenuto si dirà un consiglio
  2) Esatte: Bisogna metterlo visibile e inserire al suo interno i risultati ottenuti
  3) Sbagliate:  Bisogna metterlo visibile e inserire al suo interno i risultati ottenuti
  4) Descrizione : qualocosa di breve e "spiritoso" sul punteggio preso
    */
    public RectTransform[] ALotOfTexts;

    private float media = 0;

    private int[] sbagliate=new int[7];
    private int[] esatte = new int[7];
    // I due path per le descrizioni e i suggerimenti (rispettivamente 0 e 1)
    public string[] NameAssets;
    private string [] Descrizioni;
    private string[] Suggerimenti;

    // booleano che indica i campi con più risposte sbagliate di quelle giuste
    private bool[] piùsbaglichealtro= new bool [7];

    // Use this for initialization
    void Start () {

        PrendiDatiDaPlayerPrefs();

        //Mostra la media
        float media = calcoloMedia(sbagliate, esatte);
        ALotOfTexts[0].GetComponent<Text>().text = "Punteggio : " + string.Format("{0:#.# }", media);

        ScriviUnaDescrizione(media);
        posizionaPunteggiRisposte();
    
        MandaUnSuggerimento();

    }
	
    // Prende i dati Salvati in PlayerPrefs sui risultati e li salva negli array
    void PrendiDatiDaPlayerPrefs()
    {
        string[] tempesatte = PlayerPrefs.GetString("Esatte").Split(' ');
        string[] tempsbagliate = PlayerPrefs.GetString("Sbagliate").Split(' ');
       
        for (int i=0; i<tempesatte.Length-1; i++) {
           esatte[i] = int.Parse(tempesatte[i].Substring(0,tempesatte[i].Length));
            sbagliate[i] = int.Parse(tempsbagliate[i].Substring(0, tempsbagliate[i].Length));
        }
    }

    //Calcola la media tra risposte esatte e sbagliate
    float calcoloMedia(int[] sbagliate, int[] esatte)
    {
        int tot_esatte = 0;
        float tot_sbagliate = 0f;
        // sbagliate ed esatte hanno la stessa lunghezza, quindi non ci sono problemi di eccezioni
        for (int i = 0; i < sbagliate.Length; i++) {
            tot_esatte += esatte[i];
            tot_sbagliate += sbagliate[i];
        }
        media = tot_esatte / (tot_esatte + tot_sbagliate) * 10f;
        return media;
    }

    // Prende i text figli di esatte e sistema i dati dell'array dentro le caselle
    void posizionaPunteggiRisposte()
    {
        Text[] CaselleEsatte = ALotOfTexts[2].GetComponentsInChildren<Text>();
        Text[] CaselleSbagliate = ALotOfTexts[3].GetComponentsInChildren<Text>();

        for (int i=0; i<piùsbaglichealtro.Length; i++) {
            piùsbaglichealtro[i] = false;
        }

        for (int i = 1; i < CaselleEsatte.Length; i++) {
           CaselleEsatte[i].text = "" + esatte[i - 1];
            CaselleSbagliate[i].text = "" + sbagliate[i - 1];

            //Se ha fatto più errori rispetto alle risposte esatte in quel campo
            if (sbagliate[i - 1] > esatte[i - 1])  {
                CaselleEsatte[i].color = Color.red;
                CaselleSbagliate[i].color = Color.red;
                piùsbaglichealtro[i - 1] = true;
            }
               
        }
    }
    
    // Scrive qualcosa nella casella suggerimento
    void MandaUnSuggerimento()
    {
        Suggerimenti = ((TextAsset)Resources.Load(NameAssets[1])).text.Split('\n');
        string perMigliorare = "Devi migliorare nel campo ";
        for (int i=0; i<piùsbaglichealtro.Length; i++)  {
            if (piùsbaglichealtro[i]) {               
                if (i == 2 || i == 6) {
                    if (piùsbaglichealtro[i] != piùsbaglichealtro[i - 1]) {
                        if (i == 2)
                            perMigliorare += ""  + Suggerimenti[i - 1] + ", ";
                        if (i == 6)
                            perMigliorare += "" + Suggerimenti[i - 1] + ", ";
                    }
                 }
                else
                    perMigliorare += ""  + Suggerimenti[i] + ", ";
            }  
        }
        //Frase nel  caso non ci siano più errori che altro in nessun campo
        if (!piùsbaglichealtro[0] && !piùsbaglichealtro[1] &&! piùsbaglichealtro[2] && !piùsbaglichealtro[3] && !piùsbaglichealtro[4] && !piùsbaglichealtro[5] && !piùsbaglichealtro[6])
        {
            perMigliorare = "Nel complesso sei messo bene. Continua Così!  ";
        }

        ALotOfTexts[1].GetComponent<Text>().text = perMigliorare.Substring(0, perMigliorare.Length - 3) + ".";


    }

    //Metodo che prende visione del punteggio ottenuto e scrive un commento preso da file
    void ScriviUnaDescrizione(float media)
    {
        string commentoDaScrivere = "";
        TextAsset commenti = (TextAsset)Resources.Load(NameAssets[0]);
        Descrizioni = commenti.text.Split('\n');
        //Ha il numero di riga da cui iniziano i commenti sul rispettivo punteggio
        string[] primoRigo = Descrizioni[0].Split(' ');
        
        if (media>=0 && media < 2) {
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[0]) + Random.Range(0, 2)];
        }
        if (media >= 2 && media < 3) {
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[1]) + Random.Range(0, 2)];
        }
        if (media >= 3 && media < 4) {
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[2]) + Random.Range(0, 2)];
        }
        if (media >= 4 && media < 5){
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[3]) + Random.Range(0, 2)];
        }
        if (media >= 5 && media < 6) {
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[4]) + Random.Range(0, 2)];
        }
        if (media >= 6 && media < 7) {
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[5]) + Random.Range(0, 2)];
        }
        if (media >= 7 && media < 8) {
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[6]) + Random.Range(0, 2)];
        }
        if (media >= 8 && media < 9) {
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[7]) + Random.Range(0, 2)];
        }
        if (media >= 9 && media < 10) {
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[8]) + Random.Range(0, 2)];
        }
        if (media == 10){
            commentoDaScrivere = Descrizioni[int.Parse(primoRigo[9]) + Random.Range(0, 2)];
        }

        ALotOfTexts[4].GetComponent<Text>().text = "Valutazione :\n"+commentoDaScrivere;
    }

    //Metodo per far tornare al menu principale
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("SplashScreen");
    }

    
}
