using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Linq;

// Classe usata per la gestione delle domande e risposte
public class AnswerHandler : MonoBehaviour
{
    public Transform gameController;
    public Text QuestionText;
    public Text RoundsDone;
    public string[] NameAssets;
    TextAsset[] files;
    public Text AnswerResult;
   
    // I giocatori
    public GameObject[] Players;

    //Prende le risposte Multiple -> possono essere prese qui al posto di prenderle da GameHandler
    // E usare rindondanza
    public Toggle[] answers;
    public InputField OpenAnswer;

    private int rounds;
    string rispostaAperta;

    private int lastQuestion;
    
    void Start()
    {
       // Debug.Log("Entra in start e la lunghezza è : "+NameAssets.Length);
        if( files == null){
          //  Debug.Log("Entra in if");
            files = new TextAsset[NameAssets.Length];
            for (int i = 0; i < NameAssets.Length; i++)
            {
                files[i] = (TextAsset)Resources.Load(NameAssets[i]);
                 
            }
        }
        
    }

    //Rende i toggle esclusivi. Gestisce i toggle delle risposte multiple
    public void ToogleHandler(int toogle)
    {
        switch (toogle)
        {
            case 1:
                if (answers[0].isOn) {
                    answers[1].isOn = false;
                    answers[2].isOn = false;
                    answers[3].isOn = false;

                }
                break;
            case 2:
                if (answers[1].isOn)
                {
                    answers[0].isOn = false;
                    answers[2].isOn = false;
                    answers[3].isOn = false;

                }
                break;
            case 3:
                if (answers[2].isOn)
                {
                    answers[0].isOn = false;
                    answers[1].isOn = false;
                    answers[3].isOn = false;
                }
                break;

            case 4:
                if (answers[3].isOn)
                {
                    answers[0].isOn = false;
                    answers[1].isOn = false;
                    answers[2].isOn = false;
                }
                break;

            default:
                break;
        }
    }

    //Conferma la risposta e controlla la scelta effettuata
    public void Submit()
    {
        //Per la gestione delle risposte Multiple
        bool allFalse = true;
        for (int i = 0; i < answers.Length; i++)
            if (answers[i].isOn)
                allFalse = false;
    
        // Controlla che siano o i toggle tutti disattivati o la risposta aperta non attiva
        if (allFalse && !OpenAnswer.IsActive())
            return;

        bool correctAnswer = false;
        for (int i = 0; i < 4; i++)
        {
            if (answers[i].isOn && answers[i].GetComponent<AnswerInfo>().rightAnswer)
                correctAnswer = true;
            answers[i].gameObject.SetActive(false);
        }

        //Controllo sulle risposte multiple
        if (correctAnswer&& !OpenAnswer.IsActive())
        {
            AnswerResult.color = Color.white;
            AnswerResult.text = "La risposta è corretta";
            
            QuestionText.text = "Bravo! Tira di nuovo i dadi!";
            Players[gameController.GetComponent<GameHandler>().turno % gameController.GetComponent<GameHandler>().numeroGiocatori].GetComponent<PlayerManager>().addCorrectAnswer(lastQuestion);        
        }
        else if( !OpenAnswer.IsActive())
        {
            AnswerResult.color = Color.red;
            AnswerResult.text = "La risposta è sbagliata";
            
            QuestionText.text = "Potevi Fare di meglio. Magari andrai Meglio nella prossima!. Tira i dadi";
            Players[gameController.GetComponent<GameHandler>().turno % gameController.GetComponent<GameHandler>().numeroGiocatori].GetComponent<PlayerManager>().addWrongAnswer(lastQuestion);
            CambioTurno();
        }

        // Controllo sulle risposte aperte
        if (checkOpenAnswer()) {
            if (controllaRisposta()) {
                AnswerResult.color = Color.white;
                AnswerResult.text = "La risposta è corretta";
                // Si inserirà qui un contatore sulle risposte esatte
                QuestionText.text = "Bravo! Tira di nuovo i dadi!";
                Players[gameController.GetComponent<GameHandler>().turno % gameController.GetComponent<GameHandler>().numeroGiocatori].GetComponent<PlayerManager>().addCorrectAnswer(lastQuestion);
            }
            else {
                AnswerResult.color = Color.red;
                AnswerResult.text = "La risposta è sbagliata";
                // Si inserirà qui un contatore sulle risposte sbagliate
                // Bisogna controllare se c'è un solo giocatore o più giocatore: se 1 allora va avanti, 
                // altrimenti tocca al giocatore successivo.
                QuestionText.text = "Potevi Fare di meglio. Magari andrai Meglio nella prossima!. Tira i dadi";
                Players[gameController.GetComponent<GameHandler>().turno % gameController.GetComponent<GameHandler>().numeroGiocatori].GetComponent<PlayerManager>().addWrongAnswer(lastQuestion);
                CambioTurno();

            }
        }
        else if (OpenAnswer.IsActive())
        {
            // Quando è una stringa con meno di un carattere sarebbe giusto non poter confermare la risposta
            AnswerResult.text = "Un po' di Impegno!";
            return;
        }

        // Continua a non rimuovere la stringa. Biogna capire perchè
        OpenAnswer.textComponent.text=" ";
        RoundsDone.text = " Domande Fatte : " + ++rounds;
        gameController.GetComponent<GameHandler>().DomandaInCorso = false;
        gameController.GetComponent<GameHandler>().NascondiUI();
    }

