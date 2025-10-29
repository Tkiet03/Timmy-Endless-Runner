using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControll : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;
    [SerializeField] GameObject bounceText;
    [SerializeField] GameObject bigButton;
    [SerializeField] GameObject animCam;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject menuControls;
    [SerializeField] AudioSource buttonSelect;
    public static bool hasCliked;
    [SerializeField] GameObject staticCam;
    [SerializeField] GameObject fadeIn;

    void Start()
    {
        StartCoroutine(fadeInTurnOff());
        if (hasCliked == true)
        {
            staticCam.SetActive(true);
            animCam.SetActive(false);
            menuControls.SetActive(true);
            bounceText.SetActive(false);
            bigButton.SetActive(false);
            
        }
    }

   
    void Update()
    {
        
    }
    public void MenuBeginButton()
    {
        StartCoroutine(AnimCam());
    }
    public void StartGame()
    {
        StartCoroutine(StartButton());
    }
    IEnumerator StartButton()
    {
        buttonSelect.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
    IEnumerator AnimCam()
    {
        animCam.GetComponent<Animator>().Play("AimMenuCam");
        bounceText.SetActive(false);
        bigButton.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        fadeIn.SetActive(false);
        mainCam.SetActive(true);
        animCam.SetActive(false);
        menuControls.SetActive(true);
        hasCliked = true;
    }

    IEnumerator fadeInTurnOff()
    {
        yield return new WaitForSeconds(1);
        fadeIn.SetActive(false);
    }
}
