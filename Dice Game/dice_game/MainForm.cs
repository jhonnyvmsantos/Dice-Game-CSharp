using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace dice_game
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		
		SoundPlayer rolling = new SoundPlayer("rollingDice.wav");
		Random randomizer = new Random();
		Timer timer = new Timer();
		Label points = new Label();
		Button start = new Button();
		Button finished = new Button();
		
		int[] listPoints = new int[]
		{
			0, 0
		};
		
		int count = 0;
		
		void MainFormLoad(object sender, EventArgs e)
		{
			this.BackColor = Color.Green;
			
			points.Text = "Computer: " + listPoints[0].ToString() + "\nPlayer: " + listPoints[1].ToString();
			points.Top = 50; points.Left = (this.Width / 2) + 25;
			points.Font = new Font(FontFamily.GenericSerif, 17f, FontStyle.Bold);
			points.AutoSize = true; points.Parent = this;
			
			for (int i = 0; i < 5; i++)
			{
				PictureBox dice = new PictureBox();
				dice.Name = "user"; dice.Tag = i;
				dice.SizeMode = PictureBoxSizeMode.StretchImage;
				dice.Width = 100; dice.Height = 100;
				dice.Load("interrogation.png");
				dice.Left = 110 * i + 1 + 15;
				dice.Top = this.Height - 240;
				dice.Click += PictureBoxClick;
				dice.Parent = this;
			}
			
			for (int i = 0; i < 2; i++)
			{
				PictureBox dice = new PictureBox();
				dice.Name = "computer"; dice.Tag = i;
				dice.SizeMode = PictureBoxSizeMode.StretchImage;
				dice.Width = 100; dice.Height = 100;
				dice.Load("interrogation.png");
				dice.Left = 110 * i + 1 + 15;
				dice.Top = 50;
				dice.Click += PictureBoxClick;
				dice.Parent = this;
			}
			
			start.Width = 150; start.Height = 50;
			start.Top = this.Height - 100;
			start.Left = 15;
			start.Text = "INICIAR";
			start.Click += ButtonClickStart;
			start.Parent = this;
			
			finished.Width = 150; finished.Height = 50;
			finished.Top = this.Height - 100;
			finished.Left = this.Width - 185;
			finished.Text = "FINALIZAR";
			finished.Click += ButtonClickFinish;
			finished.Parent = this;
		}
		
		void PictureBoxClick(object sender, EventArgs e)
		{
			PictureBox pic = sender as PictureBox;
			string name = pic.Name.ToString();
			
			switch (name)
			{
				case "computer":
					MessageBox.Show("Computer " + pic.Tag.ToString());
					break;
				case "user":
					MessageBox.Show("User " + pic.Tag.ToString());
					break;
			}

		}
		
		void ButtonClickStart(object sender, EventArgs e)
		{
			
		}
		
		void ButtonClickFinish(object sender, EventArgs e)
		{
			
		}
	}
}
