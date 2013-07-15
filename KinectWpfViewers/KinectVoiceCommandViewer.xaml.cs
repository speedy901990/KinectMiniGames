//------------------------------------------------------------------------------
// <copyright file="KinectVoiceCommandViewer.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.WpfViewers
{
    using System;
    using Microsoft.Kinect;

    /// <summary>
    /// Interaction logic for KinectVoiceCommandViewer.xaml
    /// </summary>
    public partial class KinectVoiceCommandViewer : KinectViewer
    {
        public KinectVoiceCommandViewer()
        {
            InitializeComponent();
        }

        // Event: VoiceCommand is changing the status of kinect, host should know
        public delegate void VoiceCommandActivatedHandler(object sender, EventArgs e);

        public event VoiceCommandActivatedHandler VoiceCommandActivated;

        protected override void OnKinectSensorChanged(object sender, KinectSensorManagerEventArgs<KinectSensor> args)
        {
            if ((null != args.OldValue) && (null != args.OldValue.AudioSource))
            {
                // remove old handlers
                kinectSpeechCommander.Stop();
                kinectSpeechCommander.ActiveListeningModeChanged -= KinectSpeechCommander_ActiveListeningModeChanged;
                kinectSpeechCommander.SpeechRecognized -= KinectSpeechCommander_SpeechRecognized;
            }

            if ((null != args.NewValue) && (null != args.NewValue.AudioSource))
            {
                if (args.NewValue.IsRunning)
                {
                    kinectSpeechCommander.Start(args.NewValue);
                }

                // add new handlers
                kinectSpeechCommander.ActiveListeningModeChanged += KinectSpeechCommander_ActiveListeningModeChanged;
                kinectSpeechCommander.SpeechRecognized += KinectSpeechCommander_SpeechRecognized;
            }
        }

        protected override void OnKinectRunningStateChanged(object sender, KinectSensorManagerEventArgs<bool> args)
        {
            if ((null != KinectSensorManager) && 
                (null != KinectSensorManager.KinectSensor))
            {
                if (KinectSensorManager.KinectSensor.IsRunning)
                {
                    kinectSpeechCommander.Start(KinectSensorManager.KinectSensor);
                }
                else
                {
                    kinectSpeechCommander.Stop();
                }
            }
        }

        protected override void OnAudioWasResetBySkeletonEngine(object sender, EventArgs args)
        {
            // If the SkeletonEngine caused a reset of the Audio system, we need to restart the 
            // KinectSpeechCommander.
            // This is to work around a bug present in 1.0 and maintained in 1.5 for compatibility reasons.
            var kinectSensorManager = KinectSensorManager;
            var kinectSensor = (null == kinectSensorManager) ? null : kinectSensorManager.KinectSensor;

            if (null != kinectSensor)
            {
                kinectSpeechCommander.Stop();
                kinectSpeechCommander.Start(kinectSensor);
            }
        }

        private void OnVoiceCommandActivated()
        {
            if (VoiceCommandActivated != null)
            {
                EventArgs e = new EventArgs();

                VoiceCommandActivated(this, e);
            }
        }

        private void KinectSpeechCommander_ActiveListeningModeChanged(object sender, ActiveListeningModeChangedEventArgs e)
        {
            if (e.ActiveListeningModeEnabled)
            {
                textBlockModeFeedback.Text = "Yes?.....";
                textBlockCommandFeedback.Text = "Say: NEAR MODE or DEFAULT MODE for Range, SEATED MODE or STANDING MODE for Skeleton";
            }
            else
            {
                textBlockModeFeedback.Text = "Say KINECT to activate...";
                textBlockCommandFeedback.Text = string.Empty;
            }
        }

        private void KinectSpeechCommander_SpeechRecognized(object sender, SpeechCommanderEventArgs e)
        {
            switch (e.Command)
            {
                case "IRMODE_NEAR":
                    try
                    {
                        KinectSensorManager.DepthRange = DepthRange.Near;
                        textBlockActionFeedback.Text = "Range: Near";
                    }
                    catch (InvalidOperationException)
                    {
                        textBlockActionFeedback.Text = "Near mode is not supported.";
                    }

                    OnVoiceCommandActivated();
                    break;
                case "IRMODE_DEFAULT":
                    KinectSensorManager.DepthRange = DepthRange.Default;
                    textBlockActionFeedback.Text = "Range: Default";
                    OnVoiceCommandActivated();
                    break;
                case "STMODE_SEATED":
                    KinectSensorManager.SkeletonTrackingMode = SkeletonTrackingMode.Seated;
                    textBlockActionFeedback.Text = "Skeleton: Seated";
                    OnVoiceCommandActivated();
                    break;
                case "STMODE_DEFAULT":
                    KinectSensorManager.SkeletonTrackingMode = SkeletonTrackingMode.Default;
                    textBlockActionFeedback.Text = "Skeleton: Default";
                    OnVoiceCommandActivated();
                    break;
                default:
                    break;
            }
        }
    }
}
