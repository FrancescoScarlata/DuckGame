using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {

    public Slider volume;
    public Text conferma;
    private GameObject music;
    private float time;
    bool needTodisappear = false;

	// Use this for initialization
	void Start () {
        volume.value = PlayerPrefs.GetFloat("volume");
	}
	
	// Update is called once per frame
	void Update () {
        if (time > 3 && needTodisappear) {
            conferma.gameObject.SetActive(false);
            needTodisappear = false;
        }
        else
           time += Time.deltaTime;
      }


    // Metodo della scelta delle opzioni (0 jazz, 1 classica)
    public void ChooseTypeOfMusic(int tipo)
    {
        PlayerPrefs.SetInt("tipoMusica", tipo);
       
        switch (tipo) {
            case 0:
                conferma.text = "Verrà riprodotta musica Jazz durante il gioco.";
                conferma.gameObject.SetActive(true);
                time = 0;
                needTodisappear = true;
                break;
            case 1:
                conferma.text = "Verrà riprodotta musica Classica durante il gioco.";
                conferma.gameObject.SetActive(true);
                time = 0;
                needTodisappear = true;
                break;
            default:
                conferma.text = "Se mi vedi hai sbagliato qualcosa";
                conferma.gameObject.SetActive(true);
                time = 0;
                needTodisappear = true;
                break;
        }
    }

    public void CambiaVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        if (music = GameObject.FindGameObjectWithTag("SplashScreenMusic"))
            music.GetComponent<AudioSource>().volume = volume;
    }

    //Metodo per tornare indietro al menu
    public void ComeBack()
    {
        SceneManager.LoadScene("SplashScreen");
    }

   

}
