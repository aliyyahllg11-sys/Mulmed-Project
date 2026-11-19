using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

// Input keyboard yang jalan di Input Manager lama maupun Input System baru.
// Kiri/Kanan: panah atau A/D. Lompat: Spasi, panah atas, atau W. Esc: kembali ke menu.
public static class GameInput
{
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
    public static float Horizontal
    {
        get
        {
            var k = Keyboard.current; if (k == null) return 0f;
            float h = 0f;
            if (k.leftArrowKey.isPressed || k.aKey.isPressed) h -= 1f;
            if (k.rightArrowKey.isPressed || k.dKey.isPressed) h += 1f;
            return h;
        }
    }
    public static bool JumpDown { get { var k = Keyboard.current; return k != null && (k.spaceKey.wasPressedThisFrame || k.upArrowKey.wasPressedThisFrame || k.wKey.wasPressedThisFrame); } }
    public static bool JumpHeld { get { var k = Keyboard.current; return k != null && (k.spaceKey.isPressed || k.upArrowKey.isPressed || k.wKey.isPressed); } }
    public static bool EscapeDown { get { var k = Keyboard.current; return k != null && k.escapeKey.wasPressedThisFrame; } }
    public static bool NumberDown(int n)
    {
        var k = Keyboard.current; if (k == null) return false;
        return n == 1 ? k.digit1Key.wasPressedThisFrame : n == 2 ? k.digit2Key.wasPressedThisFrame : k.digit3Key.wasPressedThisFrame;
    }
#else
    public static float Horizontal
    {
        get
        {
            float h = 0f;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) h -= 1f;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) h += 1f;
            return h;
        }
    }
    public static bool JumpDown { get { return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W); } }
    public static bool JumpHeld { get { return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W); } }
    public static bool EscapeDown { get { return Input.GetKeyDown(KeyCode.Escape); } }
    public static bool NumberDown(int n) { return Input.GetKeyDown(KeyCode.Alpha1 + (n - 1)); }
#endif
}
