using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Classe che controlla il giocatore
public class PlayerManager : MonoBehaviour {

    public Transform CameraController;
    public float speed = 2f;
    public AnimationClip moving;

    // Renderle private o lasciarle pubbliche?
    public int[] risposteEsatte = new int[7];
    public int[] risposteSbagliate = new int[7];

    // Posizione del personaggio durante il gioco. Da trasportare qui il processo di mossa del pupo e del conto della posizione.
    public int posizione;

    bool Cambiadirezione = false;
    

    CharacterController controller;

    Vector3 MoveDirection = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start() {
        controller = GetComponent<CharacterController>();
    }

    // non funziona al momento. è da rivedere
    void Update() {
        //bisogna usarlo all'interno del "muovi Pupo"

        //  transform.position+= controller.transform.forward* speed *Time.deltaTime;
    }

    // Gestisce il movimento generale del personaggio
    public void MuoviPupo(int spostamento, int length, int giocatore)
    {
        if (spostamento == 0)
            return;
 
        //Movimento
        if (spostamento > 0) {
            if (posizione + 1 < length) {
                sposta(true,giocatore);

                MuoviPupo(spostamento - 1, length,giocatore);
                return;
            }
            else {
                MuoviPupo(-spostamento, length,giocatore);
                return;
            }
        }
        if (spostamento < 0) {
            sposta(false,giocatore);

            MuoviPupo(spostamento + 1, length,giocatore);
            return;
        }
     }

    // Fa gli spostamenti. 
    void sposta(bool avanti,int giocatore)
    {
        if (posizione >= 0 && posizione < 12) {
            
            GetComponent<CharacterController>().transform.position+=((MoveDirection + new Vector3(0, 0, 10)));
            CameraController.transform.position += new Vector3(0, 0, 10);
            
            //  Debug.Log("Avanti");
            if (posizione == 11)
            {
                Cambiadirezione = true;
            }
            posizione++;
            return;
        }

        if (posizione >= 12 && posizione < 23)
        {
            if (Cambiadirezione)
            {
                CameraController.transform.Rotate(new Vector3(0, -90, 0));
                this.transform.Rotate(new Vector3(0, -90, 0));
                switch (giocatore)
                {
                    case 0:
                        if (PlayerPrefs.GetInt("NumPlayersLength") == 1)
                            this.transform.position += new Vector3(-1, 0, +0);
                        else
                            this.transform.position += new Vector3(+2, 0, -4);
                        break;
                    case 1:
                        this.transform.position += new Vector3(-3, 0, +2);
                        break;
                }
                Cambiadirezione = false;
            }
                
            GetComponent<CharacterController>().transform.position += ((MoveDirection + new Vector3(-10, 0, 0)));
            CameraController.transform.position += new Vector3(-10, 0, 0);

            if (posizione == 22)
            {
                Cambiadirezione = true;
                Debug.Log("Cambia direzione");
            }
            posizione++;

            return;
        }
        if (posizione >= 23 && posizione < 32)
        {
            Debug.Log(Cambiadirezione);
            Debug.Log(giocatore);
            if (Cambiadirezione)
            {
                Debug.Log("Entra nella condizione");
                CameraController.transform.Rotate(new Vector3(0, -90, 0));
                this.transform.Rotate(new Vector3(0, -90, 0));
                
                switch (giocatore)
                { 
                    case 0:
                        if (PlayerPrefs.GetInt("NumPlayersLength") == 1)
                            this.transform.position += new Vector3(+1 ,0 , 0);
                        else
                            this.transform.position += new Vector3(+4, 0, +2);
                        break;
                    case 1:
                        this.transform.position += new Vector3(-3, 0, -4);
                        break;
                }
                Cambiadirezione = false;
            }

            GetComponent<CharacterController>().transform.position += ((MoveDirection + new Vector3(0, 0, -10)));
            CameraController.transform.position += new Vector3(0, 0, -10);
            

            if (posizione == 31)
                Cambiadirezione = true;
            posizione++;
            return;
        }
        if (posizione >= 32 && posizione < 40) { //Ultima parte della mappa
            if (Cambiadirezione)
            {
                CameraController.transform.Rotate(new Vector3(0, -90, 0));
                this.transform.Rotate(new Vector3(0, -90, 0));
                Cambiadirezione = false;
                switch (giocatore) {
                    case 0:
                        if (PlayerPrefs.GetInt("NumPlayersLength") == 1)
                            this.transform.position += new Vector3(-1, 0, +1.5f);
                        else
                             this.transform.position += new Vector3(-2, 0, +4);
                        break;
                    case 1:
                        this.transform.position += new Vector3(+4, 0, -2);
                        break;
                }
            }
            if (avanti) {
                GetComponent<CharacterController>().transform.position += ((MoveDirection + new Vector3(10, 0, 0)));
                CameraController.transform.position += new Vector3(10, 0, 0);
                posizione++;
            }
            else
            {
                GetComponent<CharacterController>().transform.position += ((MoveDirection + new Vector3(-10, 0, 0)));
                CameraController.transform.position += new Vector3(-10, 0, 0);
                posizione--;
             }
        }

    }

    // dovrebbe essere il metodo per animare, ma non ho ancora capito che si deve fare con l'animator
    public void Anima()
    {
        GetComponent<Animation>().Play();
    }

    //Aggiunge un +1 alle risposte esatte nel punto indicato dall'indice

    public void addCorrectAnswer(int indice)
    {
        risposteEsatte[indice]++;
    }

    //Aggiunge un +1 alle risposte sbagliate nel punto indicato dall'indice
    public void addWrongAnswer(int indice)
    {
        risposteSbagliate[indice]++;
    }

    //Restituisce l'array di risposte esatte
    public int [] ShowCorrectAnswers()
    {
        return risposteEsatte;
    }

    //Restituisce l'array di risposte sbagliate
    public int[] ShowWrongAnswers()
    {
        return risposteSbagliate;
    }

    // Rende inattiva la camera e il relativo audioListener
    public void ShutCamera()
    {
        CameraController.GetComponentInChildren<Camera>().enabled = false;
        CameraController.GetComponentInChildren<AudioListener>().enabled = false;
    }

    // Rende attiva la camera e il relativo audioListener
    public void EnableCamera()
    {
        CameraController.gameObject.SetActive(true);
        CameraController.GetComponentInChildren<AudioListener>().enabled = true;
        CameraController.GetComponentInChildren<Camera>().enabled = true;
    }

}
