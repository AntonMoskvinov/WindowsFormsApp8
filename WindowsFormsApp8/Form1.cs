using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class app : Form
    {
        public app()
        {
            InitializeComponent();

        }



        public void Open_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Текстовый документ |*.txt";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                char[] delimiter = new char[] { '\n', '\t', ' ', '\r' };
                string[] s = File.ReadAllText(dialog.FileName).Split(delimiter,
                StringSplitOptions.RemoveEmptyEntries);

                double[] xValues = (from val in s
                                    let res = double.Parse(val)
                                    orderby res
                                    select res).Distinct().ToArray();
                GraphPane pane = zedGraph.GraphPane; // Получим панель для рисования
                // Очистим список кривых на тот случай, если до этого сигналы уже были нарисованы
                pane.CurveList.Clear();
                PointPairList f1_list = new PointPairList(); // Создадим список точек для кривой Графика №1
                PointPairList f2_list = new PointPairList(); // Создадим список точек для кривой Графика №2
                // Заполним массив точек для Графика №1
                foreach (float x in xValues)
                {
                    double y = (x / 3f) * Math.Cos(x - 1f - (Math.PI / 3f));
                    f1_list.Add(x, y);
                }
                // Заполним массив точек для Графика №2
                // Интервал и шаги по X могут не совпадать на разных кривых
                foreach (float x in xValues)
                {
                    double y = (Math.Abs(x - 1) / 2) * 1.75;
                    f2_list.Add(x, y);
                }
            
                LineItem f1_curve = pane.AddCurve("График №1", f1_list, Color.Blue, SymbolType.Plus);
            
                LineItem f2_curve = pane.AddCurve("График №2", f2_list, Color.Red, SymbolType.Plus);

                // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
                // В противном случае на рисунке будет показана только часть графика, 
                // которая умещается в интервалы по осям, установленные по умолчанию
                zedGraph.AxisChange();
                // Обновляем график
                zedGraph.Invalidate();
            }
        }

        private void Save_Btn_Click(object sender, EventArgs e)
        {

        }

        private void Save_img_Btn_Click(object sender, EventArgs e)
        {
        
            zedGraph.SaveAsBitmap();
        }
    }
}