using UnityEngine;

public class FullscreenSprite : MonoBehaviour
{
    [SerializeField]
    bool scaleContainer = true;
    float _aspect;
    Vector3 _originalSpriteSize;
    Vector3 _originalLocalScale;
    SpriteRenderer _spriteRenderer;
    Vector3 _currentScale = Vector3.zero;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalSpriteSize = _spriteRenderer.sprite.bounds.size;
        _originalLocalScale = transform.localScale;
        SetBackgroundScale();
    }

    void Update()
    {
        if (System.Math.Abs(_aspect - Camera.main.aspect) > 0.01)
        {
            SetBackgroundScale();
        }
    }

    void SetBackgroundScale()
    {
        LogManager.Log("Resising background");
        _aspect = Camera.main.aspect;

        var cameraHeight = Camera.main.orthographicSize * 2;
        var cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        var spriteSize = _originalSpriteSize;

        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            _currentScale = SetLandscapeScale(cameraSize, spriteSize, _currentScale, _originalLocalScale);
            if ((_spriteRenderer.sprite.bounds.size.y * _currentScale.y) < cameraHeight)
            {
                _currentScale = SetPortraitScale(cameraSize, spriteSize, _currentScale, _originalLocalScale);
            }
        }
        else
        { // Portrait
            _currentScale = SetPortraitScale(cameraSize, spriteSize, _currentScale, _originalLocalScale);
        }

        if (scaleContainer)
        {
            transform.localScale = _currentScale;
            //align image with its base on the cameras botton
            var diff = (_spriteRenderer.sprite.bounds.size.y * _currentScale.y) - cameraHeight;
            transform.position = new Vector3(0, diff / 2, 1);
        }
        else
        {
            _spriteRenderer.transform.localScale = _currentScale;
        }
    }

    static Vector3 SetLandscapeScale(Vector2 cameraSize, Vector3 spriteSize, Vector3 scale, Vector3 originalScale)
    {
        scale = originalScale;
        scale *= cameraSize.x / spriteSize.x;
        return scale;
    }

    static Vector3 SetPortraitScale(Vector2 cameraSize, Vector3 spriteSize, Vector3 scale, Vector3 originalScale)
    {
        scale = originalScale;
        scale *= cameraSize.y / spriteSize.y;
        return scale;
    }
}