    // Qui bisogna controllare se la risposta scritta è Uguale a quella che dovrebbe essere. Da fare
    bool checkOpenAnswer()
    {
        string answer = OpenAnswer.textComponent.text;
        if (OpenAnswer.IsActive() && OpenAnswer.textComponent.text.Length >= 1)
            return true;
        else
            return false;
    }

    // Metodo che partendo dall'ordine 1-2-3-4 , arriva ad un permutazione pseudo-casuale di tale ordine
    public int[] randomize()
    {
        // crea la permutazione ordinata
        int[] Order = new int[4];
        for (int i = 0; i < Order.Length; i++)
            Order[i] = i;

        // ciclo per mescolare i numeri interni
        for (int i = 0; i < Random.Range(0, 100); i++)
        {
            int rand1 = Random.Range(0, 3);
            int rand2 = Random.Range(0, 3);
            // Per cercare di non scambiare inutilmente lo stesso numero
            if (rand1 == rand2)
                rand2 = Random.Range(0, 3);
            int temp = Order[rand1];

            Order[rand1] = Order[rand2];
            Order[rand2] = temp;
        }
        Debug.Log(Order[0]+" , "+ Order[1] + " , " + Order[2] + " , " + Order[3]);
        return Order;
    }

    // Qui si controllerà la risposta aperta in base alla risposta data dall'utente
    bool controllaRisposta() {
        string risposta = OpenAnswer.textComponent.text;
       bool tuttoGiusto = false;
        //Dallo split si ha che lo spazio/a capo viene inserito alla fine della stringa. quindi 
        try {
            for (int i = 0; i < rispostaAperta.ToCharArray().Length; i++) { //L'ultimo carattere è l'invio
            
                if (rispostaAperta.ToCharArray()[i] != 13) { //bisogna eliminare Dalla stringa il carattere finale(l'andare a capo)
                 
                    if (string.Equals(char.ToLower(rispostaAperta.ToCharArray()[i]), char.ToLower(risposta.ToCharArray()[i])))
                        tuttoGiusto = true;       
                    else
                        return false;
                }
            }
        }
        catch (System.Exception) { // La risposta scritta è più piccola della esatta       
            tuttoGiusto = false;
        }
        if (rispostaAperta.Length < risposta.Length)
            tuttoGiusto = false;
        return tuttoGiusto;
    }

    // Metodo che legge la domanda da file. Come output dà la domanda scelta.
    public int leggiDomanda(int tipoDomanda, bool multipla)
    {
        lastQuestion = tipoDomanda-1;
        int domandaScelta=-1;
        string[] fileContent = files[tipoDomanda-1].text.Split("\n"[0]);
        // Legge la prima riga: ha il numero delle domande all'interno del file
        int numeroDomande = int.Parse(fileContent[0]);
        domandaScelta = Random.Range(0, numeroDomande);

        // Se multipla si ha lo schema : 1 domanda, 4 risposte 
        if (multipla) {
            QuestionText.text = fileContent[domandaScelta + 1 + (domandaScelta * 4)];
        }
        else{
            QuestionText.text = fileContent[domandaScelta + 1 + (domandaScelta * 1)];     
        }
        return domandaScelta;
    }

    //Legge le risposte delle domanda scelta
    public string[] leggiRisposte(int tipoDomanda ,int domandaScelta, bool multipla) {
        string[] risposte = new string[4];
       string[] fileContent = files[tipoDomanda-1].text.Split("\n"[0]);
        int indicerisposta;
        if (multipla) {            
            indicerisposta = domandaScelta + 1 + (domandaScelta * 4);
            // indice della domanda. le successive quattro "caselle" sono la risposta
            for (int i= 0; i<4; i++) { 
                risposte[i] = fileContent[i + indicerisposta + 1];
            }
        }
        else {
            indicerisposta = domandaScelta + 1 + (domandaScelta * 1);
            // indice della domanda. la successiva  "casella" è la risposta
            risposte[0] = fileContent[indicerisposta + 1];
           
            rispostaAperta =(string) risposte[0];
        }
        return risposte;
     }

    //Sistema le differenze che bisogna apportare nel passaggio da un segnaposto ad un altro
    void CambioTurno ()
    {
        int giocatore = gameController.GetComponent<GameHandler>().turno % gameController.GetComponent<GameHandler>().numeroGiocatori;
        Players[giocatore].GetComponent<PlayerManager>().ShutCamera();
        Players[giocatore].GetComponent<Animation>().Stop();
        gameController.GetComponent<GameHandler>().turno++;
        Players[(giocatore + 1) % gameController.GetComponent<GameHandler>().numeroGiocatori].GetComponent<PlayerManager>().EnableCamera();
        if(gameController.GetComponent<GameHandler>().numeroGiocatori==2)
            AnswerResult.text = "Turno Cambiato! Tocca a te Giocatore " + ((giocatore+1)%gameController.GetComponent<GameHandler>().numeroGiocatori);  
    }
}