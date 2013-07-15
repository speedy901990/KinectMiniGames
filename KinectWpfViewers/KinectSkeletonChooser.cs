// -----------------------------------------------------------------------
// <copyright file="KinectSkeletonChooser.cs" company="Microsoft IT">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.WpfViewers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Microsoft.Kinect;
    
    public enum SkeletonChooserMode
    {
        /// <summary>
        /// Use system default tracking
        /// </summary>
        DefaultSystemTracking,

        /// <summary>
        /// Track the player nearest to the sensor
        /// </summary>
        Closest1Player,

        /// <summary>
        /// Track the two players nearest to the sensor
        /// </summary>
        Closest2Player,

        /// <summary>
        /// Track one player based on id
        /// </summary>
        Sticky1Player,

        /// <summary>
        /// Track two players based on id
        /// </summary>
        Sticky2Player,

        /// <summary>
        /// Track one player based on most activity
        /// </summary>
        MostActive1Player,

        /// <summary>
        /// Track two players based on most activity
        /// </summary>
        MostActive2Player
    }

    /// <summary>
    /// KinectSkeletonChooser class is a lookless control that will select skeletons based on a specified heuristic.
    /// It contains logic to track players over multiple frames and use this data to select based on distance, activity level or other methods.
    /// It is intended that if you use this control, no other code will manage the skeleton tracking state on the Kinect Sensor,
    /// as they will collide in unpredictable ways.
    /// </summary>
    public class KinectSkeletonChooser : KinectControl
    {
        public static readonly DependencyProperty SkeletonChooserModeProperty =
            DependencyProperty.Register(
                "SkeletonChooserMode",
                typeof(SkeletonChooserMode),
                typeof(KinectSkeletonChooser),
                new PropertyMetadata(SkeletonChooserMode.DefaultSystemTracking, (sender, args) => ((KinectSkeletonChooser)sender).EnsureSkeletonStreamState()));

        private readonly List<ActivityWatcher> recentActivity = new List<ActivityWatcher>();
        private readonly List<int> activeList = new List<int>();
        private Skeleton[] skeletonData;

        public SkeletonChooserMode SkeletonChooserMode
        {
            get { return (SkeletonChooserMode)GetValue(SkeletonChooserModeProperty); }
            set { SetValue(SkeletonChooserModeProperty, value); }
        }

        protected override void OnKinectSensorChanged(object sender, KinectSensorManagerEventArgs<KinectSensor> args)
        {
            if (null == args)
            {
                throw new ArgumentNullException("args");
            }

            var oldKinectSensor = args.OldValue;
            var newKinectSensor = args.NewValue;

            if (oldKinectSensor != null)
            {
                oldKinectSensor.SkeletonFrameReady -= SkeletonFrameReady;
            }

            if (newKinectSensor != null && newKinectSensor.Status == KinectStatus.Connected)
            {
                newKinectSensor.SkeletonFrameReady += SkeletonFrameReady;
            }

            EnsureSkeletonStreamState();
        }

        private void EnsureSkeletonStreamState()
        {
            if ((null != KinectSensorManager) && (null != KinectSensorManager.KinectSensor))
            {
                KinectSensorManager.KinectSensor.SkeletonStream.AppChoosesSkeletons = SkeletonChooserMode.DefaultSystemTracking != SkeletonChooserMode;
            }
        }

        private void SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            bool haveSkeletonData = false;

            // Ensure that this event still corresponds to the current Kinect Sensor (if any)
            if ((null == KinectSensorManager) || (!ReferenceEquals(KinectSensorManager.KinectSensor, sender)))
            {
                return;
            }

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    if ((skeletonData == null) || (skeletonData.Length != skeletonFrame.SkeletonArrayLength))
                    {
                        skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    skeletonFrame.CopySkeletonDataTo(skeletonData);

                    haveSkeletonData = true;
                }
            }

            if (haveSkeletonData)
            {
                ChooseTrackedSkeletons(skeletonData);
            }
        }

        private void ChooseTrackedSkeletons(IEnumerable<Skeleton> skeletonDataValue)
        {
            switch (SkeletonChooserMode)
            {
                case SkeletonChooserMode.Closest1Player:
                    ChooseClosestSkeletons(skeletonDataValue, 1);
                    break;
                case SkeletonChooserMode.Closest2Player:
                    ChooseClosestSkeletons(skeletonDataValue, 2);
                    break;
                case SkeletonChooserMode.Sticky1Player:
                    ChooseOldestSkeletons(skeletonDataValue, 1);
                    break;
                case SkeletonChooserMode.Sticky2Player:
                    ChooseOldestSkeletons(skeletonDataValue, 2);
                    break;
                case SkeletonChooserMode.MostActive1Player:
                    ChooseMostActiveSkeletons(skeletonDataValue, 1);
                    break;
                case SkeletonChooserMode.MostActive2Player:
                    ChooseMostActiveSkeletons(skeletonDataValue, 2);
                    break;
            }
        }

        private void ChooseClosestSkeletons(IEnumerable<Skeleton> skeletonDataValue, int count)
        {
            SortedList<float, int> depthSorted = new SortedList<float, int>();

            foreach (Skeleton s in skeletonDataValue)
            {
                if (s.TrackingState != SkeletonTrackingState.NotTracked)
                {
                    float valueZ = s.Position.Z;
                    while (depthSorted.ContainsKey(valueZ))
                    {
                        valueZ += 0.0001f;
                    }

                    depthSorted.Add(valueZ, s.TrackingId);
                }
            }

            ChooseSkeletonsFromList(depthSorted.Values, count);
        }

        private void ChooseOldestSkeletons(IEnumerable<Skeleton> skeletonDataValue, int count)
        {
            List<int> newList = (from s in skeletonDataValue where s.TrackingState != SkeletonTrackingState.NotTracked select s.TrackingId).ToList();

            // Remove all elements from the active list that are not currently present
            activeList.RemoveAll(k => !newList.Contains(k));

            // Add all elements that aren't already in the activeList
            activeList.AddRange(newList.FindAll(k => !activeList.Contains(k)));

            ChooseSkeletonsFromList(activeList, count);
        }

        private void ChooseMostActiveSkeletons(IEnumerable<Skeleton> skeletonDataValue, int count)
        {
            foreach (ActivityWatcher watcher in recentActivity)
            {
                watcher.NewPass();
            }

            foreach (Skeleton s in skeletonDataValue)
            {
                if (s.TrackingState != SkeletonTrackingState.NotTracked)
                {
                    ActivityWatcher watcher = recentActivity.Find(w => w.TrackingId == s.TrackingId);
                    if (watcher != null)
                    {
                        watcher.Update(s);
                    }
                    else
                    {
                        recentActivity.Add(new ActivityWatcher(s));
                    }
                }
            }

            // Remove any skeletons that are gone
            recentActivity.RemoveAll(aw => !aw.Updated);

            recentActivity.Sort();
            ChooseSkeletonsFromList(recentActivity.ConvertAll(f => f.TrackingId), count);
        }

        private void ChooseSkeletonsFromList(IList<int> list, int max)
        {
            if (KinectSensorManager.SkeletonStreamEnabled)
            {
                int argCount = Math.Min(list.Count, max);

                if (argCount == 0)
                {
                    KinectSensorManager.KinectSensor.SkeletonStream.ChooseSkeletons();
                }

                if (argCount == 1)
                {
                    KinectSensorManager.KinectSensor.SkeletonStream.ChooseSkeletons(list[0]);
                }

                if (argCount >= 2)
                {
                    KinectSensorManager.KinectSensor.SkeletonStream.ChooseSkeletons(list[0], list[1]);
                }
            }
        }

        /// <summary>
        /// Private class used to track the activity of a given player over time, which can be used to assist the KinectSkeletonChooser 
        /// when determing which player to track.
        /// </summary>
        private class ActivityWatcher : IComparable<ActivityWatcher>
        {
            private const float ActivityFalloff = 0.98f;
            private float activityLevel;
            private SkeletonPoint previousPosition;
            private SkeletonPoint previousDelta;

            internal ActivityWatcher(Skeleton s)
            {
                activityLevel = 0.0f;
                TrackingId = s.TrackingId;
                Updated = true;
                previousPosition = s.Position;
                previousDelta = new SkeletonPoint();
            }

            internal int TrackingId { get; private set; }

            internal bool Updated { get; private set; }

            public int CompareTo(ActivityWatcher other)
            {
                if (null == other)
                {
                    return -1;
                }

                // Use the existing CompareTo on float, but reverse the arguments,
                // since we wish to have larger activityLevels sort ahead of smaller values.
                return other.activityLevel.CompareTo(activityLevel);
            }

            internal void NewPass()
            {
                Updated = false;
            }

            internal void Update(Skeleton s)
            {
                SkeletonPoint newPosition = s.Position;
                SkeletonPoint newDelta = new SkeletonPoint
                {
                    X = newPosition.X - previousPosition.X,
                    Y = newPosition.Y - previousPosition.Y,
                    Z = newPosition.Z - previousPosition.Z
                };

                SkeletonPoint deltaV = new SkeletonPoint
                {
                    X = newDelta.X - previousDelta.X,
                    Y = newDelta.Y - previousDelta.Y,
                    Z = newDelta.Z - previousDelta.Z
                };

                previousPosition = newPosition;
                previousDelta = newDelta;

                float deltaVLengthSquared = (deltaV.X * deltaV.X) + (deltaV.Y * deltaV.Y) + (deltaV.Z * deltaV.Z);
                float deltaVLength = (float)Math.Sqrt(deltaVLengthSquared);

                activityLevel = activityLevel * ActivityFalloff;
                activityLevel += deltaVLength;

                Updated = true;
            }
        }
    }
}
