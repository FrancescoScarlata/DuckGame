using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Classe per il controllo del grafico delle sezioni "Grafici 1 e 2".
public class Graphic_Controller : MonoBehaviour {
    
    // Texture da scegliere
    public Texture [] graphics;
    
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Prende l'immagine della domanda specificata
    public void pickAGraphic(int domandaScelta)
    {
        /*
        int graphNumber = Random.Range(0, graphics.Length - 1);
        Debug.Log("Ha preso l'immagine " + graphNumber);
        */
        GetComponent<RawImage>().texture = (Texture)graphics[domandaScelta];
       // return graphNumber;
    }

}
