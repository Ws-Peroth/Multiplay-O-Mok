using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartUiManager : MonoBehaviour
{
    [SerializeField] private Text startText;

    [SerializeField] private float fadeTime = 1f;
    
    // Start is called before the first frame update
    private void Start()
    {
        startText.DOFade(0f, fadeTime)
                 .SetLoops(-1, LoopType.Yoyo);
    }

    public void StartButtonOnClick()
    {
        startText.DOKill();
        SceneManagerEx.SceneChange(SceneTypes.Lobby);
    }
}
