public static class Stats
{
    private static int score;
    private static float time;
    private static bool menu = false;

    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }

    public static float Time
    {
        get
        {
            return time;
        }
        set
        {
            time = value;
        }
    }

    public static bool Menu
    {
        get
        {
            return menu;
        }
        set
        {
            menu = value;
        }
    }
}