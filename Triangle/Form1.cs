using Business_Logic_Layer.Common.Models;
using Business_Logic_Layer.Common.Services;
using Triangle.ConfiguringApps;

namespace Triangle
{
    //надо создавать свои 
    public partial class Form1 : Form
    {
        private readonly IDataService _service;
        private readonly ExceptionMiddleware _exceptionMiddleware;
        public Form1(IDataService service, ExceptionMiddleware exceptionMiddleware)
        {
            _service = service;
            _exceptionMiddleware = exceptionMiddleware;
            _exceptionMiddleware.Notify += Exceptions;
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var results = await _service.Get(Color.YellowGreen, Color.ForestGreen);
            CreateGraphics(results, pictureBox1);

            var count = results.GroupBy(x => x.ZIndex).Count();
            listBox1.Items.Add($"Количество оттенков = {count}");
        }

        private void CreateGraphics(IList<DataBL> items, PictureBox control)
        {
            Bitmap bmp = new Bitmap(control.Width, control.Height);
            double maxY = items.Select(x => x.Points.Max(z => z.Y)).Max();
            double maxX = items.Select(x => x.Points.Max(z => z.X)).Max();
            maxY = (control.Height - control.Padding.Bottom - control.Padding.Top) / maxY;
            maxX = (control.Width - control.Padding.Right - control.Padding.Left) / maxX;
            var p = maxY > maxX ? maxX : maxY;
            Pen blackPen = new Pen(Color.Black, 2);
            using (Graphics grfx = Graphics.FromImage(bmp))
            {
                foreach (var item in items.OrderBy(x => x.ZIndex))
                {
                    var r = item.Points.Select(x => new Point() { X = (int)(x.X * p), Y = (int)(x.Y * p) }).ToArray();
                    grfx.FillPolygon(new SolidBrush(item.Color), r);
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