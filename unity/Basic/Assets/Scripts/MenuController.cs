using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Image logo;
    public Image loadingLogo;
    public Text loadingText;
    public GameObject buttons;
    bool loading = false;
    int opacity = 0;
    float time = 0;
    int delta = -1;

	// Use this for initialization
	void Start () {
        loading = false;
        buttons.SetActive(true);
        loadingText.gameObject.SetActive(false);
        loadingLogo.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if (loading)
        {
            if (Time.time - time > 0.001)
            {
                opacity = (opacity + (2 * delta));
                if (opacity > 254) delta = -1;
                else if (opacity < 1) delta = 1;
                loadingLogo.color = new Color(255f, 255f, 255f, ((float)opacity)/255);
                time = Time.time;
            }
        }
    }

    public void LaunchGame()
    {
        if (!loading)
        {
            Stats.Menu = true;
            loading = true;
            buttons.SetActive(false);
            loadingText.gameObject.SetActive(true);
            loadingLogo.gameObject.SetActive(true);
            Cursor.visible = false;
            time = Time.time;
            StartCoroutine(LoadSceneAsync());
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Dev_James", LoadSceneMode.Single);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
