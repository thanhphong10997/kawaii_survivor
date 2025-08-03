using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MobileJoystick : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;

    [Header(" Settings ")]
    [SerializeField] private float moveFactor;

    private PlayerControls playerControls;

    private Vector2 currentMousePosition;
    private Vector2 clickedPosition;
    private Vector2 move;
    private bool canControl;

    private void Awake()
    {
        // Khởi tạo PlayerControls một lần duy nhất
        playerControls = new PlayerControls();

        // ĐĂNG KÝ CALLBACK CHO LEFT CLICK (MOUSE DOWN)
        // Khi nút chuột trái được nhấn xuống, hàm OnLeftMouseButtonDown sẽ được gọi
        // .performed cho button action xảy ra khi nút được nhấn xuống (giống GetMouseButtonDown)
        // playerControls.Controller.LeftClick.performed += context => OnLeftMouseButtonDown(context);

        playerControls.Controller.LeftClick.canceled += context => OnLeftMouseButtonUp(context);

        // ĐĂNG KÝ CALLBACK CHO VỊ TRÍ CHUỘT
        // Action MousePosition sẽ liên tục gửi giá trị khi chuột di chuyển.
        // Callback này nên được đăng ký một lần duy nhất.
        playerControls.Controller.MousePosition.performed += ReadMousePosition;
        // Nếu bạn cần mousePosition liên tục, bạn cũng có thể đọc nó trong Update()
        // nhưng cách dùng callback này hiệu quả hơn nếu bạn chỉ cần nó khi có thay đổi.
    }

    // Start is called before the first frame update
    void Start()
    {
        HideJoystick();
    }

    private void OnEnable()
    {
        // RẤT QUAN TRỌNG: KÍCH HOẠT CÁC ACTION MAP KHI COMPONENT ĐƯỢC BẬT
        // Debug.Log("OnEnable: Kích hoạt Input Actions.");
        playerControls.Controller.Enable();
        // Đảm bảo Action Map "Controller" của bạn đang được bật
    }

    private void OnDisable()
    {
        HideJoystick();
        // RẤT QUAN TRỌNG: VÔ HIỆU HÓA CÁC ACTION MAP KHI COMPONENT BỊ TẮT HOẶC HỦY
        playerControls.Controller.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (canControl)
            ControlJoystick();
    }

    public void ClickedOnJoystickZoneCallback()
    {

        // clickedPosition = Input.mousePosition;
        Debug.Log(currentMousePosition);
        clickedPosition = currentMousePosition;
        // Đảm bảo joystickOutline đã được gán trong Inspector
        if (joystickOutline != null)
        {
            // Nếu joystickOutline là UI element, bạn cần chuyển đổi vị trí từ Screen Space sang Canvas Space
            // hoặc đơn giản gán trực tiếp nếu Canvas của bạn được cấu hình Screen Space - Overlay
            joystickOutline.position = clickedPosition;
            Debug.Log("Joystick Outline moved to: " + joystickOutline.position);
        }
        else
        {
            Debug.LogWarning("joystickOutline không được gán trong Inspector!");
        }

        ShowJoystick();
    }

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        canControl = true;
    }

    private void HideJoystick()
    {
        // ẩn object joystick
        joystickOutline.gameObject.SetActive(false);
        canControl = false;

        move = Vector2.zero;
    }

    private void ControlJoystick()
    {
        // Vector3 currentPosition = Input.mousePosition;
        // Vector3 direction = currentPosition - clickedPosition;
        Vector2 direction = currentMousePosition - clickedPosition;

        float canvasScale = GetComponentInParent<Canvas>().GetComponent<RectTransform>().localScale.x;

        float moveMagnitude = direction.magnitude * moveFactor * canvasScale;

        float absoluteWidth = joystickOutline.rect.width / 2;
        float realWidth = absoluteWidth * canvasScale;

        moveMagnitude = Mathf.Min(moveMagnitude, realWidth);

        move = direction.normalized * moveMagnitude;

        Vector2 targetPosition = clickedPosition + move;

        joystickKnob.position = targetPosition;

        // if (Input.GetMouseButtonUp(0))
        //     HideJoystick();
    }

    public Vector2 GetMoveVector()
    {
        float canvasScale = GetComponentInParent<Canvas>().GetComponent<RectTransform>().localScale.x;
        return move / canvasScale;
    }

    private void ReadMousePosition(InputAction.CallbackContext context)
    {
        // Đọc giá trị Vector2 của vị trí chuột
        currentMousePosition = context.ReadValue<Vector2>();
        // Debug.Log("Vị trí chuột hiện tại (từ Input System): " + currentMousePosition);

        // Bạn có thể sử dụng 'currentMousePosition' này để di chuyển UI, xoay camera, v.v.
        // Ví dụ: Di chuyển một đối tượng UI theo vị trí chuột
        // transform.position = currentMousePosition; // Cần chuyển đổi từ screen space sang world space nếu cần
    }

    private void OnLeftMouseButtonUp(InputAction.CallbackContext context)
    {
        // context.ReadValueAsButton() sẽ trả về false khi nút được nhả
        // context.action.WasReleasedThisFrame() cũng có thể dùng
        // Debug.Log("Nút chuột trái đã được nhả ra (Mouse Up)!");
        HideJoystick();
        // Thực hiện hành động của bạn ở đây, ví dụ:
        // Raycast để chọn đối tượng, kết thúc kéo thả, v.v.
    }

    private void OnLeftMouseButtonDown(InputAction.CallbackContext context)
    {
        // context.ReadValueAsButton() sẽ trả về false khi nút được nhả
        // context.action.WasReleasedThisFrame() cũng có thể dùng
        Debug.Log("Nút chuột trái đã được ấn (Mouse Down)!");
        // Thực hiện hành động của bạn ở đây, ví dụ:
        // Raycast để chọn đối tượng, kết thúc kéo thả, v.v.
    }
}
