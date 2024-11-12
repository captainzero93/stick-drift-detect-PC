using System;
using System.Windows.Forms;
using SharpDX.DirectInput;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;

namespace StickDrift
{
    public partial class Form1 : Form
    {
        // Constants for drift detection
        private const int DEAD_ZONE = 1000;
        private const int DRIFT_THRESHOLD = 500;
        private const int SAMPLE_TIME = 5000;

        private DirectInput directInput;
        private Joystick joystick;
        private List<Point> driftPoints;
        private bool isChecking;
        private DateTime startTime;
        private Point centerOffset = new Point(0, 0);

        public Form1()
        {
            InitializeComponent();
            driftPoints = new List<Point>();
            InitializeDirectInput();
        }

        private void InitializeDirectInput()
        {
            try
            {
                directInput = new DirectInput();

                // Find first connected controller
                var controllers = directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices);
                if (controllers.Count == 0)
                {
                    controllers = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);
                }

                if (controllers.Count == 0)
                {
                    statusLabel.Text = "No controller detected. Please connect a controller.";
                    startButton.Enabled = false;
                    return;
                }

                joystick = new Joystick(directInput, controllers[0].InstanceGuid);
                joystick.Properties.BufferSize = 128;
                joystick.Acquire();

                statusLabel.Text = $"Controller detected: {controllers[0].InstanceName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing controller: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                startButton.Enabled = false;
            }
        }

        private void CalibrateCenter()
        {
            try
            {
                if (joystick == null) return;

                joystick.Poll();
                var state = joystick.GetCurrentState();

                // Store the initial position as center offset
                centerOffset = new Point(
                    state.X - 32767,
                    state.Y - 32767
                );

                statusLabel.Text = "Calibration complete. Starting drift check...";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Calibration error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (!isChecking)
            {
                StartDriftCheck();
            }
            else
            {
                StopDriftCheck();
            }
        }

        private void StartDriftCheck()
        {
            driftPoints.Clear();
            isChecking = true;
            startTime = DateTime.Now;
            startButton.Text = "Stop";

            // Calibrate center position first
            CalibrateCenter();

            statusLabel.Text = "Checking for stick drift... Keep the controller still!";
            updateTimer.Start();
        }

        private void StopDriftCheck()
        {
            isChecking = false;
            updateTimer.Stop();
            startButton.Text = "Start Check";
            AnalyzeDrift();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (joystick == null) return;

            try
            {
                joystick.Poll();
                var state = joystick.GetCurrentState();

                // Apply center calibration and normalize joystick values
                int x = ((state.X - 32767 - centerOffset.X) * visualizer.Width / 65535) + visualizer.Width / 2;
                int y = ((32767 - state.Y + centerOffset.Y) * visualizer.Height / 65535) + visualizer.Height / 2;

                driftPoints.Add(new Point(x, y));
                visualizer.Invalidate();

                // Stop after sample time
                if ((DateTime.Now - startTime).TotalMilliseconds >= SAMPLE_TIME)
                {
                    StopDriftCheck();
                }
            }
            catch (Exception ex)
            {
                statusLabel.Text = $"Error reading controller: {ex.Message}";
                StopDriftCheck();
            }
        }

        private void Visualizer_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw center crosshair
            e.Graphics.DrawLine(Pens.Black,
                visualizer.Width / 2 - 10, visualizer.Height / 2,
                visualizer.Width / 2 + 10, visualizer.Height / 2);
            e.Graphics.DrawLine(Pens.Black,
                visualizer.Width / 2, visualizer.Height / 2 - 10,
                visualizer.Width / 2, visualizer.Height / 2 + 10);

            // Draw deadzone circle
            int deadzoneRadius = DEAD_ZONE * Math.Min(visualizer.Width, visualizer.Height) / 65536;
            e.Graphics.DrawEllipse(Pens.Gray,
                visualizer.Width / 2 - deadzoneRadius,
                visualizer.Height / 2 - deadzoneRadius,
                deadzoneRadius * 2,
                deadzoneRadius * 2);

            // Draw drift points with better visibility
            if (driftPoints.Count > 1)
            {
                using (var pen = new Pen(Color.Red, 3))
                {
                    // Draw current position as a circle
                    if (driftPoints.Count > 0)
                    {
                        var lastPoint = driftPoints[driftPoints.Count - 1];
                        e.Graphics.FillEllipse(Brushes.Red,
                            lastPoint.X - 5, lastPoint.Y - 5,
                            10, 10);
                    }

                    // Draw trail
                    for (int i = 1; i < driftPoints.Count; i++)
                    {
                        var alpha = (int)(255 * (i / (float)driftPoints.Count));
                        using (var fadedPen = new Pen(Color.FromArgb(alpha, Color.Red), 2))
                        {
                            e.Graphics.DrawLine(fadedPen, driftPoints[i - 1], driftPoints[i]);
                        }
                    }
                }

                // Draw debug info
                using (var font = new Font("Arial", 8))
                {
                    var lastPoint = driftPoints[driftPoints.Count - 1];
                    string debugInfo = $"X: {lastPoint.X}, Y: {lastPoint.Y}";
                    e.Graphics.DrawString(debugInfo, font, Brushes.Black, 5, 5);
                }
            }
        }

        private void AnalyzeDrift()
        {
            if (driftPoints.Count < 2)
            {
                statusLabel.Text = "Not enough data collected. Try again.";
                return;
            }

            // Calculate maximum deviation from center
            double maxDrift = 0;
            Point center = new Point(visualizer.Width / 2, visualizer.Height / 2);

            foreach (var point in driftPoints)
            {
                double drift = Math.Sqrt(
                    Math.Pow(point.X - center.X, 2) +
                    Math.Pow(point.Y - center.Y, 2));
                maxDrift = Math.Max(maxDrift, drift);
            }

            // Convert pixels back to joystick units
            double driftUnits = maxDrift * 65536 / Math.Min(visualizer.Width, visualizer.Height);

            if (driftUnits > DRIFT_THRESHOLD)
            {
                statusLabel.Text = $"Stick drift detected! Maximum deviation: {driftUnits:F0} units";
                statusLabel.ForeColor = Color.Red;
            }
            else
            {
                statusLabel.Text = $"No significant drift detected. Maximum deviation: {driftUnits:F0} units";
                statusLabel.ForeColor = Color.Green;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            updateTimer?.Dispose();
            joystick?.Dispose();
            directInput?.Dispose();
            base.OnFormClosing(e);
        }
    }
}