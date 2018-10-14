using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//Classe che gestisce l'inizializzazione delle domande, la struttura randomica del gioco e il cammino del gioco
public class GameHandler : MonoBehaviour {

    //I Riquadri dei tipo delle domande e le parti del canvas che devono essere tolte 

    /*
        I tipi di domande sono i tipi di domande che vengono usate all'interno della mappa.
        In base al tipo di domande si hanno domande e risposte su argomenti diversi.
        Ci sono tra queste anche la zona di Start, di Finish e di Free Pass dove rispettivamente il gioco inizia, 
        finisce e si passa senza aver risposto alla domanda.
  
    */
    public GameObject[] TipiDiDomande;
   
    // Avendo AnswerHandler è possibile prendere dai figli di questa Classe
    // i numeri sotto definiti : 2),3-6, 8,9
    // Essendo a stretto contatto tra loro... è necessario ottimizzare?
    /*
    Tutte le Parti Ui necessarie: (si parte da 0)
    1) Il panel Con il background
    2) Il testo con la domanda
    3) La risposta num1
    4) La risposta num2
    5) La risposta num3
    6) La risposta num4
    7) Il button per confermare
    8) Immagine per i grafici
    9) Campo Input per le risposte aperte
   10) Il testo del tempo
    */
    public RectTransform [] QuestionZone;

    //Parte che gestisce le risposte. Bisogna prendere dai figli i rect e così trasferire così 
    // da questa classe alla sottoindicata
    public Transform AnswerHandler;

    public RectTransform ValuationButton;

    //Giocatori  chiesti. Vengono richiesti nel menu principali tramite finestra durante la scelta del livello
    public GameObject[] Players;

    public bool DomandaInCorso = false;

    // Da sistemara poi la i turni ecc
    public int turno=0;
    public int numeroGiocatori;

    //La mappa che verrà creata. Dipenderà dalla terreno di gioco ( il numero sarà variabile)
    GameObject[] Mappa = new GameObject[40];

    float clocktime = 0;
    
  
	// Inizializza la mappa e inizializza per far muovere il dado
	void Start () {
        ValuationButton.gameObject.SetActive(false);
        numeroGiocatori = PlayerPrefs.GetInt("NumPlayersLength");
        CreaMappa();
       // AnswerHandler.GetComponent<AnswerHandler>().Start();

        for (int i = 0; i < Players.Length; i++)
            Players[i].GetComponent<PlayerManager>().posizione = 0;

        aggiustamenti();

        NascondiUI();
        QuestionZone[1].GetComponent<Text>().text ="Tira i Dadi per iniziare!";
    }

    //inizia la mossa dopo aver tirato i dadi 
    //C'è anche il " Hai vinto"
    public void ProssimaDomanda(int dado )
    {
        Muovi(dado);
        DomandaInCorso = true;
        if (Players[turno%numeroGiocatori].GetComponent<PlayerManager>().posizione < Mappa.Length-1) {
            GestisciDomanda();
            Players[turno % numeroGiocatori].GetComponent<PlayerManager>().Anima();
        }
        else {
           NascondiUI();
           QuestionZone[1].GetComponent<Text>().text = "Hai Vinto";

            faiPartireLeParticelle();
            faiPartireLoShowResults(numeroGiocatori);
        }
    }

    // Per fare l' update del tempo
    void Update () {
        clocktime += Time.deltaTime;
        QuestionZone[QuestionZone.Length-1].GetComponent<Text>().text= "Tempo: "+(int)clocktime + " sec";
	}

