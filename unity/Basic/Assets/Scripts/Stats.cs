public static class Stats
{
    private static int score;
    private static float time;

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
}