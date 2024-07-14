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
		Label winComputer = new Label();
		Label winPlayer = new Label();
		
		Button start = new Button();
		Button finished = new Button();
		
		int[] listPoints = new int[]
		{
			0, 0
		};
		
		int[] listWins = new int[]
		{
			0, 0
		};
		
		int count = 0, actual = 0;
		bool check = false;
		
		void MainFormLoad(object sender, EventArgs e)
		{
			this.BackColor = Color.Green;
			loading.Enabled = false;
			loading.Tick += TimerTickLoading;
			loading.Interval = 100;
			
			ScoreChange("points");
			points.Top = 50; points.Left = (this.Width / 2) + 25;
			points.Font = new Font(FontFamily.GenericSerif, 17f, FontStyle.Bold);
			points.AutoSize = true; points.Parent = this;
			
			winComputer.Top = 15; winComputer.Left = 15;
			winComputer.Font = new Font(FontFamily.GenericSerif, 14f, FontStyle.Bold);
			winComputer.AutoSize = true; winComputer.Parent = this;
			
			winPlayer.Top = (this.Height / 2) - 45; winPlayer.Left = 15;
			winPlayer.Font = new Font(FontFamily.GenericSerif, 14f, FontStyle.Bold);
			winPlayer.AutoSize = true; winPlayer.Parent = this;
			
			ScoreChange("wins");
			
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
				dice.Top = 60; dice.Enabled = false;
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
			finished.Enabled = true; Checking("check");
			
			if (check == false && next != null)
			{
				next.Enabled = true;
			}
			else if (check == false && next == null)
			{
				finished.PerformClick();
			}
		}
		
		async void ButtonClickStart(object sender, EventArgs e)
		{
			try
			{
				start.Enabled = false;
				Checking("check");
				
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
			            finish.Enabled = false;
			            finish.Tag = "unavailable";
			            finish.Load("eliminated.png");
			            await Task.Delay(100);
			        }
			    }
					
				for (int i = 0; i < 2; i++)
				{
					PictureBox pic = SearchPic("computer", "available") as PictureBox;
					pic.Tag = "loading";
					loading.Enabled = true;
					await Task.Delay(1500);
				}
				
				Checking("finish");
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
			
			if (count == 1)
			{
				rolling.Play();
			}
			
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
					ScoreChange("points");
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
		
		void ScoreChange(string change)
		{
			switch (change)
			{
				case "points":
					points.Text = "Computer: " + listPoints[0].ToString() + "\nPlayer: " + listPoints[1].ToString();
					break;
				case "wins":
					winComputer.Text = "Computer (Wins): " + listWins[0].ToString();
					winPlayer.Text = "Player (Wins): " + listWins[1].ToString();
					break;
			}
		}
		
		void Checking(string status)
		{
			if (listPoints[1] > 13)
			{
				MessageBox.Show("Que pena!!! Seus pontos excederam o limite (13).\nVocê perdeu essa rodada..!");
				check = true; listWins[0] += 1;
				Restart(); ScoreChange("wins");
			}
			
			if (listWins[1] >= 2)
			{
				MessageBox.Show("WOW!!! Parece que você venceu a máquina...\nMeu parabéns por terminar o jogo.");
				Application.Restart();
			}
			else if (listWins[0] >= 2){
				MessageBox.Show("Hum... Sinto em dizer que você perdeu essa partida!\nDesejo mais sorte na próxima.");
				Application.Restart();
			}
			
			if (status == "finish")
				{
					if (listPoints[1] > listPoints[0])
				{
					MessageBox.Show("Parabéns!!! Você ganhou essa rodada.");
					listWins[1] += 1;
				}
				else if (listPoints[0] > listPoints[1])
				{
					MessageBox.Show("Vish... Você perdeu essa rodada!\nBoa sorte na próxima.");
					listWins[0] += 1;
				}
				else
				{
					MessageBox.Show("Hmmmm... Você empatou com a máquina.\nParabéns ???");
				}
				
				check = true; 
				Restart(); ScoreChange("wins");
			}
		}
		
		void Restart()
		{
			foreach (Control control in this.Controls)
		    {
				if (control is PictureBox && control.Name.Contains("user") == true || control.Name.Contains("computer") == true)
		        {
		            PictureBox pic = control as PictureBox;
		            pic.Tag = "available";
		         	pic.Enabled = false;
		         	pic.Load("interrogation.png");
		        }
		    }
			
			for(int i = 0; i < 2; i++){
				listPoints[i] = 0;
			}
			
			finished.Enabled = false; 
			check = false;
			ScoreChange("points");
			
			start.Enabled = true; 
			start.PerformClick();
		}
	}
}
