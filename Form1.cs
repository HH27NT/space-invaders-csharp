using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceInvaders.Clases.SpaceInvaders;
using SpaceInvaders.Clases;

namespace SpaceInvaders
{
    public partial class Form1 : Form
    {
        private Jugador jugador;
        private List<Enemigo> enemigos;
        private List<Disparo> disparos;
        private List<Disparo> disparosEnemigos;
        private List<Barrera> barreras;
        private Timer gameLoop;
        private int score = 0;
        private int vidas = 3;
        private Label scoreLabel;
        private Label vidasLabel;
        private Random random = new Random();
        private DateTime ultimoDisparoEnemigo = DateTime.MinValue;
        private int tiempoEntreDisparosEnemigo = 1000;
        private int direccionEnemigos = 1;
        private int velocidadHorizontal = 5;
        private int velocidadVertical = 10; 

        public Form1()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("Recursos/fondo.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            scoreLabel = new Label
            {
                Text = "Score: 0",
                ForeColor = Color.White,
                Font = new Font("Arial", 14),
                AutoSize = true,
                Top = 10,
                Left = 10
            };
            this.Controls.Add(scoreLabel);

            vidasLabel = new Label
            {
                Text = "Vidas: 3",
                ForeColor = Color.White,
                Font = new Font("Arial", 14),
                AutoSize = true,
                Top = 10,
                Left = this.ClientSize.Width - 120
            };
            this.Controls.Add(vidasLabel);

            this.DoubleBuffered = true;
            IniciarJuego();
        }

        private void IniciarJuego()
        {
            jugador = new Jugador(this);
            enemigos = new List<Enemigo>();
            disparos = new List<Disparo>();
            disparosEnemigos = new List<Disparo>();
            barreras = new List<Barrera>(); 

            int espacioEntreBarreras = 150;
            int posicionY = this.ClientSize.Height - 150; 
            for (int i = 0; i < 4; i++)
            {
                int posicionX = 100 + i * espacioEntreBarreras;
                barreras.Add(new Barrera(this, posicionX, posicionY));
            }

            int filas = 3 ;
            int columnas = 6;
            int espacioHorizontal = 70;
            int espacioVertical = 60;
            int anchoTotalEnemigos = columnas * espacioHorizontal;
            int offsetX = (this.ClientSize.Width - anchoTotalEnemigos) / 2;
            int offsetY = 50;

            for (int fila = 0; fila < filas; fila++)
            {
                for (int columna = 0; columna < columnas; columna++)
                {
                    int x = offsetX + columna * espacioHorizontal;
                    int y = offsetY + fila * espacioVertical;
                    enemigos.Add(new Enemigo(this, x, y));
                }
            }

            gameLoop = new Timer();
            gameLoop.Interval = 10;
            gameLoop.Tick += ActualizarJuego;
            gameLoop.Start();
        }

        private void ActualizarJuego(object sender, EventArgs e)
        {
            foreach (var disparo in disparos.ToList())
            {
                disparo.Mover(true);

                foreach (var barrera in barreras.ToList())
                {
                    if (disparo.Sprite.Bounds.IntersectsWith(barrera.Sprite.Bounds))
                    {
                        this.Controls.Remove(disparo.Sprite);
                        disparos.Remove(disparo);

                        barrera.RecibirImpacto();

                        if (barrera.Sprite == null || barrera.Sprite.Height <= 0)
                        {
                            barreras.Remove(barrera);
                        }

                        break;
                    }
                }

                foreach (var enemigo in enemigos.ToList())
                {
                    if (disparo.Sprite.Bounds.IntersectsWith(enemigo.Sprite.Bounds))
                    {
                        this.Controls.Remove(enemigo.Sprite);
                        this.Controls.Remove(disparo.Sprite);
                        enemigos.Remove(enemigo);
                        disparos.Remove(disparo);

                        score += 10;
                        scoreLabel.Text = $"Score: {score}";
                        break;
                    }
                }
            }

            foreach (var enemigo in enemigos)
            {
                enemigo.Sprite.Left += velocidadHorizontal * direccionEnemigos;
            }

            bool cambioDireccion = false;

            foreach (var enemigo in enemigos)
            {
                if (enemigo.Sprite.Right >= this.ClientSize.Width && direccionEnemigos == 1)
                {
                    direccionEnemigos = -1;
                    cambioDireccion = true;
                }
                else if (enemigo.Sprite.Left <= 0 && direccionEnemigos == -1)
                {
                    direccionEnemigos = 1;
                    cambioDireccion = true;
                }
            }

            if (cambioDireccion)
            {
                foreach (var enemigo in enemigos)
                {
                    enemigo.Sprite.Top += velocidadVertical;
                }
            }

            if ((DateTime.Now - ultimoDisparoEnemigo).TotalMilliseconds >= tiempoEntreDisparosEnemigo)
            {
                if (enemigos.Count > 0)
                {
                    int indiceEnemigo = random.Next(enemigos.Count);
                    var enemigo = enemigos[indiceEnemigo];
                    disparosEnemigos.Add(new Disparo(this, enemigo.Sprite.Left + enemigo.Sprite.Width / 2 - 5, enemigo.Sprite.Bottom));
                    ultimoDisparoEnemigo = DateTime.Now;
                }
            }

            foreach (var disparoEnemigo in disparosEnemigos.ToList())
            {
                disparoEnemigo.Mover(false);

                foreach (var barrera in barreras.ToList())
                {
                    if (disparoEnemigo.Sprite.Bounds.IntersectsWith(barrera.Sprite.Bounds))
                    {
                        this.Controls.Remove(disparoEnemigo.Sprite);
                        disparosEnemigos.Remove(disparoEnemigo);

                        barrera.RecibirImpacto();

                        if (barrera.Sprite == null || barrera.Sprite.Height <= 0)
                        {
                            barreras.Remove(barrera);
                        }

                        break;
                    }
                }

                if (disparoEnemigo.Sprite.Bounds.IntersectsWith(jugador.Sprite.Bounds))
                {
                    this.Controls.Remove(disparoEnemigo.Sprite);
                    disparosEnemigos.Remove(disparoEnemigo);

                    vidas--;
                    vidasLabel.Text = $"Vidas: {vidas}";

                    if (vidas <= 0)
                    {
                        gameLoop.Stop();
                        MessageBox.Show("YOU DIEEEEEEEEEEEEEEEEEEED");
                        this.Close();
                    }
                }
            }

            this.Refresh();

            if (enemigos.Count == 0)
            {
                gameLoop.Stop();
                MessageBox.Show("VICTORIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                this.Close();
            }

            foreach (var enemigo in enemigos)
            {
                if (enemigo.Sprite.Bottom >= this.ClientSize.Height)
                {
                    gameLoop.Stop();
                    MessageBox.Show("YOU DIEEEEEEEEEEEEEEEEEEED");
                    this.Close();
                    break;
                }
            }
        }

        private DateTime ultimoDisparo = DateTime.MinValue;
        private int tiempoEntreDisparos = 200;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && jugador.Sprite.Left > 0)
            {
                jugador.Mover(-20);
            }
            if (e.KeyCode == Keys.Right && jugador.Sprite.Right < this.ClientSize.Width)
            {
                jugador.Mover(20);
            }
            if (e.KeyCode == Keys.Space)
            {
                if ((DateTime.Now - ultimoDisparo).TotalMilliseconds >= tiempoEntreDisparos)
                {
                    disparos.Add(new Disparo(this, jugador.Sprite.Left + jugador.Sprite.Width / 2 - 5, jugador.Sprite.Top));
                    ultimoDisparo = DateTime.Now;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}