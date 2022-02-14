using System;
using System.Runtime;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParticleEngine
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer frameTimer;
        private static System.Timers.Timer updateTimer;
        public static System.Timers.Timer pSpawnTimer;

        public static int particleCount = 1000;

        public static double baseSize = 5;
        public static double sizeDeviation = 2;

        public static double baseLifetime = 100;
        public static double lifetimeDeviation = 25;

        public static double baseVelocity = 10;
        public static double velocityDeviation = 2;

        public static double UPS = 100;
        public static double FPS = 60;

        public static double gravity = 3;

        public static int spawnInterval = 60;

        public Vector origin = new Vector(100, 100);

        public static Vector direction = new Vector();

        public static int originDeviation = 50;

        public double ElapsedTime = 0;

        public int renderMode = 0;

        List<Particle> particles = new List<Particle>();

        Bitmap refBitmap;

        Bitmap bitmap;

        Graphics g;

        Brush brush = new SolidBrush(Color.Red);

        public Form1()
        {
            InitializeComponent();
            BitmapSetup();
            TimerSetup();
            this.DoubleBuffered = true;
            InstansiateFormMenu();
        }

        private void BitmapSetup()
        {
            refBitmap = new Bitmap(panel1.Size.Width, panel1.Size.Height);

            bitmap = refBitmap;

            g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        private void TimerSetup()
        {
            frameTimer = new System.Timers.Timer();
            frameTimer.Elapsed += OnTimedEvent1;
            frameTimer.Interval = 1000 / FPS;
            frameTimer.AutoReset = true;

            updateTimer = new System.Timers.Timer();
            updateTimer.Elapsed += OnTimedEvent2;
            updateTimer.Interval = 1000 / UPS;
            updateTimer.AutoReset = true;

            pSpawnTimer = new System.Timers.Timer();
            pSpawnTimer.Elapsed += OnTimedEvent3;
            pSpawnTimer.Interval = spawnInterval;
            pSpawnTimer.AutoReset = true;
        }

        private void InstansiateFormMenu()
        {
            domainUpDown8.Text = particleCount.ToString();
            domainUpDown7.Text = FPS.ToString();
            domainUpDown9.Text = gravity.ToString();
            domainUpDown1.Text = baseSize.ToString();
            domainUpDown2.Text = sizeDeviation.ToString();
            domainUpDown6.Text = baseLifetime.ToString();
            domainUpDown5.Text = lifetimeDeviation.ToString();
            domainUpDown4.Text = baseVelocity.ToString();
            domainUpDown3.Text = velocityDeviation.ToString();
            domainUpDown10.Text = origin.Y.ToString();
            domainUpDown11.Text = origin.X.ToString();
            domainUpDown12.Text = originDeviation.ToString();
            label13.Text = direction.ToString();
            panel1.BackColor = Color.LightGreen;
            progressBar1.Maximum = 1000;
        }

        private void OnTimedEvent1(Object source, System.Timers.ElapsedEventArgs e)
        {
            UpdateParticles();
            panel1.Invalidate();
            ElapsedTime += frameTimer.Interval;
            UpdateInfo((Math.Round(ElapsedTime / 1000, 2)).ToString(), particleCount, particles.Count);
        }

        private void OnTimedEvent2(Object source, System.Timers.ElapsedEventArgs e)
        {
            //UpdateParticles();
        }

        private void OnTimedEvent3(Object source, System.Timers.ElapsedEventArgs e)
        {

        }

        public void UpdateParticles()
        {
            Random rand = new Random();

            switch (renderMode)
            {
                case 0:
                    while (particles.Count < particleCount)
                    {
                        Particle particle = new Particle();

                        particle.radius = baseSize + rand.Next((int)-sizeDeviation, (int)sizeDeviation);

                        particle.position = new Vector(origin.X + rand.Next(-originDeviation, originDeviation), origin.Y/* + rand.Next(-originDeviation, originDeviation)*/);

                        particle.life = baseLifetime + rand.NextDouble() * lifetimeDeviation;

                        //particle.color = Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), (rand.Next(0, 255)), (rand.Next(0, 255)));
                        particle.color = Color.FromArgb(rand.Next(55, 200), 112, 128, 144);
                        //double xPercent = 1 - particle.position.X / (double)panel1.Width;
                        //int xColor = (int)(xPercent * 16777216);
                        //particle.color = Color.FromArgb(rand.Next(55, 200), (xColor >> 16) & 0xFF, (xColor >> 8) & 0xFF, xColor & 0xFF);

                        Vector velVector = new Vector();

                        velVector.X = ((double)rand.Next((int)-baseVelocity * 100, (int)baseVelocity * 100)) / 100 + direction.X / 5;
                        velVector.Y = ((double)rand.Next((int)-baseVelocity * 100, (int)baseVelocity * 100)) / 100 + direction.Y / 5;

                        // velVector.X = (double)rand.Next((int)-baseVelocity, (int)baseVelocity);
                        // velVector.Y = (double)rand.NextDouble() * Math.Sqrt(baseVelocity * baseVelocity - velVector.X * velVector.X) + direction.Y / 25;

                        // velVector.X += (direction.X / 25) + rand.Next((int)-velocityDeviation, (int)velocityDeviation);

                        particle.velocity = velVector;

                        particles.Add(particle);
                    }
                    break;

                case 1:
                    break;
            }

            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].life < 0 || particles[i].position.IsOutside(panel1.Size))
                {
                    particles.RemoveAt(i);
                }
                else
                {
                    double xPercent = 1 - particles[i].position.X / (double)panel1.Width;
                    //int xColor = (int)(xPercent * 16777216);
                    //particles[i].color = Color.FromArgb((xColor >> 16) & 0xFF, (xColor >> 8) & 0xFF, xColor & 0xFF);
                    particles[i].position.AddVector(particles[i].velocity);
                    particles[i].velocity.Y += gravity / 10;
                    particles[i].life--;
                }
            }
        }

        void ClearScreen()
        {
            pSpawnTimer.Enabled = false;
            switch (renderMode)
            {
                case 0:
                    break;

                case 1:
                    pSpawnTimer.Enabled = true;
                    break;

                case 2:
                    break;

                case 3:
                    break;
            }

            frameTimer.Interval = 1000 / FPS;

            g.Clear(Color.Transparent);

            bitmap = refBitmap;

            particles.RemoveRange(0, particles.Count);
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g.Clear(Color.Transparent);
            for (int i = 0; i < particles.Count; i++)
            {
                Rectangle rectangle = new Rectangle((int)particles[i].position.X, (int)particles[i].position.Y, (int)particles[i].radius, (int)particles[i].radius);
                brush = new SolidBrush(particles[i].color);
                g.FillEllipse(brush, rectangle);
            }
            panel1.BackgroundImage = bitmap;
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        delegate void UpdateInfoCallBack(string text, int maxCount, int valueCount);

        private void UpdateInfo(string text, int maxCount, int valueCount)
        {
            if (this.label3.InvokeRequired)
            {
                UpdateInfoCallBack d = new UpdateInfoCallBack(UpdateInfo);
                this.Invoke(d, new object[] { text, maxCount, valueCount });
            }
            else
            {
                this.label3.Text = "Elapsed Time: " + text + " s";

                this.progressBar1.Value = (int)(Clamp((double)valueCount / (double)(maxCount + 1), 0d, 1d) * 1000);
            }
        }

        public double Clamp(double input, double min, double max)
        {
            double output = input;

            if (input > max)
            {
                output = max;
            }
            else if (input < min)
            {
                output = min;
            }

            return output;
        }

        private void domainUpDown8_SelectedItemChanged(object sender, EventArgs e)
        {
            int.TryParse(domainUpDown8.Text, out particleCount);
        }

        private void domainUpDown7_SelectedItemChanged(object sender, EventArgs e)
        {
            double.TryParse(domainUpDown7.Text, out FPS);
        }

        private void domainUpDown9_SelectedItemChanged(object sender, EventArgs e)
        {
            double.TryParse(domainUpDown9.Text, out gravity);
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

            double.TryParse(domainUpDown1.Text, out baseSize);
        }

        private void domainUpDown2_SelectedItemChanged(object sender, EventArgs e)
        {
            double.TryParse(domainUpDown2.Text, out sizeDeviation);
        }

        private void domainUpDown6_SelectedItemChanged(object sender, EventArgs e)
        {
            double.TryParse(domainUpDown6.Text, out baseLifetime);
        }

        private void domainUpDown5_SelectedItemChanged(object sender, EventArgs e)
        {
            double.TryParse(domainUpDown5.Text, out lifetimeDeviation);
        }

        private void domainUpDown4_SelectedItemChanged(object sender, EventArgs e)
        {
            double.TryParse(domainUpDown4.Text, out baseVelocity);
        }

        private void domainUpDown3_SelectedItemChanged(object sender, EventArgs e)
        {
            double.TryParse(domainUpDown3.Text, out velocityDeviation);
        }

        private void domainUpDown10_SelectedItemChanged(object sender, EventArgs e)
        {
            int.TryParse(domainUpDown10.Text, out int o);
            origin.Y = o;
        }

        private void domainUpDown11_SelectedItemChanged(object sender, EventArgs e)
        {
            int.TryParse(domainUpDown11.Text, out int o);
            origin.X = o;
        }

        private void domainUpDown12_SelectedItemChanged(object sender, EventArgs e)
        {
            int.TryParse(domainUpDown12.Text, out originDeviation);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frameTimer.Enabled = !frameTimer.Enabled;
            updateTimer.Enabled = !updateTimer.Enabled;
            ClearScreen();
            if (frameTimer.Enabled)
            {
                button1.Text = "Stop";
                UpdateParticles();
            }
            else
            {
                button1.Text = "Start";
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            direction.Y = -trackBar2.Value;
            label13.Text = direction.ToString();
            label15.Text = Math.Round(direction.Magnitude(), 2).ToString();
            panel2.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            direction.X = trackBar1.Value;
            label13.Text = direction.ToString();
            label15.Text = Math.Round(direction.Magnitude(), 2).ToString();
            panel2.Invalidate();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = panel2.CreateGraphics();
            Pen pen = new Pen(Brushes.Black);
            pen.Width = 2f;
            Point point1 = new Point(panel2.Size.Width / 2, panel2.Size.Height / 2);
            Point point2 = new Point((int)direction.X + panel2.Size.Width / 2, (int)direction.Y + panel2.Size.Height / 2);
            gr.DrawLine(pen, point1, point2);
            gr.DrawRectangle(pen, new Rectangle(point2.X - 3, point2.Y - 3, 6, 6));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            origin.X = panel1.Width / 2;
            origin.Y = panel1.Height / 2;
            InstansiateFormMenu();
        }
    }
}