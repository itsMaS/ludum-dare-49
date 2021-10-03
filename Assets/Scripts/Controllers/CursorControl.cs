using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorControl : MonoBehaviour
{
    [SerializeField] Image CursorImage;

    [SerializeField] float transitionDuration = 0.5f;
    void Start()
    {
        Cursor.visible = false;
        CursorImage.DOFade(0, 0);
    }

    Tween tween;
    void Update()
    {
        CursorImage.transform.position = Vector2.Lerp(CursorImage.transform.position, Input.mousePosition, 20 * Time.deltaTime); 

        if(Input.GetMouseButtonDown(0))
        {
            tween?.Kill();
            tween = CursorImage.DOFade(0.5f, transitionDuration);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            tween?.Kill();
            tween = CursorImage.DOFade(0, transitionDuration);
        }

        Cursor.visible = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.visible = false;
    }
}
