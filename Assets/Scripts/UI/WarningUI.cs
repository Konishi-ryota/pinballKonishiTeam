using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    [Header("消えるまでの時間")] 
    [SerializeField] private float _hideTime = 1f;
    [Header("フェード時間")] 
    [SerializeField] private float _fadeTime = 2f;
    [Header("ワーニングテキスト")] 
    [SerializeField] private Text _warningText;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ワーニングUIの表示
    /// </summary>
    /// <param name="text">表示したい文字列</param>
    public void ShowWarningUI(string text)
    {
        gameObject.SetActive(true);
        _warningText.text = text;
        _canvasGroup.alpha = 1;
        StartCoroutine(HideAfterDelay());
    }
    
    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(_hideTime);
        float elapsedTime = 0f;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = 1 - (elapsedTime / _fadeTime);
            yield return null;
        }

        _canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
