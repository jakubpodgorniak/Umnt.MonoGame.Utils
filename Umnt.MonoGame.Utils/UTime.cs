using Microsoft.Xna.Framework;

namespace Umnt.MonoGame.Utils;

public static class UTime
{
    private static GameTime lastGameTime = new(new TimeSpan(0), new TimeSpan(0));

    public static GameTime GetLastTime() => lastGameTime;

    public static void Update(GameTime gameTime)
    {
        lastGameTime = gameTime;
    }

    public static double TotalSeconds => lastGameTime.TotalGameTime.TotalSeconds;
}

public class UTimer
{
    private double durationSeconds;
    private double durationSecondsInverse;
    private double startTotalSeconds;
    private double endTotalSeconds;

    public UTimer(double durationSeconds)
    {
        this.durationSeconds = durationSeconds;
        durationSecondsInverse = 1.0 / durationSeconds;
        startTotalSeconds = UTime.TotalSeconds;
        endTotalSeconds = startTotalSeconds + durationSeconds;
    }

    public bool Elapsed => UTime.TotalSeconds > endTotalSeconds;

    public double Progress => Math.Clamp((UTime.TotalSeconds - startTotalSeconds) * durationSecondsInverse, 0.0, 1.0);

    public void Reset()
    {
        startTotalSeconds = UTime.TotalSeconds;
        endTotalSeconds = startTotalSeconds + durationSeconds;
    }

    public void SetDuration(double newDurationSeconds)
    {
        durationSeconds = newDurationSeconds;
        durationSecondsInverse = 1.0 / newDurationSeconds;
        endTotalSeconds = startTotalSeconds + newDurationSeconds;
    }
}