    //Crea la mappa di gioco. Percorso a spirale.
    void CreaMappa() {
        Vector3 Coordinate;
        switch (numeroGiocatori) {
            case 1:
                Coordinate = new Vector3(0, 0, 0);
                break;
            case 2:
                Coordinate = new Vector3(0, 0.1f, 660);
                break;

            default:
                Coordinate = new Vector3(0, 0, 0);
                return;
        }
        Mappa[0] = GameObject.Instantiate(TipiDiDomande[0]); // Nella prima casella viene posizionato lo start
        Mappa[0].transform.localPosition += Coordinate;
        
        for (int i = 1; i < Mappa.Length - 1; i++) {        
            if (i >= 1 & i <= 12) // Percorso avanti
                Coordinate += new Vector3(0, 0, 10);
            if (i > 12 & i <= 23)  // Percorso Sinistra
                Coordinate += new Vector3(-10, 0, 0 );
            if (i > 23 & i <= 32) // Percorso Indietro
                Coordinate += new Vector3(0, 0, -10);
            if (i > 32 & i < Mappa.Length-1) // Percorso Opposto sinistra
                Coordinate += new Vector3(10, 0, 0);
           if (i % 5 == 0) { // passaggio liberi ogni 5 caselle            
                Mappa[i] = GameObject.Instantiate(TipiDiDomande[TipiDiDomande.Length - 2]);
                Mappa[i].transform.localPosition += Coordinate;
            }
            else {
                Mappa[i] = GameObject.Instantiate(TipiDiDomande[Random.Range(1, TipiDiDomande.Length - 2)]);
                Mappa[i].transform.localPosition += Coordinate;
            }
        }
      
           Coordinate += new Vector3(10, 0, 0);
        Mappa[Mappa.Length - 1] = GameObject.Instantiate(TipiDiDomande[TipiDiDomande.Length - 1]); // nell'ultima casello il finish
        Mappa[Mappa.Length-1].transform.localPosition += Coordinate;
    }

    //Muoverà il personaggio e cambierà la sua posizione. Bisogn cambiare in base al turno e alla posizione del giocatore
    void Muovi(int tragitto){
            Players[turno%numeroGiocatori].GetComponent<PlayerManager>().MuoviPupo(tragitto, Mappa.Length, turno % numeroGiocatori);       
    }

    //Gestisce il processo della creazione della domanda
    void GestisciDomanda()
    {
        // 1)  analizza il tipo di domanda
        int tipoDomanda = analizzaDomanda();
     
        // 2) Mostra la UI in base al tipo di domanda
        MostraUI(tipoDomanda);

        // 3) prende la domanda e la/e risposta/e
        string[] risposte = prendiDomandaERisposte(tipoDomanda);

        // 4) Randomizza le risposte
        randomizzaRisposte(risposte);

    }

    //Prende le UI e le rende visibili in base alla domanda
    void MostraUI(int tipoDomanda)
    {
        //Per le Risposte multiple senza grafici
        if (tipoDomanda == 1 || tipoDomanda == 2 || tipoDomanda == 4 || tipoDomanda == 5){
            for (int i = 0; i < QuestionZone.Length - 3; i++)
                QuestionZone[i].gameObject.SetActive(true);
            return;
        }
      
        if (tipoDomanda == 3) { //Per le risposte aperte senza grafici
            QuestionZone[0].gameObject.SetActive(true);
            QuestionZone[1].gameObject.SetActive(true);
            QuestionZone[6].gameObject.SetActive(true);
            QuestionZone[8].gameObject.SetActive(true);
            return;
        }
        if (tipoDomanda == 6){  // Risposte Multiple con grafici
            for (int i = 0; i < QuestionZone.Length - 2; i++)
                QuestionZone[i].gameObject.SetActive(true);
            return;
        }
        if (tipoDomanda == 7) { // Risposta Aperta con grafici
            QuestionZone[0].gameObject.SetActive(true);
            QuestionZone[1].gameObject.SetActive(true);
            QuestionZone[6].gameObject.SetActive(true);
            QuestionZone[7].gameObject.SetActive(true);
            QuestionZone[8].gameObject.SetActive(true);
            return;
        }
        if (tipoDomanda == 8) { // Ha preso il Free Pass

            QuestionZone[1].gameObject.SetActive(true);
            QuestionZone[1].GetComponent<Text>().text = "Passaggio Libero! Tira di Nuovo!";
            DomandaInCorso = false;
            return;
        }
   }

