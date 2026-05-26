using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    public GameObject letterPrefab;    // 출력할 Letter 프리팹
    public RectTransform targetUi;
    public Camera letterCamera;        // 캡처할 UI 전용 카메라

    public void Capture()
    {
        StartCoroutine(CaptureArea(texture =>
        {
            // 캡처된 Texture2D를 바로 UI에 적용
            ApplyToUI(texture);
        }));
    }

    private IEnumerator CaptureArea(System.Action<Texture2D> callback)
    {
        yield return new WaitForEndOfFrame();

        if (GameManager.Instance.giftReinitializer.originalGift.GetComponentInChildren<LetterCheckingScript>())
        {
            Destroy(GameManager.Instance.giftReinitializer.originalGift.GetComponentInChildren<LetterCheckingScript>().gameObject);    
        }
        
        
        

        // targetUI의 월드 좌표를 화면 좌표로 변환
        Vector3[] corners = new Vector3[4];
        targetUi.GetWorldCorners(corners);

        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(letterCamera, corners[0]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(letterCamera, corners[2]);

        // Rect 정의 (화면 좌표 기준)
        Rect captureRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y
        );

        // RenderTexture 생성
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        letterCamera.targetTexture = rt;

        // 카메라 렌더링
        letterCamera.Render();

        // Texture2D로 targetUI 영역만 읽기
        RenderTexture.active = rt;
        Texture2D texture = new Texture2D((int)captureRect.width, (int)captureRect.height, TextureFormat.RGBA32, false);
        texture.ReadPixels(captureRect, 0, 0);
        texture.Apply();

        // RenderTexture 초기화
        letterCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // 콜백을 통해 Texture2D 반환
        callback?.Invoke(texture);
    }

    private void ApplyToUI(Texture2D texture)
    {
        // Letter 프리팹 생성 및 적용
        GameObject newLetter = Instantiate(letterPrefab, transform.position, Quaternion.identity);
        newLetter.name = "LetterPrefab";
        Transform letterPos = GameObject.Find("LetterPos").transform;

        newLetter.transform.position = letterPos.position;
        newLetter.transform.SetParent(letterPos);
        newLetter.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);

        Image imageComponent = newLetter.GetComponent<Image>();
        if (imageComponent != null)
        {
            // Texture2D를 Sprite로 변환하여 적용
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imageComponent.sprite = sprite;
        }
        
        GameManager.Instance.DisableLetterUi();
    }
}