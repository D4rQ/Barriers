using System.Diagnostics;

namespace Barriers
{
    public partial class Barriers : Form
    {
        private Cell[,] cells = new Cell[8, 8];
        private Color Color;
        private Cell? current;
        private int turn; // 0 - ход первого игрока, 1 - второго
        private int state; // 0 - выбор первой клетки, 1 - выбор второй
        private int countdown = 60;
        private TimeOnly time = new TimeOnly(0, 1, 0);
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 1000 };
        private EventHandler first;
        private EventHandler second;
        private Dictionary<string, Color> colors = new Dictionary<string, Color>()
        {
            { "red", Color.Red },
            { "blue", Color.Blue },
            { "orange", Color.DarkOrange },
            { "purple", Color.Purple },
            { "green", Color.Green },
            { "yellow", Color.Yellow },
        };
        private Dictionary<Color, Color> antipods = new Dictionary<Color, Color>()
        {
            {Color.Red, Color.Green},
            {Color.Blue, Color.DarkOrange},
            {Color.Yellow, Color.Purple},
            {Color.Green, Color.Red},
            {Color.DarkOrange, Color.Blue},
            {Color.Purple , Color.Yellow}
        };
        Cell trash;

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

            first = (s, e) => CountdownTimer_Tick(s!, e, timer1);
            second = (s, e) => CountdownTimer_Tick(s!, e, timer2);

            FormClosing += (sender, e) => Process.GetCurrentProcess().Kill();

            foreach (var i in GenerateRandomCoords()) // Расставляем бомбы в трех рандомных местах
                cells[i.Item1, i.Item2].Bomb = true;

            timer.Tick += first;
            timer.Start();
            timer2.Visible = false;

