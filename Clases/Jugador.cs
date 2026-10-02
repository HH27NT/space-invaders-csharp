using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SpaceInvaders
{
    public class Jugador
    {
        public PictureBox Sprite { get; private set; }

        public Jugador(Form form)
        {
            Sprite = new PictureBox
            {
                Image = Image.FromFile("Recursos/jugador.png"),
                SizeMode = PictureBoxSizeMode.StretchImage, 
                Size = new Size(50, 50), 
                Top = form.ClientSize.Height - 80,
                Left = (form.ClientSize.Width / 2) - 25 
            };
            form.Controls.Add(Sprite);
        }
        public void Mover(int direccion)
        {
            Sprite.Left += direccion;
        }
    }
}