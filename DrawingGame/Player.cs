//------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace DrawingGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Microsoft.Kinect;

    #region Struct
    public struct Bone
    {
        public JointType Joint1;
        public JointType Joint2;

        public Bone(JointType j1, JointType j2)
        {
            Joint1 = j1;
            Joint2 = j2;
        }
    }

    public struct Segment
    {
        public double X1;
        public double Y1;
        public double X2;
        public double Y2;
        public double Radius;

        public Segment(double x, double y)
        {
            Radius = 1;
            X1 = X2 = x;
            Y1 = Y2 = y;
        }

        public Segment(double x1, double y1, double x2, double y2)
        {
            Radius = 1;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public bool IsCircle()
        {
            return (X1 == X2) && (Y1 == Y2);
        }
    }

    public struct BoneData
    {
        public Segment Segment;
        public Segment LastSegment;
        public double XVelocity;
        public double YVelocity;
        public double XVelocity2;
        public double YVelocity2;
        public DateTime TimeLastUpdated;

        private const double Smoothing = 0.8;

        public BoneData(Segment s)
        {
            Segment = LastSegment = s;
            XVelocity = YVelocity = 0;
            XVelocity2 = YVelocity2 = 0;
            TimeLastUpdated = DateTime.Now;
        }

        
        public void UpdateSegment(Segment s)
        {
            LastSegment = Segment;
            Segment = s;

            DateTime cur = DateTime.Now;
            double fMs = cur.Subtract(TimeLastUpdated).TotalMilliseconds;
            if (fMs < 10.0)
            {
                fMs = 10.0;
            }

            double fps = 1000.0 / fMs;
            TimeLastUpdated = cur;

            if (Segment.IsCircle())
            {
                XVelocity = (XVelocity * Smoothing) + ((1.0 - Smoothing) * (Segment.X1 - LastSegment.X1) * fps);
                YVelocity = (YVelocity * Smoothing) + ((1.0 - Smoothing) * (Segment.Y1 - LastSegment.Y1) * fps);
            }
            else
            {
                XVelocity = (XVelocity * Smoothing) + ((1.0 - Smoothing) * (Segment.X1 - LastSegment.X1) * fps);
                YVelocity = (YVelocity * Smoothing) + ((1.0 - Smoothing) * (Segment.Y1 - LastSegment.Y1) * fps);
                XVelocity2 = (XVelocity2 * Smoothing) + ((1.0 - Smoothing) * (Segment.X2 - LastSegment.X2) * fps);
                YVelocity2 = (YVelocity2 * Smoothing) + ((1.0 - Smoothing) * (Segment.Y2 - LastSegment.Y2) * fps);
            }
        }

        // Using the velocity calculated above, estimate where the segment is right now.
        public Segment GetEstimatedSegment(DateTime cur)
        {
            Segment estimate = Segment;
            double fMs = cur.Subtract(TimeLastUpdated).TotalMilliseconds;
            estimate.X1 += (double)((fMs * XVelocity) / 1000);
            estimate.Y1 += (double)((fMs * YVelocity) / 1000);
            if (Segment.IsCircle())
            {
                estimate.X2 = estimate.X1;
                estimate.Y2 = estimate.Y1;
            }
            else
            {
                estimate.X2 += fMs * XVelocity2 / 1000.0;
                estimate.Y2 += fMs * YVelocity2 / 1000.0;
            }

            return estimate;
        }
    }
    #endregion

    public class Player
    {
        private const double BoneSize = 0.01;
        private const double HeadSize = 0.075;
        private const double HandSize = 0.03;

        // Keeping track of all bone segments of interest as well as head, hands and feet
        private readonly Dictionary<Bone, BoneData> _segments = new Dictionary<Bone, BoneData>();
        private readonly Brush _jointsBrush;
        private readonly Brush _bonesBrush;
        private readonly int _id;
        private static int _colorId;
        private Rect _playerBounds;
        public Point _playerCenter;
        public double _playerScale;

        public Player(int skeletonSlot)
        {
            _id = skeletonSlot;

            // Generate one of 7 colors for player
            int[] mixR = { 1, 1, 1, 0, 1, 0, 0 };
            int[] mixG = { 1, 1, 0, 1, 0, 1, 0 };
            int[] mixB = { 1, 0, 1, 1, 0, 0, 1 };
            byte[] jointCols = { 245, 200 };
            byte[] boneCols = { 235, 160 };

            int i = _colorId;
            _colorId = (_colorId + 1) % mixR.Count();

            _jointsBrush = new SolidColorBrush(Color.FromRgb(jointCols[mixR[i]], jointCols[mixG[i]], jointCols[mixB[i]]));
            _bonesBrush = new SolidColorBrush(Color.FromRgb(boneCols[mixR[i]], boneCols[mixG[i]], boneCols[mixB[i]]));
            LastUpdated = DateTime.Now;
        }

        public bool IsAlive { get; set; }

        public DateTime LastUpdated { get; set; }

        public Dictionary<Bone, BoneData> Segments
        {
            get
            {
                return _segments;
            }
        }

        public int GetId()
        {
            return _id;
        }

        public void SetBounds(Rect r)
        {
            _playerBounds = r;
            _playerCenter.X = (_playerBounds.Left + _playerBounds.Right) / 2;
            _playerCenter.Y = (_playerBounds.Top + _playerBounds.Bottom) / 2;
            _playerScale = Math.Min(_playerBounds.Width, _playerBounds.Height / 2);
        }

        public void UpdateBonePosition(JointCollection joints, JointType j1, JointType j2)
        {
            var seg = new Segment(
                (joints[j1].Position.X * _playerScale) + _playerCenter.X,
                _playerCenter.Y - (joints[j1].Position.Y * _playerScale),
                (joints[j2].Position.X * _playerScale) + _playerCenter.X,
                _playerCenter.Y - (joints[j2].Position.Y * _playerScale))
                { Radius = Math.Max(3.0, _playerBounds.Height * BoneSize) / 2 };
            UpdateSegmentPosition(j1, j2, seg);
        }

        public void UpdateJointPosition(JointCollection joints, JointType j)
        {
            var seg = new Segment(
                (joints[j].Position.X * _playerScale) + _playerCenter.X,
                _playerCenter.Y - (joints[j].Position.Y * _playerScale))
                { Radius = _playerBounds.Height * ((j == JointType.Head) ? HeadSize : HandSize) / 2 };
            UpdateSegmentPosition(j, j, seg);
        }

        public void Draw(UIElementCollection children)
        {
            if (!IsAlive)
            {
                return;
            }

            // Draw all bones first, then circles (head and hands).
            DateTime cur = DateTime.Now;
            foreach (var segment in _segments)
            {
                Segment seg = segment.Value.GetEstimatedSegment(cur);
                if (!seg.IsCircle())
                {
                    var line = new Line
                        {
                            StrokeThickness = seg.Radius * 2,
                            X1 = seg.X1,
                            Y1 = seg.Y1,
                            X2 = seg.X2,
                            Y2 = seg.Y2,
                            Stroke = _bonesBrush,
                            StrokeEndLineCap = PenLineCap.Round,
                            StrokeStartLineCap = PenLineCap.Round
                        };
                    children.Add(line);
                }
            }

            foreach (var segment in _segments)
            {
                Segment seg = segment.Value.GetEstimatedSegment(cur);
                if (seg.IsCircle())
                {
                    var circle = new Ellipse { Width = seg.Radius * 2, Height = seg.Radius * 2 };
                    circle.SetValue(Canvas.LeftProperty, seg.X1 - seg.Radius);
                    circle.SetValue(Canvas.TopProperty, seg.Y1 - seg.Radius);
                    circle.Stroke = _jointsBrush;
                    circle.StrokeThickness = 1;
                    circle.Fill = _bonesBrush;
                    children.Add(circle);
                }
            }

            // Remove unused players after 1/2 second.
            if (DateTime.Now.Subtract(LastUpdated).TotalMilliseconds > 500)
            {
                IsAlive = false;
            }
        }

        private void UpdateSegmentPosition(JointType j1, JointType j2, Segment seg)
        {
            var bone = new Bone(j1, j2);
            if (_segments.ContainsKey(bone))
            {
                BoneData data = _segments[bone];
                data.UpdateSegment(seg);
                _segments[bone] = data;
            }
            else
            {
                _segments.Add(bone, new BoneData(seg));
            }
        }
    }
}