            Color_Click("red");
            giveup2.Visible = false;
            trash = new Cell(new Panel() { Name = "trash" });
            DoubleBuffered = true;
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
            if (panel.Cursor == Cursors.No) return;
            var x = int.Parse(panel.Name[7].ToString());
            var y = int.Parse(panel.Name[5].ToString());
            if (state == 0)
            {
                current = cells[x - 1, y - 1];
                panel.BackColor = Color.White;
                DisablePanels();
                CheckPositions(current!);
                state = 1;

            }
            else
            {
                if (panel.Name == current!.Name)
                {
                    current = null;
                    EnablePanels();
                    panel.BackColor = SystemColors.Control;
                    state = 0;
                    return;
                }
                panel.BackColor = Color;
                field.Controls[current!.Name]!.BackColor = Color;
                state = 0;
                if (turn == 0)
                {
                    StupidColorClick("green");
                    ResetTimer();
                    timer.Tick -= first;
                    timer.Tick += second;
                    timer2.Text = "1:00";
                    timer2.Visible = true;
                    timer1.Visible = false;
                    giveup2.Visible = true;
                    giveup1.Visible = false;
                    turn = 1;
                    timer.Start();
                }
                else
                {
                    StupidColorClick("red");
                    ResetTimer();
                    timer.Tick -= second;
                    timer.Tick += first;
                    timer1.Text = "1:00";
                    timer1.Visible = true;
                    timer2.Visible = false;
                    giveup1.Visible = true;
                    giveup2.Visible = false;
                    timer.Start();
                    turn = 0;

                }
                ChangeColors();
                EnablePanels();
                current.Relations.Add(new Relation() { Second = cells[y - 1, x - 1], Color = Color });
                cells[y - 1, x - 1].Relations.Add(new Relation() { Color = Color, Second = current });
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
            => Color_Click("red");
        private void blue_Click(object sender, EventArgs e)
            => Color_Click("blue");

        private void orange_Click(object sender, EventArgs e)
            => Color_Click("orange");

        private void purple_Click(object sender, EventArgs e)
            => Color_Click("purple");

        private void green_Click(object sender, EventArgs e)
            => Color_Click("green");

        private void yellow_Click(object sender, EventArgs e)
            => Color_Click("yellow");

        private void CountdownTimer_Tick(object sender, EventArgs e, Label label)
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
                if (turn == 0)
                    giveup1_Click(sender, e);
                else
                    giveup2_Click(sender, e);
            }
        }
        private void ResetTimer()
        {
            timer.Stop();
            countdown = 60;
        }

        private void Color_Click(string color)
        {
            if (((Panel)background.Controls[color]!).Cursor == Cursors.No) return;
            StupidColorClick(color);
        }

        private void StupidColorClick(string color)
        {
            background.BackgroundImage = Image.FromFile($"Resourses/{color}.png");
            foreach (var item in background.Controls)
                if (item as Panel != null)
                    ((Panel)item).BorderStyle = BorderStyle.None;
            ((Panel)background.Controls[color]!).BorderStyle = BorderStyle.Fixed3D;
            Color = colors[color];
        }

        private void ChangeColors()
        {
            if (turn == 0)
            {
                green.Cursor = Cursors.No;
                purple.Cursor = Cursors.No;
                orange.Cursor = Cursors.No;
                red.Cursor = Cursors.Hand;
                yellow.Cursor = Cursors.Hand;
                blue.Cursor = Cursors.Hand;
            }
            else
            {
                red.Cursor = Cursors.No;
                yellow.Cursor = Cursors.No;
                blue.Cursor = Cursors.No;
                green.Cursor = Cursors.Hand;
                purple.Cursor = Cursors.Hand;
                orange.Cursor = Cursors.Hand;
            }
        }

        private void DisablePanels()
        {
            foreach (var item in field.Controls)
            {
                ((Panel)item).Cursor = Cursors.No;
            }
        }

        private void EnablePanels()
        {
            foreach (var item in field.Controls)
            {
                var panel = ((Panel)item);
                if (panel.Name.Length == 5) break;
                var x = int.Parse(panel.Name[7].ToString());
                var y = int.Parse(panel.Name[5].ToString());
                if (cells[x - 1, y - 1].Relations.Count < 2)
                {
                    ((Panel)item).Cursor = Cursors.Hand;
                    //((Panel)item).BackColor = SystemColors.Control;
                }
            }
        }

        private void CheckPositions(Cell cell)
        {
            int x, y;
            (x, y) = cell.GetCoords();
            var up = x >= 1 && y >= 2 && x <= 8 && y <= 9 ? cells[x - 1, y - 2] : trash;
            var down = x >= 1 && y >= 0 && x <= 8 && y <= 7 ? cells[x - 1, y] : trash;
            var left = x >= 2 && y >= 1 && x <= 9 && y <= 8 ? cells[x - 2, y - 1] : trash;
            var right = x >= 0 && y >= 1 && x <= 7 && y <= 8 ? cells[x, y - 1] : trash;

            foreach (var item in new Cell[] { up, down, left, right })
            {
                if (item.Name != "trash")
                {
                    int x1, y1;
                    (x1, y1) = item.GetCoords();
                    if (item.Relations.Count == 0)
                    {
                        field.Controls[$"panel{y1}_{x1}"]!.Cursor = Cursors.Hand; // Всей душой люблю матрицы, в следующий раз буду их именовать в декартовых координатах
                        field.Controls[$"panel{y1}_{x1}"]!.BackColor = Color.Gray;
                    }
                    else if (item.Relations.Count == 1)
                    {
                        if (antipods[Color] == item.Relations.First().Color && item.Relations.First().Second != cell)
                        {
                            field.Controls[$"panel{y1}_{x1}"]!.Cursor = Cursors.Hand; 
                            field.Controls[$"panel{y1}_{x1}"]!.BackColor = Color.Gray;
                        }
                    }
                }
            }
            field.Controls[cell.Name]!.Cursor = Cursors.Hand;
        }

        private void Render()
        {
            foreach (var cell in cells)
            {
                (var x1, var y1) = cell.GetCoords();
                if (cell.Relations.Count == 1)
                {
                    var first = cell.Relations.First().Second;
                    (var x2, var y2) = first!.GetCoords();
                    if (x1 < x2 || y1 < y2)
                        PaintLine(cell.Name, first.Name);
                }
                if (cell.Relations.Count == 2)
                {
                    var second = cell.Relations.Skip(1).First().Second;
                    (var x3, var y3) = second!.GetCoords();
                    if (x1 < x3 || y1 < y3)
                        PaintLine(cell.Name, second.Name);
                }
            }
        }

        private void PaintLine(string c1, string c2)
        {
            var cell1 = cells[int.Parse(c1[7].ToString()) - 1, int.Parse(c1[5].ToString()) - 1];
            var cell2 = cells[int.Parse(c2[7].ToString()) - 1, int.Parse(c2[5].ToString()) - 1];

            (var x1, var y1) = cell1.GetCoords();
            (var x2, var y2) = cell2.GetCoords();

            if (x1 < x2)
            {
                field.Controls.Add(new Panel() { Size = horizontalLine.Size, Location = new Point((y1 - 1) * 96 - 48, (x1 - 1) * 96 + 40), Visible = true, BackColor = Color, Name = $"{x1}{y1}+{x2}{y2}" });
                field.Controls[field.Controls.Count - 1].BringToFront();
            }
            if (y1 < y2)
            {
                field.Controls.Add(new Panel() { Size = verticalLine.Size, Location = new Point((y1 - 1) * 96 + 40, (x1 - 1) * 96 - 48), Visible = true, BackColor = Color, Name = $"{x1}{y1}+{x2}{y2}" });
                field.Controls[field.Controls.Count - 1].BringToFront();
            }
        }
    }
}
