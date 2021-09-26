using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using org.mariuszgromada.math.mxparser;

namespace dihotomia
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            GraphPane graphfield = zedGraphControl1.GraphPane;
            // Установим цвет рамки для всего компонента
            graphfield.Border.Color = Color.Black;

            // Установим цвет рамки вокруг графика
            graphfield.Chart.Border.Color = Color.Black;

            // Закрасим фон всего компонента ZedGraph
            // Заливка будет сплошная
            graphfield.Fill.Type = FillType.Solid;
            graphfield.Fill.Color = Color.FromArgb(50, 49, 69);

            // Закрасим область графика (его фон) в черный цвет
            graphfield.Chart.Fill.Type = FillType.Solid;
            graphfield.Chart.Fill.Color = Color.Black;

            // Включим показ оси на уровне X = 0 и Y = 0, чтобы видеть цвет осей
            graphfield.XAxis.MajorGrid.IsZeroLine = true;
            graphfield.YAxis.MajorGrid.IsZeroLine = true;
            // Установим цвет осей
            graphfield.XAxis.Color = Color.CornflowerBlue;
            graphfield.YAxis.Color = Color.CornflowerBlue;

            // Включим сетку
            graphfield.XAxis.MajorGrid.IsVisible = true;
            graphfield.YAxis.MajorGrid.IsVisible = true;
            // Установим цвет для сетки
            graphfield.XAxis.MajorGrid.Color = Color.FromArgb(62, 120, 138);
            graphfield.YAxis.MajorGrid.Color = Color.FromArgb(62, 120, 138);
            //62, 120, 138

            // Установим цвет для подписей рядом с осями
            graphfield.XAxis.Title.FontSpec.FontColor = Color.Silver;
            graphfield.YAxis.Title.FontSpec.FontColor = Color.Silver;

            // Установим цвет подписей под метками
            graphfield.XAxis.Scale.FontSpec.FontColor = Color.Silver;
            graphfield.YAxis.Scale.FontSpec.FontColor = Color.Silver;

            // Установим цвет заголовка над графиком
            graphfield.Title.FontSpec.FontColor = Color.White;

        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown_1(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


        private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double amin, bmax;
            PointPairList stack1 = new PointPairList();
            PointPairList stack2 = new PointPairList();
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            pane.GraphObjList.Clear();



            if (tbA.Text == "")
            {
                MessageBox.Show("Введите A");
            }
            else if (tbB.Text == "")
            {
                MessageBox.Show("Введите B");
            }
            else
            {
                amin = Convert.ToDouble(tbA.Text);
                bmax = Convert.ToDouble(tbB.Text);
                //Отрисовка графика по границам
                for (double x = amin; x <= bmax; x += 0.1)
                {
                    stack1.Add(x, f(x));
                }
                //Обозначение минимума на графике

                double minX = Dichotomy(double.Parse(tbA.Text), double.Parse(tbB.Text), double.Parse(tbE.Text));
                stack2.Add(minX, f(minX));
                min.Text = Convert.ToString(minX);
                //цвет графика и метки
                LineItem Curve = pane.AddCurve(tbFX.Text, stack1, Color.FromArgb(184, 57, 8), SymbolType.None);
                LineItem Min = pane.AddCurve("Минимум", stack2, Color.DarkCyan, SymbolType.XCross);
                //обновление графика
                zedGraphControl1.AxisChange();

                Argument x_arg = new Argument("x");
                Curve.Line.Width = 2.0F;
                GraphPane graphfield = zedGraphControl1.GraphPane;
                graphfield.Title.Text = "График" + " " + tbFX.Text;
                zedGraphControl1.Invalidate();

            }
        }

        //Очистка
        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphPane graphfield = zedGraphControl1.GraphPane;
            graphfield.CurveList.Clear();
            graphfield.CurveList.Clear();
            graphfield.GraphObjList.Clear();
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }


        //Функция
        private double f(double x)
        {
            Argument x_arg = new Argument("x");
            Expression fx = new Expression(tbFX.Text, x_arg);
            x_arg.setArgumentValue(x);
            return fx.calculate();
        }


        //Расчёт дихотомии
        private double Dichotomy(double a, double b, double e)
        {
            double x;
            while (b - a > e)
            {
                x = (a + b) / 2;

                if (f(x - e) < f(x + e))
                {
                    b = x;
                }
                else
                {
                    a = x;
                }
            }

            x = (a + b) / 2;
            return x;
        }



        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tbFX_keyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tbA_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 45) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void tbB_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 45) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }

        private void tbE_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44) //цифры, клавиша BackSpace и запятая а ASCII
            {
                e.Handled = true;
            }
        }
    }
}

