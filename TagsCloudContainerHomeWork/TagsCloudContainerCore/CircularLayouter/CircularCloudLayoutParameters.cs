using System;

namespace TagsCloudContainerCore.CircularLayouter;

public class CircularCloudLayoutParameters
{
    private const float Pi2 = MathF.PI * 2;
    private const int MaxRadius = 10000;


    private readonly float minAngle;
    private readonly int step;

    public CircularCloudLayoutParameters(int countBoostsForChangeAngle = 5,
        float startAngle = MathF.PI / 2,
        int step = 10,
        float minAngle = MathF.PI / 180 * 5)
    {
        StepAngle = this.startAngle = startAngle;
        this.countBoostsForChangeAngle = countBoostsForChangeAngle;
        this.step = step;
        Radius = this.step;
        this.minAngle = minAngle;
    }

    public CircularCloudLayoutParameters(LayoutSettings settings)
    {
        minAngle = settings.MinAngle;
        step = settings.Step;
        Radius = step;
        StepAngle = startAngle = MathF.PI / 2;
        countRadiusBoosts = 2;
    }


    private readonly float startAngle;
    private readonly int countBoostsForChangeAngle;
    private int stepCount;

    public int Radius { get; private set; }
    public float StepAngle { get; private set; }
    public float NextAngle => StepAngle * NextStep;
    public float Angle => stepCount * StepAngle;

    private int NextStep => ++stepCount;


    private int countRadiusBoosts;

    private int CountRadiusBoosts
    {
        get => ++countRadiusBoosts;
        set => countRadiusBoosts = value;
    }

    public void BoostRadius()
    {
        Radius += step;
        stepCount = 0;

        if (CountRadiusBoosts % countBoostsForChangeAngle == 0 && NextAngle > minAngle)
        {
            StepAngle /= 2;
        }

        if (Radius > MaxRadius)
        {
            throw new Exception("The radius exceeds the maximum allowed value\n" +
                                $"MaxRadius = {MaxRadius} CurrentRadius = {Radius}");
        }
    }

    public void ResetRadius()
    {
        StepAngle = startAngle;
        stepCount = 0;
        CountRadiusBoosts = 0;
        Radius = step;
    }

    public bool IsValidNextAngle() => NextAngle < Pi2;
}