    //nasconde le tutte le ui richieste
   public void NascondiUI()
    {
        //Debug.Log("Sto nascondendo la UI");
        QuestionZone[0].gameObject.SetActive(false);
        for (int i = 2; i < QuestionZone.Length-1; i++)
            QuestionZone[i].gameObject.SetActive(false);
    }

    // COnverte il tipo di domanda in un numero
    int analizzaDomanda() {

        if (Mappa[Players[turno%numeroGiocatori].GetComponent<PlayerManager>().posizione].tag == "Aritmetica")
            return 1;
        if (Mappa[Players[turno % numeroGiocatori].GetComponent<PlayerManager>().posizione].tag == "Funzioni 1")
            return 2;
        if (Mappa[Players[turno % numeroGiocatori].GetComponent<PlayerManager>().posizione].tag == "Funzioni 2")
            return 3;
        if (Mappa[Players[turno % numeroGiocatori].GetComponent<PlayerManager>().posizione].tag == "Geometria")
            return 4;
        if (Mappa[Players[turno % numeroGiocatori].GetComponent<PlayerManager>().posizione].tag == "Statistica")
            return 5;
        if (Mappa[Players[turno % numeroGiocatori].GetComponent<PlayerManager>().posizione].tag == "Grafici 1")
            return 6;
        if (Mappa[Players[turno % numeroGiocatori].GetComponent<PlayerManager>().posizione].tag == "Grafici 2")
            return 7;
        if (Mappa[Players[turno % numeroGiocatori].GetComponent<PlayerManager>().posizione].tag == "Free Pass")
            return 8;

        return -1;
    }

    /*
        In questo metodo bisogna inserire :
        - la Domanda e mostrarla
        - Il Grafico e mostrarlo
        - le risposte e mostrarle o mostrare il campo di testo
     
        */
     string[] prendiDomandaERisposte(int tipoDomanda) {
        // serve per dare un valore di default. Alla fine viene modificato quindi nessun problema
        int domandaScelta=-1;

        if (tipoDomanda != 8) // Prende la domanda da file
            domandaScelta = prendiDomandaDaFile(tipoDomanda);
        else {
            // Non deve fare partire niente perché non si hanno domande
            return null;
        }
        
        //Se è un grafico deve prendere l'immagine. Inserite all'interno come prefab tra le immagini o si posso prendere in altra maniera?
        if( tipoDomanda==6|| tipoDomanda== 7){
            // Chiama un metodo all'interno dell'immagine per prendere un'immagine in base alla domanda scelta
            // Nella sezione sarà simile il comportamento, tranne per la sistemazione della domanda in base all'immagine
           QuestionZone[7].GetComponent<Graphic_Controller>().pickAGraphic(domandaScelta);
        }

        //Inserisce domande con risposte multiple
        if(tipoDomanda==1|| tipoDomanda == 2 || tipoDomanda == 4 || tipoDomanda == 5 || tipoDomanda == 6){
            // Prende le successive 4 righe
            return prendiRisposte(tipoDomanda,domandaScelta,true);
        }
        //Inserisce domande con risposte aperte
        if (tipoDomanda == 3 || tipoDomanda == 7)
        {
            // Bisogna cercarla?
            return prendiRisposte(tipoDomanda, domandaScelta, false);
            // Prende la  successiva riga
        }
        return null;
    }
    
    // Metodo che deve randomizzare le risposte e collocare la risposta esatta nella casella corretta.
    void randomizzaRisposte(string [] risposte) {

        // La risposta è aperta
        if (risposte==null|| risposte.Length < 1 || risposte[1] == null)
            return;

        // ordine delle risposte
         int [] order= AnswerHandler.GetComponent<AnswerHandler>().randomize();

        // Si assegna la risposta alla rispettiva casella.
        // Se risposte multiple
        // le question zone ( delle risposte multiple) sono tra 2 e 5
        for (int i=2; i<6; i++)
        {
            QuestionZone[i].GetComponentInChildren<Text>().text = risposte[order[i-2]];
            QuestionZone[i].GetComponent<Toggle>().isOn = false;
            // Si suppone che la risposta esatta sia la prima della lista
            if (order[i - 2] == 0)
                QuestionZone[i].GetComponent<AnswerInfo>().rightAnswer = true;
            else
                QuestionZone[i].GetComponent<AnswerInfo>().rightAnswer = false;

        }

    }

