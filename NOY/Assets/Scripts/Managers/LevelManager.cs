using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;

    private float _target = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneFromButton(string sceneName)
    {
        LoadSceneAsync(sceneName);
    }

    public async void LoadSceneAsync(string sceneName)
    {
        _target = 0f;
        if (_progressBar != null) _progressBar.fillAmount = 0f;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        if (_loaderCanvas != null) _loaderCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(1000); // Wait before final activation
        _target = 1f; // Smoothly fill to 100%

        await Task.Delay(500); // Optional short delay before switching
       scene.allowSceneActivation = true;

        // Wait one more frame before disabling loader
        await Task.Yield();
        _loaderCanvas.SetActive(false);

    }

    void Update()
    {
        if (_progressBar != null)
        {
            _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
        }
    }
}
