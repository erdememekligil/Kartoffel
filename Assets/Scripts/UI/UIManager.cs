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
	private Text yearText,EraText;
    [SerializeField]
    private float tweenDuration = 1;

	private long year;
    private string yearString = "Year: ";

	void Start(){
		jumpButton.GetComponent<Button> ().onClick.AddListener(InputManager.Instance.Dojump);
	}

	// Use this for initialization
	void OnEnable () {
		ServerManager.Instance.OnServerFrame += OnServerFrame;
	}

	void OnDisable () {
		ServerManager.Instance.OnServerFrame -= OnServerFrame;
	}

    void Update()
    {
        yearText.text = yearString + year.ToString();
    } 

	public void SetDate(long date)
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

	public void BeginGame(string direction)
    {
		int room;
		ServerManager.Instance.RoomId = int.TryParse(roomField.text, out room) ? int.Parse(roomField.text): 1;

		ServerManager.Instance.RegionType = direction;
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

	private void OnServerFrame(object sender, ServerFrame serverFrame) 
	{
		EraText.text = LevelManager.Instance.GetEraName ();
		SetDate (LevelManager.Instance.Year);
	}


}
