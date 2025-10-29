using System;
using System.Collections.Generic;
using System.Globalization;

namespace ConsultationScheduler;

public class AvailabilityPlanner
{
    public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
    {
        ArgumentNullException.ThrowIfNull(startTimes);
        ArgumentNullException.ThrowIfNull(durations);

        if (consultationTime <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(consultationTime), "Consultation time must be greater than zero.");
        }

        if (beginWorkingTime >= endWorkingTime)
        {
            throw new ArgumentException("Working day start time must be earlier than end time.");
        }

        if (startTimes.Length != durations.Length)
        {
            throw new ArgumentException("The startTimes and durations arrays must be of the same length.");
        }

        var workingStart = beginWorkingTime;
        var workingEnd = endWorkingTime;
        var busyIntervals = BuildBusyIntervals(startTimes, durations, workingStart, workingEnd);

        return BuildAvailableSlots(busyIntervals, workingStart, workingEnd, consultationTime);
    }

    private static List<(TimeSpan start, TimeSpan end)> BuildBusyIntervals(TimeSpan[] startTimes, int[] durations, TimeSpan workingStart, TimeSpan workingEnd)
    {
        var busyIntervals = new List<(TimeSpan start, TimeSpan end)>(startTimes.Length);

        for (var i = 0; i < startTimes.Length; i++)
        {
            if (durations[i] < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(durations), "Duration values must be non-negative.");
            }

            var start = startTimes[i];
            var end = start + TimeSpan.FromMinutes(durations[i]);

            if (end <= workingStart || start >= workingEnd)
            {
                continue;
            }

            start = start < workingStart ? workingStart : start;
            end = end > workingEnd ? workingEnd : end;

            busyIntervals.Add((start, end));
        }

        busyIntervals.Sort((a, b) => a.start.CompareTo(b.start));
        return MergeOverlappingIntervals(busyIntervals);
    }

    private static List<(TimeSpan start, TimeSpan end)> MergeOverlappingIntervals(List<(TimeSpan start, TimeSpan end)> intervals)
    {
        var merged = new List<(TimeSpan start, TimeSpan end)>();

        foreach (var interval in intervals)
        {
            if (merged.Count == 0)
            {
                merged.Add(interval);
                continue;
            }

            var last = merged[^1];
            if (interval.start <= last.end)
            {
                var extended = (last.start, interval.end > last.end ? interval.end : last.end);
                merged[^1] = extended;
            }
            else
            {
                merged.Add(interval);
            }
        }

        return merged;
    }

    private static string[] BuildAvailableSlots(List<(TimeSpan start, TimeSpan end)> busyIntervals, TimeSpan workingStart, TimeSpan workingEnd, int consultationTime)
    {
        var slots = new List<string>();
        var slotLength = TimeSpan.FromMinutes(consultationTime);
        var current = workingStart;

        foreach (var interval in busyIntervals)
        {
            AddSlots(slots, current, interval.start, slotLength);
            current = interval.end > current ? interval.end : current;
        }

        AddSlots(slots, current, workingEnd, slotLength);

        return slots.ToArray();
    }

    private static void AddSlots(ICollection<string> slots, TimeSpan freeStart, TimeSpan freeEnd, TimeSpan slotLength)
    {
        if (freeEnd <= freeStart)
        {
            return;
        }

        var start = freeStart;
        while (start + slotLength <= freeEnd)
        {
            var end = start + slotLength;
            slots.Add(string.Format(CultureInfo.InvariantCulture, "{0:HH\\:mm}-{1:HH\\:mm}", start, end));
            start = end;
        }
    }
}