    // temporaneo. il tempo di fare il vero metodo in quello giusto
    string [] prendiRisposte(int tipoDomanda, int domandaScelta,bool multipla)
    {
        if (domandaScelta == -1)
            return null;

        string[] risposte;
       
        risposte = AnswerHandler.GetComponent<AnswerHandler>().leggiRisposte(tipoDomanda,domandaScelta, multipla);
        return risposte;
    }

    // Metodo Che prende la domanda da file
    int prendiDomandaDaFile(int tipoDomanda)
    {
        // Serve un indice per sapere quante domande ci sono. Da scrivere sul Player Prefs o su un file apposito.
        // Si suppone che la prima riga sia per la domanda e le successive per le domande.
        // Le risposte successive vengono scritte su righe diverse. La prima è quella giusta, le altre quelle sbagliate

        if (tipoDomanda==1|| tipoDomanda == 2 || tipoDomanda == 4 || tipoDomanda == 5 || tipoDomanda == 6 )
          return AnswerHandler.GetComponent<AnswerHandler>().leggiDomanda(tipoDomanda, true);
        if (tipoDomanda==3|| tipoDomanda == 7)
            return AnswerHandler.GetComponent<AnswerHandler>().leggiDomanda(tipoDomanda, false);

        return -1;
    }

    //Parte sulla valutazione.   
    void faiPartireLoShowResults(int numeroGiocatori)
    {
             // prende le risposte dal giocatore vincente
            int[] esatte = Players[turno % numeroGiocatori].GetComponent<PlayerManager>().ShowCorrectAnswers();
            int[] sbagliate = Players[turno % numeroGiocatori].GetComponent<PlayerManager>().ShowWrongAnswers();

            InserisciPerLaValutazione(sbagliate, esatte);
    }
    
    // Da fare salva su PlayerPrefs i dati da salvare 
    void InserisciPerLaValutazione(int [] sbagliate, int[] esatte)
    {
        ValuationButton.gameObject.SetActive(true);
        //Mette a video i risultato del giocatore vincente
       
            string tempsbagliate = "";
            string tempesatte = "";
           
            for (int i = 0; i < sbagliate.Length; i++) {
                tempesatte += esatte[i] + " ";
                tempsbagliate += sbagliate[i] + " ";
               
                PlayerPrefs.SetString("Esatte", tempesatte);
                PlayerPrefs.SetString("Sbagliate", tempsbagliate);
            }
    }

    // Aggiustamenti da Fare se ci sono più segnaposto o solo 1
    void aggiustamenti()
    {
        switch (numeroGiocatori)
        {
            case 1:
                {
                    Players[0].transform.Rotate(new Vector3(0, -90, 0));
                    Players[0].transform.position += new Vector3(+2.5f, 0, -1);
                    Players[1].gameObject.SetActive(false);
                    break;
                }

            case 2:
                {
                    Players[0].transform.position += new Vector3(0, 0, 660);
                    Players[0].GetComponent<PlayerManager>().CameraController.transform.position += new Vector3(0, 0, 660);
                    Players[1].transform.position += new Vector3(0, 0, 660);
                    Players[1].GetComponent<PlayerManager>().CameraController.transform.position += new Vector3(0, 0, 660);

                    break;
                }

            case 3:
                {
                    break;
                }
            default: return;

        }



    }

    void faiPartireLeParticelle()
    {
        ParticleSystem[] emittenti;
        emittenti = Mappa[Mappa.Length - 1].GetComponentsInChildren<ParticleSystem>();
        emittenti[0].Play();
        emittenti[1].Play();
    }
   
}
