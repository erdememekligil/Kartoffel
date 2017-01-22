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
	private GameObject jumpButton;
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

	void Start(){
		jumpButton.GetComponent<Button> ().onClick.AddListener(InputManager.Instance.Dojump);
	}

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
		// N =0 , E = 1, S = 2 , W = 3
		int room;
		ServerManager.Instance.RoomId = int.TryParse(roomField.text, out room) ? int.Parse(roomField.text): 1;
		string directionString = "";
		switch (Direction) {
		case 0:
			directionString = "n";
			break;
		case 1:
			directionString = "w";
			break;
		case 2:
			directionString = "s";
			break;
		case 3:
			directionString = "e";
			break;
		default:
			directionString = "n";
			break;
		}
		ServerManager.Instance.RegionType = directionString;
		ServerManager.Instance.SetReady();
        lobbyPanel.SetActive(false);
        menuPanel.SetActive(false);
		jumpButton.SetActive (true);
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
