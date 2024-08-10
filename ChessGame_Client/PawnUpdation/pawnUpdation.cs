using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PawnUpdation
{
    public partial class pawnUpdation : Form
    {
        public string pieceType { get; set; }
        public Image img { get; set; }

        public pawnUpdation()
        {
            InitializeComponent();
            SetPiece(button1, Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Queen_Black.png"));
            button1.Name = "QUEEN_BLACK";
            SetPiece(button2, Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Bishop_Black.png"));
            button2.Name = "BISHOP_BLACK";
            SetPiece(button3, Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Knight_Black.png"));
            button3.Name = "KNIGHT_BLACK";
            SetPiece(button4, Image.FromFile(@"C:\Users\Muhammad Rizwan\source\repos\ChessGame\ChessGame\Assets\Rook_Black.png"));
            button4.Name = "ROOK_BLACK";

        }

        private void SetPiece(Button button, Image img)
        {
            int imageSize = 40; // Adjust this size to fit your button
            Image resizedImage = ResizeImage(img, imageSize, imageSize);
            button.Image = resizedImage;
            button.ImageAlign = ContentAlignment.MiddleCenter; // Center the image

        }

        private Image ResizeImage(Image image, int width, int height)
        {
            var resizedImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            pieceType = btn.Name;
            img = btn.Image;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            pieceType = btn.Name;
            img = btn.Image;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            pieceType = btn.Name;
            img = btn.Image;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            pieceType = btn.Name;
            img = btn.Image;
            this.Close();
        }
    }
}
