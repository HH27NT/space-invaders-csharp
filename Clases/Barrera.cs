using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SpaceInvaders.Clases
{
    public class Barrera
    {
        public PictureBox Sprite { get; private set; }

        public Barrera(Form form, int x, int y)
        {
            Sprite = new PictureBox
            {
                BackColor = Color.Green,
                Size = new Size(80, 40),
                Top = y,
                Left = x
            };
            form.Controls.Add(Sprite);
        }

        public void RecibirImpacto()
        {
            if (Sprite.Height > 10) 
            {
                Sprite.Height -= 10;
                Sprite.Top += 5; 
            }
            else
            {
                Sprite.Dispose();
                Sprite = null;
            }
        }
    }
}
