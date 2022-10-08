using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Umnt.MonoGame.Utils;

public enum MouseButton
{
    Left,
    Middle,
    Right,
    Extra1,
    Extra2
}

public static class InputU
{
    private readonly static List<Keys> downKeys = new();
    private readonly static List<Keys> upKeys = new();
    private static List<Keys> pressedKeys = new();

    private readonly static List<MouseButton> downMouseButtons = new();
    private readonly static List<MouseButton> upMouseButtons = new();
    private static List<MouseButton> pressedMouseButtons = new();

    private static int previousScrollWheelValue = 0;
    private static int currentScrollWheelValue = 0;
    private static int instanctScrollWheelValue = 0;

    private const float SmoothScrollWheelAcceleration = 0.02f;
    private const float SmoothScroolWheelDecelerate = 0.95f;
    private const float SmoothScrollWheelValueMax = 1.0f;
    private const float SmoothScrollWheelValueGravity = 0.85f;
    
    private static float smoothScrollWheelValue = 0f;
    private static float smoothScrollWheelVelocity = 0f;

    public static Vector2 MousePosition { get; private set; }

    public static void Update(KeyboardState keyboardState, MouseState mouseState)
    {
        // keyboard
        var currentPressedKeys = keyboardState.GetPressedKeys().ToList();

        upKeys.Clear();
        foreach (var key in pressedKeys)
        {
            if (!currentPressedKeys.Contains(key))
            {
                upKeys.Add(key);
            }
        }

        downKeys.Clear();
        foreach (var key in currentPressedKeys)
        {
            if (!pressedKeys.Contains(key))
            {
                downKeys.Add(key);
            }
        }

        pressedKeys = currentPressedKeys;

        // mouse
        var currentPressedMouseButtons = new List<MouseButton>(5);

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            currentPressedMouseButtons.Add(MouseButton.Left);
        }

        if (mouseState.MiddleButton == ButtonState.Pressed)
        {
            currentPressedMouseButtons.Add(MouseButton.Middle);
        }

        if (mouseState.RightButton == ButtonState.Pressed)
        {
            currentPressedMouseButtons.Add(MouseButton.Right);
        }

        if (mouseState.XButton1 == ButtonState.Pressed)
        {
            currentPressedMouseButtons.Add(MouseButton.Extra1);
        }

        if (mouseState.XButton2 == ButtonState.Pressed)
        {
            currentPressedMouseButtons.Add(MouseButton.Extra2);
        }

        upMouseButtons.Clear();
        foreach (var mouseButton in pressedMouseButtons)
        {
            if (!currentPressedMouseButtons.Contains(mouseButton))
            {
                upMouseButtons.Add(mouseButton);
            }
        }

        downMouseButtons.Clear();
        foreach (var mouseButton in currentPressedMouseButtons)
        {
            if (!pressedMouseButtons.Contains(mouseButton))
            {
                downMouseButtons.Add(mouseButton);
            }
        }

        pressedMouseButtons = currentPressedMouseButtons;

        // scroll wheel
        previousScrollWheelValue = currentScrollWheelValue;
        currentScrollWheelValue = mouseState.ScrollWheelValue;
        instanctScrollWheelValue = Math.Sign(previousScrollWheelValue - currentScrollWheelValue);

        if (instanctScrollWheelValue == 0)
        {
            smoothScrollWheelVelocity *= SmoothScroolWheelDecelerate;

            if (Math.Abs(smoothScrollWheelVelocity) < 1e-4)
            {
                smoothScrollWheelVelocity = 0f;
            }
        }
        else
        {
            smoothScrollWheelVelocity += instanctScrollWheelValue * SmoothScrollWheelAcceleration;
        }

        float _new_smooth_scroll_wheel_value = smoothScrollWheelValue + smoothScrollWheelVelocity;
        if (_new_smooth_scroll_wheel_value > SmoothScrollWheelValueMax)
        {
            _new_smooth_scroll_wheel_value = SmoothScrollWheelValueMax;
        }
        else if (_new_smooth_scroll_wheel_value < -SmoothScrollWheelValueMax)
        {
            _new_smooth_scroll_wheel_value = -SmoothScrollWheelValueMax;
        }
        _new_smooth_scroll_wheel_value *= SmoothScrollWheelValueGravity;

        if (Math.Abs(_new_smooth_scroll_wheel_value) < 1e-4)
        {
            _new_smooth_scroll_wheel_value = 0;
        }

        smoothScrollWheelValue = _new_smooth_scroll_wheel_value;

        // mouse position
        MousePosition = mouseState.Position.ToVector2();
    }

    public static bool IsKeyDown(Keys key)
    {
        return downKeys.Contains(key);
    }

    public static bool IsKeyPressed(Keys key)
    {
        return pressedKeys.Contains(key);
    }

    public static bool IsKeyUp(Keys key)
    {
        return upKeys.Contains(key);
    }

    public static int GetInstantScrollWheelValue()
    {
        return instanctScrollWheelValue;
    }

    public static float GetSmoothScrollWheelValue()
    {
        return smoothScrollWheelValue;
    }

    public static bool IsMouseButtonDown(MouseButton btn)
    {
        return downMouseButtons.Contains(btn);
    }

    public static bool IsMouseButtonPressed(MouseButton btn)
    {
        return pressedMouseButtons.Contains(btn);
    }

    public static bool IsMouseButtonUp(MouseButton btn)
    {
        return upMouseButtons.Contains(btn);
    }
}
