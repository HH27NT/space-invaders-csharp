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
        public class Disparo
        {
            public PictureBox Sprite { get; private set; }

            public Disparo(Form form, int x, int y)
            {
                Sprite = new PictureBox
                {
                    Image = Image.FromFile("Recursos/disparo.png"), 
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Size = new Size(10, 20),
                    Top = y,
                    Left = x
                };
                form.Controls.Add(Sprite);
            }

            public void Mover(bool esJugador = true)
            {
                if (esJugador)
                {
                    Sprite.Top -= 50;
                }
                else
                {
                    Sprite.Top += 10;
                }
            }
        }
    }
}
