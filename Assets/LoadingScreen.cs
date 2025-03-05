using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text loadingLabel; // Gán TMP_Text trong Inspector

    private void Start()
    {
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {
        // Bắt đầu load Level1 bất đồng bộ
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
        // Đảm bảo cho phép tự động chuyển scene khi load xong
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            // Giá trị progress đạt tối đa 0.9 trước khi scene tự kích hoạt, nên chia cho 0.9 để tính phần trăm
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingLabel.text = "Loading... " + (progress * 100).ToString("F0") + "%";

            // Nếu progress đạt 100% (thực tế là asyncLoad.progress >= 0.9f), có thể thêm chút delay nếu cần
            if (asyncLoad.progress >= 0.9f)
            {
                // Optional: chờ thêm chút để hiển thị 100% thật rõ trước khi chuyển
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }
}
