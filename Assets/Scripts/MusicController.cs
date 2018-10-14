using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour {

    // Vengono inseriti solo quelli che servono nel caso specifico (tranne jazz e Classic in Game)
    public Transform[] SplashScreen;
    public Transform [] JazzMusics;
    public Transform[] ClassicMusics;
    public Transform[] Valuation;

    private Object mManager;
    private GameObject music;
    private int index;
    // Per Inserire la musica.
    void Start () {
        GameObject oldMusic;
        index = PlayerPrefs.GetInt("indiceMusica");
        if ((SceneManager.GetActiveScene()).name == "SplashScreen")  {
            if (!GameObject.FindGameObjectWithTag("SplashScreenMusic")) {
                index++;
                mManager = Instantiate(SplashScreen[index%SplashScreen.Length], transform.position, Quaternion.identity);
                mManager.name = SplashScreen[index%SplashScreen.Length].name;
                Transform temp = (Transform)mManager;
                if(PlayerPrefs.GetInt("primaVolta")==0)
                {
                    PlayerPrefs.SetFloat("volume",1);
                    PlayerPrefs.SetInt("primaVolta", 1);
                }
                temp.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                PlayerPrefs.SetInt("indiceMusica", index);
                DontDestroyOnLoad(mManager);

                //Controlla se ci sono musiche di altre parti
                if (oldMusic = GameObject.FindGameObjectWithTag("Jazz"))
                    Destroy(oldMusic.gameObject);
                if(oldMusic = GameObject.FindGameObjectWithTag("Classic"))
                    Destroy(oldMusic.gameObject);
                if (oldMusic = GameObject.FindGameObjectWithTag("ValMusic"))
                    Destroy(oldMusic.gameObject);
             }
        }

        if ((SceneManager.GetActiveScene()).name == "Game")
        {
            if (PlayerPrefs.GetInt("tipoMusica") == 0)
            {
                if (!GameObject.FindGameObjectWithTag("Jazz"))
                {
                    //Per ora solo musica Jazz, dopo aver fatto le opzioni se ne parla
                    index++;
                    mManager = Instantiate(JazzMusics[index % JazzMusics.Length], transform.position, Quaternion.identity);
                    mManager.name = JazzMusics[index % JazzMusics.Length].name;
                    DontDestroyOnLoad(mManager);
                    Transform temp = (Transform)mManager;
                    temp.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                    PlayerPrefs.SetInt("indiceMusica", index);

                    //Controlla se ci sono musiche di altre parti
                    if (oldMusic = GameObject.FindGameObjectWithTag("SplashScreenMusic"))
                        Destroy(oldMusic.gameObject);
                    if (oldMusic = GameObject.FindGameObjectWithTag("Classic"))
                        Destroy(oldMusic.gameObject);
                    if (oldMusic = GameObject.FindGameObjectWithTag("ValMusic"))
                        Destroy(oldMusic.gameObject);

                }


            }
            if (PlayerPrefs.GetInt("tipoMusica") == 1)
            {
                if (!GameObject.FindGameObjectWithTag("Classic"))
                {
                    //Per ora solo musica Jazz, dopo aver fatto le opzioni se ne parla
                    index++;
                    mManager = Instantiate(ClassicMusics[index % ClassicMusics.Length], transform.position, Quaternion.identity);
                    mManager.name = ClassicMusics[index % ClassicMusics.Length].name;
                    DontDestroyOnLoad(mManager);
                    Transform temp = (Transform)mManager;
                    temp.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                    PlayerPrefs.SetInt("indiceMusica", index);

                    //Controlla se ci sono musiche di altre parti
                    if (oldMusic = GameObject.FindGameObjectWithTag("SplashScreenMusic"))
                        Destroy(oldMusic.gameObject);
                    if (oldMusic = GameObject.FindGameObjectWithTag("Jazz"))
                        Destroy(oldMusic.gameObject);
                    if (oldMusic = GameObject.FindGameObjectWithTag("ValMusic"))
                        Destroy(oldMusic.gameObject);

                }
            }
        }
        if ((SceneManager.GetActiveScene()).name == "Valuation")
        {
            if (!GameObject.FindGameObjectWithTag("ValMusic"))
            {
                index++;
                mManager = Instantiate(Valuation[index % Valuation.Length], transform.position, Quaternion.identity);
                mManager.name = Valuation[index % Valuation.Length].name;
                DontDestroyOnLoad(mManager);
                Transform temp = (Transform)mManager;
                temp.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                PlayerPrefs.SetInt("indiceMusica", index);

                //Controlla se ci sono musiche di altre parti
                if (oldMusic = GameObject.FindGameObjectWithTag("Jazz"))
                    Destroy(oldMusic.gameObject);
                if (oldMusic = GameObject.FindGameObjectWithTag("Classic"))
                    Destroy(oldMusic.gameObject);
                if (oldMusic = GameObject.FindGameObjectWithTag("SplashScreenMusic"))
                    Destroy(oldMusic.gameObject);
            }        
        }
    }

   
    public void FixedUpdate()
    {
       
        if (music = GameObject.FindGameObjectWithTag("Jazz")) {
            if (music == null||JazzMusics.Length==0)
                return;
            if (!music.GetComponent<AudioSource>().isPlaying) {
                GameObject.Destroy(music);
                index++;
                mManager = Instantiate(JazzMusics[index % JazzMusics.Length], transform.position, Quaternion.identity);
                mManager.name = JazzMusics[index % JazzMusics.Length].name;
                DontDestroyOnLoad(mManager);
                Transform temp = (Transform)mManager;
                temp.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");

                PlayerPrefs.SetInt("indiceMusica", index);
            }
        }
        
        if (music = GameObject.FindGameObjectWithTag("Classic")) {
            if (music == null||ClassicMusics.Length==0)
                return;
            if (!music.GetComponent<AudioSource>().isPlaying) {
                GameObject.Destroy(music);
                index++;
                mManager = Instantiate(ClassicMusics[index % ClassicMusics.Length], transform.position, Quaternion.identity);
                mManager.name = ClassicMusics[index % ClassicMusics.Length].name;
                DontDestroyOnLoad(mManager);
                Transform temp = (Transform)mManager;
                temp.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                PlayerPrefs.SetInt("indiceMusica", index);
            }
        }
        
        if (music = GameObject.FindGameObjectWithTag("SplashScreenMusic")) {
            if (music == null||SplashScreen.Length==0)
                return;
            if (!music.GetComponent<AudioSource>().isPlaying)
            {
                GameObject.Destroy(music);
                index++;
                mManager = Instantiate(SplashScreen[index % SplashScreen.Length], transform.position, Quaternion.identity);
                mManager.name = SplashScreen[index % SplashScreen.Length].name;
                
                Transform temp = (Transform)mManager;
                temp.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                DontDestroyOnLoad(mManager);
                PlayerPrefs.SetInt("indiceMusica", index);
            }
        }

        if (music = GameObject.FindGameObjectWithTag("ValMusic"))
        {
            if (music == null||Valuation.Length==0)
                return;
            if (!music.GetComponent<AudioSource>().isPlaying) {
                GameObject.Destroy(music);
                index++;
                mManager = Instantiate(Valuation[index % Valuation.Length], transform.position, Quaternion.identity);
                mManager.name = Valuation[index % Valuation.Length].name;
                DontDestroyOnLoad(mManager);
                Transform temp = (Transform)mManager;
                temp.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                PlayerPrefs.SetInt("indiceMusica", index);

            }
        }
    }

}
