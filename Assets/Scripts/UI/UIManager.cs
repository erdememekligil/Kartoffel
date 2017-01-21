using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject creditsPanels;

    [SerializeField]
    private InputField roomField;

    [SerializeField]
    private Text yearText;
    [SerializeField]
    private float tweenDuration = 1;


    [SerializeField]
    private int yearTest;

    private int year;
    private string yearString = "Year: ";

    void Update()
    {
        yearText.text = yearString + year.ToString();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetDate(year + yearTest);
        }
    } 

    public void SetDate(int date)
    {
        DOTween.To(() => year, x => year = x, date, tweenDuration);
    }

    public void ShowLobby()
    {
        lobbyPanel.SetActive(true);

    }
    public void HideLobby()
    {
        lobbyPanel.SetActive(false);

    }

    public void BeginGame(int Direction)
    {
        lobbyPanel.SetActive(false);
        menuPanel.SetActive(false);

    }

    public void ShowCredits()
    {
        creditsPanels.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanels.SetActive(false);
    }

}
