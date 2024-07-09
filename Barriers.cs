using System.Diagnostics;

namespace Barriers
{
    public partial class Barriers : Form
    {
        private Cell[,] cells = new Cell[8, 8];
        private Color Color;
        private int turn; // 0 - ход первого игрока, 1 - второго
        private int state; // 0 - выбор первой клетки, 1 - выбор второй
        private int countdown = 60;
        private TimeOnly time = new TimeOnly(0, 1, 0);
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 1000 };
        private EventHandler first;
        private EventHandler second;
        public Barriers()
        {
            InitializeComponent();
            foreach (var cell in field.Controls)
            {
                var name = ((Panel)cell).Name;
                var x = int.Parse(name[7].ToString());
                var y = int.Parse(name[5].ToString());
                cells[x - 1, y - 1] = new Cell((Panel)cell);
                ((Panel)cell).Click += (sender, e) => panels_Click(sender!, e, (Panel)cell);
            }

            first = (s, e) => countdownTimer_Tick(s!, e, timer1);
            second = (s, e) => countdownTimer_Tick(s!, e, timer2);

            FormClosing += (sender, e) => Process.GetCurrentProcess().Kill();

            foreach (var i in GenerateRandomCoords()) // Расставляем бомбы в трех рандомных местах
                cells[i.Item1, i.Item2].Bomb = true;

            timer.Tick += first;
            timer.Start();
            timer2.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Click(object sender, EventArgs e)
        {

        }

        private void giveup1_Click(object sender, EventArgs e)
        {
            var results = new Results("Player 2 wins");
            timer.Stop();
            results.Show();
            Hide();
        }

        private void giveup2_Click(object sender, EventArgs e)
        {
            var results = new Results("Player 1 wins");
            results.Show();
            Hide();
        }

        private void panels_Click(object sender, EventArgs e, Panel panel) // та самая палка с отдачей 1000 (метод, в котором логика всей игры)
        {
            if (state == 0)
            {
                state = 1;
            }
            else
            {
                state = 0;
                if (turn == 0)
                {
                    ResetTimer();
                    timer.Tick -= first;
                    timer.Tick += second;
                    timer2.Text = "1:00";
                    timer2.Visible = true;
                    timer1.Visible = false;
                    turn = 1;

                    timer.Start();
                }
                else
                {
                    ResetTimer();
                    timer.Tick -= second;
                    timer.Tick += first;
                    timer1.Text = "1:00";
                    timer1.Visible = true;
                    timer2.Visible = false;
                    timer.Start();
                    turn = 0;
                }
            }
        }

        private (int, int)[] GenerateRandomCoords()
        {
            var r = new Random();
            var res = new (int, int)[3];
            for (int i = 0; i < 3; i++)
            {
                var coords = (r.Next(0, 8), r.Next(0, 8));
                if (res.Contains(coords))
                    coords = (r.Next(0, 8), r.Next(0, 8));
                res[i] = (coords);
            }
            return res;
        }

        private void red_Click(object sender, EventArgs e)
            => color_Click("red");
        private void blue_Click(object sender, EventArgs e)
            => color_Click("blue");

        private void orange_Click(object sender, EventArgs e)
            => color_Click("orange");
        
        private void purple_Click(object sender, EventArgs e)
            => color_Click("purple");

        private void green_Click(object sender, EventArgs e)
            => color_Click("green");

        private void yellow_Click(object sender, EventArgs e)
            => color_Click("yellow");

        private void countdownTimer_Tick(object sender, EventArgs e, Label label)
        {
            countdown--; // уменьшаем значение обратного отсчета на 1 каждую секунду 

            if (countdown != -1)
            {
                var text = countdown.ToString();
                label.Text = $"0:{(text.Length >= 2 ? text : $"0{text}")}";
                
            }
            else
            {
                timer.Stop();
                MessageBox.Show("Обратный отсчет закончен!");
            }
        }
        private void ResetTimer()
        {
            timer.Stop();
            countdown = 60;
        }

        private void color_Click(string color)
        {
            background.BackgroundImage = new Bitmap(File.Open($"Resourses/{color}.png", FileMode.Open));
            foreach (var item in background.Controls)
                if (item as Panel != null)
                    ((Panel)item).BorderStyle = BorderStyle.None;
            ((Panel)background.Controls[color]!).BorderStyle = BorderStyle.Fixed3D;
        }
    }
}

