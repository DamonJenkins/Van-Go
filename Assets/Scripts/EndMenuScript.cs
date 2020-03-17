using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndMenuScript : MonoBehaviour
{

    [SerializeField]
    Image fadePanel;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Play(fadePanel.DOFade(0.0f, 0.5f).SetEase(Ease.InOutCubic));

        Debug.Log("TODO: Add score value changing");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMainMenu()
    {
        DOTween.Play(DOTween.Sequence().Join(fadePanel.DOFade(1.0f, 0.5f).SetEase(Ease.InOutCubic)).AppendCallback(() => { SceneManager.LoadScene("MainMenu"); }));
    }

}
