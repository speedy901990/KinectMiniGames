using System;
using Microsoft.Kinect;

namespace DrawingGame
{
    public class KinectManager
    {
        private MainWindow _mainWindow;

        public KinectManager(MainWindow mainWindow)
        {
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            _mainWindow = mainWindow;
        }

        public void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Initializing:
                case KinectStatus.Connected:
                    _mainWindow.KinectDevice = e.Sensor;
                    break;
                case KinectStatus.Disconnected:

                    _mainWindow.KinectDevice = null;
                    break;
                default:

                    break;
            }
        }

        public void SkeletonsReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {

                    Skeleton skeleton;



                    frame.CopySkeletonDataTo(_mainWindow.FrameSkeletons);

                    _mainWindow.SkeletonBoardElement.Children.Clear();
                    GetPrimarySkeleton(_mainWindow.FrameSkeletons);

                    skeleton = GetPrimarySkeleton(_mainWindow.FrameSkeletons);
                    if (skeleton != null)
                    {
                        if (SkeletonTrackingState.Tracked == skeleton.TrackingState)
                        {

                            if (_mainWindow.Player == null)
                            {
                                _mainWindow.Player = new Player(11);
                                _mainWindow.Player.SetBounds(_mainWindow.PlayerBounds);
                            }

                            _mainWindow.Player.LastUpdated = DateTime.Now;

                            // Update player's bone and joint positions
                            if (skeleton.Joints.Count > 0)
                            {
                                _mainWindow.Player.IsAlive = true;

                                // Head, hands, feet (hit testing happens in order here)
                                _mainWindow.Player.UpdateJointPosition(skeleton.Joints, JointType.Head);
                                _mainWindow.Player.UpdateJointPosition(skeleton.Joints, JointType.HandLeft);
                                _mainWindow.Player.UpdateJointPosition(skeleton.Joints, JointType.HandRight);
                                _mainWindow.Player.UpdateJointPosition(skeleton.Joints, JointType.FootLeft);
                                _mainWindow.Player.UpdateJointPosition(skeleton.Joints, JointType.FootRight);

                                // Hands and arms
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.HandRight, JointType.WristRight);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.WristRight, JointType.ElbowRight);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.ElbowRight, JointType.ShoulderRight);
                                //_Player.Draw(SkeletonBoardElement.Children);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.HandLeft, JointType.WristLeft);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.WristLeft, JointType.ElbowLeft);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.ElbowLeft, JointType.ShoulderLeft);


                                // Head and Shoulders
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderCenter, JointType.Head);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderLeft,
                                    JointType.ShoulderCenter);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderCenter,
                                    JointType.ShoulderRight);

                                // Legs
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.HipLeft, JointType.KneeLeft);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.KneeLeft, JointType.AnkleLeft);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.AnkleLeft, JointType.FootLeft);

                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.HipRight, JointType.KneeRight);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.KneeRight, JointType.AnkleRight);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.AnkleRight, JointType.FootRight);

                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.HipLeft, JointType.HipCenter);
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.HipCenter, JointType.HipRight);

                                // Spine
                                _mainWindow.Player.UpdateBonePosition(skeleton.Joints, JointType.HipCenter, JointType.ShoulderCenter);
                            }
                            _mainWindow.Player.Draw(_mainWindow.SkeletonBoardElement.Children);



                            if (!_mainWindow.GameOver)
                            {
                                if (_mainWindow.Configuration.HandsState == 2)
                                {
                                    _mainWindow.Tracker.TrackPuzzle(skeleton.Joints[JointType.HandRight], ref _mainWindow.PuzzleDotIndexR, _mainWindow.ListPuzzleR, _mainWindow.CrayonElementR, _mainWindow.PuzzleBoardElementR, ref _mainWindow.IndexOfCurrentFigureRight, ref _mainWindow.RRaeady, _mainWindow.CrayonElementRforPuzzleLine);
                                    _mainWindow.Tracker.TrackPuzzle(skeleton.Joints[JointType.HandLeft], ref _mainWindow.PuzzleDotIndexL, _mainWindow.ListPuzzleL, _mainWindow.CrayonElementL, _mainWindow.PuzzleBoardElementL, ref _mainWindow.IndexOfCurrentFigureLeft, ref _mainWindow.LRaeady, _mainWindow.CrayonElementLforPuzzleLine);
                                }
                                else if (_mainWindow.Configuration.HandsState == 1)
                                {
                                    if (_mainWindow.CurrentHand == false)
                                        _mainWindow.Tracker.TrackPuzzle(skeleton.Joints[JointType.HandRight], ref _mainWindow.PuzzleDotIndexR, _mainWindow.ListPuzzleR, _mainWindow.CrayonElementR, _mainWindow.PuzzleBoardElementR, ref _mainWindow.IndexOfCurrentFigureRight, ref _mainWindow.RRaeady);

                                    else if (_mainWindow.CurrentHand == true)
                                        _mainWindow.Tracker.TrackPuzzle(skeleton.Joints[JointType.HandLeft], ref _mainWindow.PuzzleDotIndexL, _mainWindow.ListPuzzleL, _mainWindow.CrayonElementL, _mainWindow.PuzzleBoardElementL, ref _mainWindow.IndexOfCurrentFigureLeft, ref _mainWindow.LRaeady);

                                }
                            }



                        }


                    }

                }
            }
        }

        public static Skeleton GetPrimarySkeleton(Skeleton[] skeletons)
        {
            Skeleton skeleton = null;

            if (skeletons != null)
            {

                for (int i = 0; i < skeletons.Length; i++)
                {
                    if (skeletons[i].TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (skeleton == null)
                        {
                            skeleton = skeletons[i];
                        }
                        else
                        {
                            if (skeleton.Position.Z > skeletons[i].Position.Z)
                            {
                                skeleton = skeletons[i];
                            }
                        }
                    }
                }
            }

            return skeleton;
        }
    }
}