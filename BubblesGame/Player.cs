//------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace BubblesGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Microsoft.Kinect;
    using Utils;

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
        private Point _playerCenter;
        private double _playerScale;

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
