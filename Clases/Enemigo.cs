using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
namespace SpaceInvaders.Clases
{
    namespace SpaceInvaders
    {
        public class Enemigo
        {
            public PictureBox Sprite { get; private set; }

            public Enemigo(Form form, int x, int y)
            {
                Sprite = new PictureBox
                {
                    Image = Image.FromFile("Recursos/enemigo.png"),
                    SizeMode = PictureBoxSizeMode.StretchImage, 
                    Size = new Size(40, 40),
                    Top = y,
                    Left = x
                };
                form.Controls.Add(Sprite);
            }
        }
    }
}
