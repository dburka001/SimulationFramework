using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroSim.DataSource.Entities;
using System.Drawing.Imaging;
using System.IO;
using MicroSim.DataSource.DataAccess;

namespace MicroSim.DataSource.Core
{
    /// <summary>
    /// Default User Control for displaying an Age Tree
    /// </summary>
    /// <seealso cref="MicroSim.DataSource.Core.MsfBaseUC" />
    public partial class MsfAgeTreeDisplayUC : UserControl, IBaseUC<IPopulationEntity>
    {
        private Pen _pen = new Pen(Color.Black, 1f);
        private SolidBrush _brush = new SolidBrush(Color.Black);
        private SolidBrush _maleBrush = new SolidBrush(Color.Blue);
        private SolidBrush _femaleBrush = new SolidBrush(Color.Red);
        private int? _year = null;        
        private int _maxAge;
        private Bitmap bitmap;
        private Graphics graphics;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsfAgeTreeDisplayUC"/> class.
        /// </summary>
        public MsfAgeTreeDisplayUC()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            Paint += MsfAgeTreeDisplayUCPaint;

            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add(new MenuItem("Save", MenuItemSaveClick));           
            displayPanel.ContextMenu = cm;
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public float? MaxValue { get; set; } = null;

        /// <summary>
        /// The data
        /// </summary>
        public List<IPopulationEntity> Data { get; } = new List<IPopulationEntity>();

        /// <summary>
        /// Contextmenu Save Click eventhandler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MenuItemSaveClick(Object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            sfd.DefaultExt = ".jpg";
            sfd.InitialDirectory =  Settings.WorkingDirectory;

            if (sfd.ShowDialog() != DialogResult.OK) return;

            ImageFormat format = ImageFormat.Jpeg;
            var extension = Path.GetExtension(sfd.FileName);
            switch (extension.ToLower())
            {
                case ".jpg": format = ImageFormat.Jpeg; break;
                case ".gif": format = ImageFormat.Gif; break;
                case ".bmp": format = ImageFormat.Bmp; break;
                default: throw new NotImplementedException(extension);
            }

            bitmap.Save(sfd.FileName, format);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData(IEnumerable<IPopulationEntity> data)
        {
            Data.Clear();
            Data.AddRange(data);
            if (data.Count() == 0) return;
            var allValueNull = Data.Count(d => d.Value != null) == 0;
            if (MaxValue == null)
                MaxValue = !allValueNull
                    ? Math.Max((float)Data.Max(d => d.Value), 1)
                    : 0f;
            if (_year == null)
                _year = !allValueNull
                    ? data.Where(d => d.Value != null).Min(d => d.Year)
                    : data.Min(d => d.Year);           
            _maxAge = Data.Max(d => d.Age);
            hScrollBar.Minimum = data.Min(d => d.Year);
            hScrollBar.Maximum = data.Max(d => d.Year);
            _year = _year < hScrollBar.Minimum
                ? hScrollBar.Minimum
                : _year > hScrollBar.Maximum
                    ? hScrollBar.Maximum
                    : _year;
            hScrollBar.Value = (int)_year;
        }

        /// <summary>
        /// MSFs the age tree display uc paint.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        private void MsfAgeTreeDisplayUCPaint(object sender, PaintEventArgs e)
        {
            ShowDisplayDataSet();
        }

        /// <summary>
        /// Handles the Scroll event of the hScrollBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ScrollEventArgs"/> instance containing the event data.</param>
        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _year = e.NewValue;
            ShowDisplayDataSet();
        }

        /// <summary>
        /// Shows the display data set.
        /// </summary>
        private void ShowDisplayDataSet()
        {                        
            if (bitmap != null) bitmap.Dispose();
            if (graphics != null) graphics.Dispose();

            bitmap = createBitmap(_year);

            displayPanel.CreateGraphics().DrawImage(bitmap, new PointF(0.0f, 0.0f));
        }

        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        private Bitmap createBitmap(int? year)
        {
            var data = Data.Where(d => d.Year == year);
            Bitmap bitmap = new Bitmap(displayPanel.Width, displayPanel.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            if (data.Count() == 0) return bitmap;

            var boxHorizontalSpacing = 20f;
            var boxVerticalSpacing = 40f;
            var sideWidth = bitmap.Width / 2f;
            var multiplier = MaxValue != 0
                ? ((sideWidth - boxHorizontalSpacing) / (float)MaxValue)
                : 0f;
            var height = (bitmap.Height - boxVerticalSpacing) / _maxAge;

            foreach (var entity in data)
            {
                if (entity.Value == null) continue;
                var width = (float)entity.Value * multiplier;
                RectangleF rect = new RectangleF(0.0f, bitmap.Height - (entity.Age + 1) * height, width, height);

                Brush brush;
                if (entity.Gender == Gender.Male)
                {
                    brush = _maleBrush;
                    rect.X = sideWidth - boxHorizontalSpacing - width;
                }
                else
                {
                    brush = _femaleBrush;
                    rect.X = sideWidth + boxHorizontalSpacing;
                }

                graphics.FillRectangle(brush, rect);
                graphics.DrawRectangle(_pen, rect.X, rect.Y, rect.Width, rect.Height);
            }

            var tickStep = (float)Math.Pow(10, Math.Floor(Math.Log10((float)MaxValue)));
            if ((MaxValue / tickStep) <= 2) tickStep /= 10;

            for (float tick = 0.0f; tick < MaxValue * multiplier; tick += tickStep * multiplier)
            {
                float leftPosition = sideWidth - boxHorizontalSpacing - tick;
                graphics.DrawLine(_pen, leftPosition, boxVerticalSpacing - height, leftPosition, bitmap.Height);
                float rightPosition = sideWidth + boxHorizontalSpacing + tick;
                graphics.DrawLine(_pen, rightPosition, boxVerticalSpacing - height, rightPosition, bitmap.Height);
            }
            RectangleF layoutRectangle = new RectangleF(0.0f, 0.0f, 100f, boxVerticalSpacing);
            Font font1 = new Font("Arial", 20f);
            StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Near
            };

            graphics.DrawString(Resources.Males, font1, _brush, layoutRectangle, format);
            layoutRectangle.X = bitmap.Width - layoutRectangle.Width;
            format.Alignment = StringAlignment.Far;
            graphics.DrawString(Resources.Females, font1, _brush, layoutRectangle, format);
            layoutRectangle.X = ((bitmap.Width - layoutRectangle.Width) / 2f);
            format.Alignment = StringAlignment.Center;
            graphics.DrawString(year.ToString(), font1, _brush, layoutRectangle, format);
            Font font2 = new Font("Arial", 10f);
            layoutRectangle.X = sideWidth - boxHorizontalSpacing;
            layoutRectangle.Width = boxHorizontalSpacing * 2f;
            layoutRectangle.Height = height * 5f;
            for (int index = 0; index <= _maxAge; index += 5)
            {
                layoutRectangle.Y = bitmap.Height - (index + 1.5f) * height;
                graphics.DrawString(index.ToString(), font2, _brush, layoutRectangle, format);
            }

            return bitmap;
        }
    }
}
