/// <summary>
/// This is a simple (for the time being) engine running simulator.
/// 
/// Changelog:
/// V0.001 New project created. Buttons and tacho created alongside rButtons for cylinder bangs. Initial logic for cr bgworker.
/// </summary>

using System;
using System.ComponentModel;
using System.Media;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;


namespace moottoriSimulaattori
{

	public partial class MainForm : Form
	{
		BackgroundWorker bg_crankshaft;
		
		public MainForm()
		{
			InitializeComponent();
			
			bg_crankshaft = new BackgroundWorker();  //creating a bg worker (thread) for our cr rotation
			
			bg_crankshaft.DoWork += new DoWorkEventHandler(bg_crankshaft_DoWork);
            bg_crankshaft.ProgressChanged += new ProgressChangedEventHandler(bg_crankshaft_ProgressChanged);
            bg_crankshaft.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_crankshaft_RunWorkerCompleted);
            bg_crankshaft.WorkerReportsProgress = true;
            bg_crankshaft.WorkerSupportsCancellation = true;		//event handlers and properties
			
			
		}
		
		void bg_crankshaft_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)		//if thread exits and it has been cancelled
            {
                MessageBox.Show("Engine was turned off");
            }

            if (e.Error != null)
            {
            	MessageBox.Show("It seems that the engine died for some reason... " + e.Error.Data);
            }

        }
		
		void bg_crankshaft_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar_RPM.Value = e.ProgressPercentage;  //bg worker cant paint ui thread so this does it. Sounds here? 
            
        }
		
		void bg_crankshaft_DoWork(object sender, DoWorkEventArgs e)
        {
			//logic here
			double crankshaftPosition;
			double t = 1;
			double frequency = 1;
			Stopwatch runTime = new Stopwatch();
			runTime.Start();
			
			double[] log = new Double[99999];
			int i_log = 0;
			
			while(!bg_crankshaft.CancellationPending)
			{
				//t = runTime.ElapsedMilliseconds;
				t++;
				crankshaftPosition = (1 * Math.Sin((2 * Math.PI * frequency * t)));
				
				log[i_log] = crankshaftPosition;
				i_log++;
				Thread.Sleep(1);
				if(i_log > 100)
					break;
			}
			
			
			
		}
		        
		void Button_ignitionClick(object sender, EventArgs e)
		{
			 /*System.Media.SoundPlayer player = 
			 new System.Media.SoundPlayer();
			 player.SoundLocation = @"C:\Users\Public\Music\Sample Music\xxxx.wav";
			 player.Load();
			 player.Play();*/
			 bg_crankshaft.RunWorkerAsync();
			
		}
		

	}
}
