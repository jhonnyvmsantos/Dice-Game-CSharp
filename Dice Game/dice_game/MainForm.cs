using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.Threading.Tasks;

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
		Timer loading = new Timer();
		Label points = new Label();
		Button start = new Button();
		Button finished = new Button();
		
		int[] listPoints = new int[]
		{
			0, 0
		};
		
		int count = 0;
		int actual = 0;
		
		void MainFormLoad(object sender, EventArgs e)
		{
			this.BackColor = Color.Green;
			loading.Enabled = false;
			loading.Tick += TimerTickLoading;
			loading.Interval = 100;
			
			PointsChange();
			points.Top = 50; points.Left = (this.Width / 2) + 25;
			points.Font = new Font(FontFamily.GenericSerif, 17f, FontStyle.Bold);
			points.AutoSize = true; points.Parent = this;
			
			for (int i = 0; i < 5; i++)
			{
				PictureBox dice = new PictureBox();
				dice.Name = "user" + i.ToString(); 
				dice.Tag = "available";
				dice.SizeMode = PictureBoxSizeMode.StretchImage;
				dice.Width = 100; dice.Height = 100;
				dice.Load("interrogation.png");
				dice.Left = 110 * i + 1 + 15;
				dice.Top = this.Height - 240;
				dice.Enabled = false;
				dice.Click += PictureBoxClick;
				dice.Parent = this;
			}
			
			for (int i = 0; i < 2; i++)
			{
				PictureBox dice = new PictureBox();
				dice.Name = "computer" + i.ToString(); 
				dice.Tag = "available";
				dice.SizeMode = PictureBoxSizeMode.StretchImage;
				dice.Width = 100; dice.Height = 100;
				dice.Load("interrogation.png");
				dice.Left = 110 * i + 1 + 15;
				dice.Top = 50; dice.Enabled = false;
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
			finished.Enabled = false;
		}
		
		async void PictureBoxClick(object sender, EventArgs e)
		{
			finished.Enabled = false;
			
			PictureBox pic = sender as PictureBox;
			pic.Tag = "loading";
			loading.Enabled = true;
			pic.Enabled = false;
			await Task.Delay(1500);
			
			PictureBox next = SearchPic("user", "available") as PictureBox;
			if (next != null)
			{
				next.Enabled = true;
			}
			finished.Enabled = true;
		}
		
		async void ButtonClickStart(object sender, EventArgs e)
		{
			try
			{
				start.Enabled = false;
				
				for (int i = 0; i < 2; i++)
				{
					PictureBox pic = SearchPic("user", "available") as PictureBox;
					pic.Tag = "loading";
					loading.Enabled = true;
					await Task.Delay(1500);
				}
				
				PictureBox next = SearchPic("user", "available") as PictureBox;
				next.Enabled = true; finished.Enabled = true;
			}
			catch
			{
				MessageBox.Show("Nothing!");
			}
		}
		
		async void ButtonClickFinish(object sender, EventArgs e)
		{
			try
			{
				finished.Enabled = false;
				
				foreach (Control control in this.Controls)
			    {
					if (control is PictureBox && control.Name.Contains("user") == true && control.Tag.ToString() == "available")
			        {
			            PictureBox finish = control as PictureBox;
			            finish.Tag = "unavailable";
			            finish.Enabled = false;
			        }
			    }
					
				for (int i = 0; i < 2; i++)
				{
					PictureBox pic = SearchPic("computer", "available") as PictureBox;
					pic.Tag = "loading";
					loading.Enabled = true;
					await Task.Delay(1500);
				}
			}
			catch
			{
				MessageBox.Show("Nothing!");
			}
		}
		
		void TimerTickLoading(object sender, EventArgs e)
		{
			PictureBox pic = null; count++;
			int random = randomizer.Next(1, 7);
			
			for (int i = 0; i < 2; i++)
			{
				if (pic == null)
				{
					pic = SearchPic((i == 0) ? "user" : "computer", "loading") as PictureBox;
				}
			}
			
			while (random == actual)
			{
				random = randomizer.Next(1, 7);
			}
			
			if (pic != null)
			{
				pic.Load("dice" + random.ToString() + ".png");
			}
			
			if (count == 12)
			{
				count = 0;
				loading.Enabled = false;
				
				if (pic != null)
				{
					pic.Tag = "unavailable";
					listPoints[(pic.Name.Contains("computer") == true) ? 0 : 1] += random;
					PointsChange();
				}
			}
			
		}
		
		object SearchPic(string name, string tag)
		{
			foreach (Control control in this.Controls)
		    {
				if (control is PictureBox && control.Name.Contains(name) == true && control.Tag.ToString() == tag)
		        {
		            return control;
		        }
		    }
			
			return null;
		}
		
		void PointsChange()
		{
			points.Text = "Computer: " + listPoints[0].ToString() + "\nPlayer: " + listPoints[1].ToString();
		}
	}
}
