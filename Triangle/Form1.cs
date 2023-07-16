using Business_Logic_Layer.Common.Models;
using Business_Logic_Layer.Common.Services;
using Business_Logic_Layer.Extensions;
using Triangle.ConfiguringApps;

namespace Triangle
{
    //надо создавать свои 
    public partial class Form1 : Form
    {
        private readonly ITriangleService _service;
        private readonly ExceptionMiddleware _exceptionMiddleware;
        public Form1(ITriangleService service, ExceptionMiddleware exceptionMiddleware)
        {
            _service = service;
            _exceptionMiddleware = exceptionMiddleware;
            _exceptionMiddleware.Notify += Exceptions;
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var datas = await FileToGroupData.Map("DB/data.txt");
            var results = await _service.Set(datas);
            results = await _service.Get(results.Id);
            results = ColorFigures.Add(results, Color.YellowGreen);

            CreateGraphics(results, pictureBox1);

            var count = results.Datas.GroupBy(x => x.ZIndex).Count();
            listBox1.Items.Add($"Количество оттенков = {count}");
        }

        private void CreateGraphics(GroupDataBL item, PictureBox control)
        {
            Bitmap bmp = new Bitmap(control.Width, control.Height);
            double maxY = item.Datas.Select(x => x.Points.Max(z => z.Y)).Max();
            double maxX = item.Datas.Select(x => x.Points.Max(z => z.X)).Max();
            maxY = (control.Height - control.Padding.Bottom - control.Padding.Top) / maxY;
            maxX = (control.Width - control.Padding.Right - control.Padding.Left) / maxX;
            var p = maxY > maxX ? maxX : maxY;
            Pen blackPen = new Pen(Color.Black, 2);
            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                foreach (var it in item.Datas.OrderBy(x => x.ZIndex))
                {
                    var r = it.Points.Select(x => new Point() { X = (int)(x.X * p), Y = (int)(x.Y * p) }).ToArray();
                    grfx.FillPolygon(new SolidBrush(it.Color), r);
                    grfx.DrawPolygon(blackPen, r);
                }
            }
            control.Image = bmp;
        }

        private void Exceptions(string message)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("ERROR");
        }

    }
}