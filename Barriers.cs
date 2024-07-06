namespace Barriers
{
    public partial class Barriers : Form
    {
        private int state = 0; // 0 - выбор первовй клетки, 1 - выбор второй клетки
        private bool[,] states = new bool[10, 10]; // —осто€ни€ клеток: false - свободна, true - зан€та
        private List<Panel> lines = new List<Panel>();
        public Barriers()
        {
            InitializeComponent();
            panels = new List<Panel>()
            {
                panel1_1,
                panel1_2,
                panel1_3,
                panel1_4,
                panel1_5,
                panel1_6,
                panel1_7,
                panel1_8,
                panel2_1,
                panel2_2,
                panel2_3,
                panel2_4,
                panel2_5,
                panel2_6,
                panel2_7,
                panel2_8,
                panel3_1,
                panel3_2,
                panel3_3,
                panel3_4,
                panel3_5,
                panel3_6,
                panel3_7,
                panel3_8,
                panel4_1,
                panel4_2,
                panel4_3,
                panel4_4,
                panel4_5,
                panel4_6,
                panel4_7,
                panel4_8,
                panel5_1,
                panel5_2,
                panel5_3,
                panel5_4,
                panel5_5,
                panel5_6,
                panel5_7,
                panel5_8,
                panel6_1,
                panel6_2,
                panel6_3,
                panel6_4,
                panel6_5,
                panel6_6,
                panel6_7,
                panel6_8,
                panel7_1,
                panel7_2,
                panel7_3,
                panel7_4,
                panel7_5,
                panel7_6,
                panel7_7,
                panel7_8,
                panel8_1,
                panel8_2,
                panel8_3,
                panel8_4,
                panel8_5,
                panel8_6,
                panel8_7,
                panel8_8,
            };
            FormClosing += (sender, e) => System.Diagnostics.Process.GetCurrentProcess().Kill();
            foreach (var panel in panels) panel.Click += (sender, e) => panels_Click(sender!, e, panel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
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
            results.Show();
            Hide();
        }

        private void giveup2_Click(object sender, EventArgs e)
        {
            var results = new Results("Player 1 wins");
            results.Show();
            Hide();
        }

        private void panels_Click(object sender, EventArgs e, Panel panel) // та сама€ палка с отдачей 1000 (метод, в котором логика всей игры)
        {
            var x = int.Parse(panel.Name[5].ToString());
            var y = int.Parse(panel.Name[7].ToString());

            // Ќаходим все граничащие панели, их четыре, так как ход возможен вверх, вниз, влево и вправо.
            var up = field.Controls[$"panel{x}_{y + 1}"];
            var down = field.Controls[$"panel{x}_{y - 1}"];
            var left = field.Controls[$"panel{x - 1}_{y}"];
            var right = field.Controls[$"panel{x + 1}_{y}"];

            if (state == 0 && !states[x, y])
            {
                if (!states[x, y + 1] && up != null) up!.BackColor = Color.Red;
                if (!states[x, y - 1] && down != null)  down!.BackColor = Color.Red;
                if (!states[x - 1, y] && left != null) left!.BackColor = Color.Red;
                if (!states[x + 1, y] && right != null)  right!.BackColor = Color.Red;
                panel.BackColor = Color.Gray;
                states[x, y] = true;
                state = 1;
            }
            else
            {
                if (panel.BackColor == Color.Red)
                {
                    if (field.Controls[$"panel{x}_{y - 1}"]?.BackColor == Color.Gray)
                    {
                        field.Controls.Add(new Panel() { Size = horizontalLine.Size, Location = new Point((y - 1) * 96 - 48, (x - 1) * 96 + 40), Visible = true, BackColor = Color.Red });
                        field.Controls[field.Controls.Count - 1].BringToFront();
                        states[x, y] = true;
                    }
                    else if (field.Controls[$"panel{x}_{y + 1}"]?.BackColor == Color.Gray)
                    {
                        field.Controls.Add(new Panel() { Size = horizontalLine.Size, Location = new Point((y - 1) * 96 + 48, (x - 1) * 96 + 40), Visible = true, BackColor = Color.Red });
                        field.Controls[field.Controls.Count - 1].BringToFront();
                        states[x, y] = true;
                    }
                    else if (field.Controls[$"panel{x - 1}_{y}"]?.BackColor == Color.Gray)
                    {
                        field.Controls.Add(new Panel() { Size = verticalLine.Size, Location = new Point((y - 1) * 96 + 40, (x - 1) * 96 - 48), Visible = true, BackColor = Color.Red });
                        field.Controls[field.Controls.Count - 1].BringToFront();
                        states[x, y] = true;
                    }
                    else if (field.Controls[$"panel{x + 1}_{y}"]?.BackColor == Color.Gray)
                    {
                        field.Controls.Add(new Panel() { Size = verticalLine.Size, Location = new Point((y - 1) * 96 + 40, (x - 1) * 96 + 48), Visible = true, BackColor = Color.Red });
                        field.Controls[field.Controls.Count - 1].BringToFront();
                        states[x, y] = true;
                    }             
                    foreach (var p in panels)
                    {
                        p.BackColor = SystemColors.Control;
                    }
                }
                else
                {
                    foreach (var p in panels)
                    {
                        p.BackColor = SystemColors.Control;
                    }
                }
                state = 0;
            }
        }
    }
}

