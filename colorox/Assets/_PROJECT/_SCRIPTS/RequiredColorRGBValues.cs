using UnityEngine;

public static class RequiredColorRGBValues
{
    // redColor = { 1f, 0.3f, 0.3f };
    // yellowColor = { 1f, 1f, 0.4f };
    // blueColor = { 0f, 0.66f, 1f };
    // violetColor = { 0.8f, 0f, 1f };
    // greenColor = { 0f, 1f, 0.44f };
    // orangeColor = { 1f, 0.6f, 0.2f };

    public static Color redColor = new Color(1f, 0.3f, 0.3f, 1f);
    public static Color yellowColor = new Color(1f, 1f, 0.4f, 1f);
    public static Color blueColor = new Color(0f, 0.66f, 1f, 1f);
    public static Color orangeColor = new Color(1f, 0.6f, 0.2f, 1f);
    public static Color greenColor = new Color(0f, 1f, 0.44f, 1f);
    public static Color violetColor = new Color(0.8f, 0f, 1f, 1f);

    public static Color GetColorFromString (string colorString)
    {
        Color fillColor = new Color();

        if(colorString == "a")
        {
            fillColor = redColor;
        }
        if (colorString == "b")
        {
            fillColor = yellowColor;
        }
        if (colorString == "c")
        {
            fillColor = blueColor;
        }
        if (colorString == "ab")
        {
            fillColor = orangeColor;
        }
        if (colorString == "bc")
        {
            fillColor = greenColor;
        }
        if (colorString == "ca")
        {
            fillColor = violetColor;
        }
        if(colorString == "")
        {
            fillColor.a = 1;
        }

        return fillColor;
    }

    public static Color GetColorFromCode(COLORCODE colorcode)
    {
        Color fillColor = new Color();

        if (colorcode == COLORCODE.RED)
        {
            fillColor = redColor;
        }
        if (colorcode == COLORCODE.YELLOW)
        {
            fillColor = yellowColor;
        }
        if (colorcode == COLORCODE.BLUE)
        {
            fillColor = blueColor;
        }
        if (colorcode == COLORCODE.ORANGE)
        {
            fillColor = orangeColor;
        }
        if (colorcode == COLORCODE.GREEN)
        {
            fillColor = greenColor;
        }
        if (colorcode == COLORCODE.VIOLET)
        {
            fillColor = violetColor;
        }
        if (colorcode == COLORCODE.CHANGABLE)
        {
            fillColor.a = 1;
        }

        return fillColor;
    }
}
