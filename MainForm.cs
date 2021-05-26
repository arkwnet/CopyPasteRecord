// コピペレコーダー Ver.1.0
// (c) 2021 Sora Arakawa all rights reserved.
// Licensed under the MIT License

using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace CopyPasteRecord
{
	public partial class MainForm : Form
	{
		ClipBoardWatcher cbw;
		string latestCopy;
		string currentDir = Directory.GetCurrentDirectory();
		public MainForm()
		{
			InitializeComponent();
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			cbw.Dispose();
			StreamWriter sw = new StreamWriter("savedata", false);
			for (int i = 0; i < listBox1.Items.Count; i++){
				sw.Write(listBox1.Items[i]+"\n");
			}
			sw.Close();
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			if (File.Exists("savedata"))
			{
				StreamReader sr = new StreamReader("savedata");
				while (sr.EndOfStream == false) {
					string line = sr.ReadLine();
					listBox1.Items.Add(line);
				}
				sr.Close();
			}
			InitCopyWatcher(sender, e);
		}
		
		void ListBox1Click(object sender, EventArgs e)
		{
			if(listBox1.SelectedIndex >= 0)
			{
				textBox1.Text = "" + listBox1.Items[listBox1.SelectedIndex];
			}
		}
		
		void ListBox1DoubleClick(object sender, EventArgs e)
		{
			cbw.Dispose();
			CopyItem(sender, e);
			InitCopyWatcher(sender, e);
		}
		
		// メニューボタン
		void PictureBox1Click(object sender, EventArgs e)
		{
			Point p = new Point(0, 32);
			contextMenuStrip1.Show(pictureBox1, p);
		}
		
		// 消去ボタン
		void PictureBox2Click(object sender, EventArgs e)
		{
			DeleteItem(sender, e);
		}
		
		void CopyToolStripMenuItemClick(object sender, EventArgs e)
		{
			CopyItem(sender, e);
		}
		
		void DeleteToolStripMenuItemClick(object sender, EventArgs e)
		{
			DeleteItem(sender, e);
		}
		
		void AllClearToolStripMenuItemClick(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("記録した内容を全て削除します。よろしいですか?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (result == DialogResult.Yes)
			{
				listBox1.Items.Clear();
			}
		}
		
		void ManualToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(currentDir + "/manual.html");
		}
		
		void ArakawaToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://arkw.net/");
		}
		
		void CPRToolStripMenuItemClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://arkw.net/products/win/cpr/");
		}
		
		void VersionToolStripMenuItemClick(object sender, EventArgs e)
		{
			VerInfoForm verInfoForm = new VerInfoForm();
			verInfoForm.Show();
		}
		
		void CloseToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		// クリップボード監視機能の初期化
		void InitCopyWatcher(object sender, EventArgs e)
		{
			cbw = new ClipBoardWatcher();
			cbw.DrawClipBoard += (sender2, e2) => {
				if(Clipboard.ContainsText())
				{
					if(latestCopy != Clipboard.GetText())
					{
						latestCopy = Clipboard.GetText();
						listBox1.Items.Add(latestCopy);
					}
				}
			};
		}
		
		// アイテムのコピー
		void CopyItem(object sender, EventArgs e)
		{
			if(listBox1.SelectedIndex >= 0)
			{
				Clipboard.SetText("" + listBox1.Items[listBox1.SelectedIndex]);
				pictureBox5.Visible = true;
				timer1.Enabled = true;
			}
		}
		
		// アイテムの削除
		void DeleteItem(object sender, EventArgs e)
		{
			if(listBox1.SelectedIndex >= 0)
			{
				listBox1.Items.RemoveAt(listBox1.SelectedIndex);
			}
		}
		
		// タイマー処理
		void Timer1Tick(object sender, EventArgs e)
		{
			if(pictureBox5.Visible) {
				timer1.Enabled = false;
				pictureBox5.Visible = false;
			}
		}
	}
}
