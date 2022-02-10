/// <summary>
/// This is a simple (for the time being) engine running simulator.
/// 
/// Changelog:
/// V0.001 New project created. Buttons and tacho created alongside rButtons for cylinder bangs. Initial logic for cr bgworker.
/// V0.002 Crankshaft rotation simulator done (backgroundworker), physics engine (sound at this time) plays sound in accordance to rpm.
/// V0.003 Sounds work for idle and acceleration, but not for decel. Radiobuttons in accordance to cylinder firing done. Accel button raises rpm like in real life.
/// </summary>

using System;
using System.ComponentModel;
using System.Media;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using System.Windows.Media;


namespace moottoriSimulaattori
{

	public partial class MainForm : Form
	{
		public struct motorState
        {
			public double crAngle;
			public double firingCylinder;
        }

		BackgroundWorker bg_crankshaft;			//simulation of crankshaft revolutions
		BackgroundWorker bg_physicsEngine;          //simulation of events happening in relation to cr rotation
		BackgroundWorker bg_increaseRPM;			//because of C# limitations, we need a thread to increase rpm like in real life (held down -> rpm++, release -> rpm--)
		double specRPS = 13.3;              //800 rpm / 60 = 13,3 cr revolutions per second (hz), max rpm 5500 = 91,667 rps cr (hz)
		motorState engine = new motorState();
		bool accPedalPress = false;
		


		public MainForm()
		{
			InitializeComponent();
			
			bg_crankshaft = new BackgroundWorker();  //creating a bg worker (thread) for our cr rotation
			
			bg_crankshaft.DoWork += new DoWorkEventHandler(bg_crankshaft_DoWork);
            bg_crankshaft.ProgressChanged += new ProgressChangedEventHandler(bg_crankshaft_ProgressChanged);
            bg_crankshaft.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_crankshaft_RunWorkerCompleted);
            bg_crankshaft.WorkerReportsProgress = true;
            bg_crankshaft.WorkerSupportsCancellation = true;        //event handlers and properties

			bg_physicsEngine = new BackgroundWorker();  //creating a bg worker (thread) for our cr rotation

			bg_physicsEngine.DoWork += new DoWorkEventHandler(bg_physicsEngine_DoWork);
			bg_physicsEngine.ProgressChanged += new ProgressChangedEventHandler(bg_physicsEngine_ProgressChanged);
			bg_physicsEngine.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_physicsEngine_RunWorkerCompleted);
			bg_physicsEngine.WorkerReportsProgress = true;
			bg_physicsEngine.WorkerSupportsCancellation = true;        //event handlers and properties

			bg_increaseRPM = new BackgroundWorker();

			bg_increaseRPM.DoWork += new DoWorkEventHandler(bg_increaseRPM_DoWork);
			bg_increaseRPM.ProgressChanged += new ProgressChangedEventHandler(bg_increaseRPM_ProgressChanged);
			bg_increaseRPM.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_increaseRPM_RunWorkerCompleted);
			bg_increaseRPM.WorkerReportsProgress = true;
			bg_increaseRPM.WorkerSupportsCancellation = true;        //event handlers and properties


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
			progressBar_RPM.Value = 0;
        }
		
		void bg_crankshaft_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

			progressBar_RPM.Value = 13;
        }

		void bg_crankshaft_DoWork(object sender, DoWorkEventArgs e)
        {
			Stopwatch runningTime = new Stopwatch();				//for sine function (rotation) we need a running time
			double elapsedSeconds = 0;
			double crankshaftAngle = 0;
			double ms = 0;

			/*
			double[] log = new double[99999];
			int i = 0;
			double[] slog = new double[99999];

			 */

			runningTime.Start();

			bg_crankshaft.ReportProgress(0);
			engine.firingCylinder = 1;

			while (!bg_crankshaft.CancellationPending)
            {
				ms = runningTime.Elapsed.TotalMilliseconds;		//accuracy
				elapsedSeconds = ms / 1000;						//conversion to seconds
				crankshaftAngle = (1 * (Math.Asin(Math.Sin(2 * 3.14 * specRPS * elapsedSeconds)))) * (180 / 3.14);    //crankshaft spinning simulation, it can be modeled as a phasor with a constant amp
				engine.crAngle = crankshaftAngle;                                                  //this simulates crankshaft pos. sensor to ecu

                #region logging/testing
                /*
				slog[i] = elapsedSeconds;
				log[i] = crankshaftAngle;
				i++;
				if (i == 99999)
					break;
				*/
                #endregion
            }
			e.Cancel = true;			//because of a race condition, this flag has to be set manually. I am not certainly sure why...
        }

		void bg_physicsEngine_DoWork(object sender, DoWorkEventArgs e)
        {
			bool soundLatch = false;            //latching for sound so no more than 1 is plaid when hitting TDC of cyl. 
		
			MediaPlayer cyl1 = new MediaPlayer();
			MediaPlayer cyl2 = new MediaPlayer();
			cyl1.Open(new System.Uri(@"C:\Users\leevi\Documents\SharpDevelop Projects\moottoriSimulaattori\moottoriSimulaattori\Sound\tdishort.wav"));
			cyl2.Open(new System.Uri(@"C:\Users\leevi\Documents\SharpDevelop Projects\moottoriSimulaattori\moottoriSimulaattori\Sound\tdishort.wav"));


			while (!bg_physicsEngine.CancellationPending)
            {
				
				if(engine.crAngle > 87 && soundLatch == false)			//4 cylinders mean that there are two reciprocating pairs. One power stroke happens every 180* of crankshaft rotation
                {                                                       //however, this approximation is a bit wrong as 4cyl engines the piston doesn't move with linear speed, it is faster near BDC, but this approx is good enough for now
					cyl1.Play();										//the positive cr TDC is to cyls 1 and 4
					Thread.Sleep(1000 / (int)specRPS);					//I suppose this could be done by events (1 piston firing --> event cylinder fired)
					cyl1.Stop();
					if (engine.firingCylinder == 1)				//check which cylinder has been fired and which is next. 1.9 TDI uses 1-3-4-2 firing order
						engine.firingCylinder = 3;
					if (engine.firingCylinder == 4)
						engine.firingCylinder = 2;
				}

				if(engine.crAngle < -87 && soundLatch == false)			//this is cylinders 2 and 3
                {
					cyl2.Play();
					Thread.Sleep(1000 / (int)specRPS);
					cyl2.Stop();
					if (engine.firingCylinder == 3)
						engine.firingCylinder = 4;
					if (engine.firingCylinder == 2)
						engine.firingCylinder = 1;
				}

				if (engine.crAngle < 87 || engine.crAngle > -87)
					soundLatch = false;

				if(accPedalPress == false && specRPS > 14.3)			//when accelerator is depressed, the revs will lower as the torque requested from engine will be enough to idle it.
                {
					specRPS = specRPS - 5;
					Thread.Sleep(20);
                }

				bg_physicsEngine.ReportProgress(0);			//used for updating the tachometer

			}
		}
	
		void bg_physicsEngine_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
			//13,3 . . . 91 rps range, oddly the progress bar percentage is similar to what wanted. Not exactly tho and too lazy to make a look-up-table
			progressBar_RPM.Value = (int)specRPS;

			if (engine.firingCylinder == 1)
				radioButton_cylinder1.Checked = true;
			if (engine.firingCylinder == 2)
				radioButton_cylinder2.Checked = true;
			if (engine.firingCylinder == 3)
				radioButton_cylinder3.Checked = true;
			if (engine.firingCylinder == 4)
				radioButton_cylinder4.Checked = true;



		}

		void bg_physicsEngine_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

		void bg_increaseRPM_DoWork(object sender, DoWorkEventArgs e)
        {
			while(accPedalPress == true)
            {
				if (specRPS > 91)
					specRPS = 91;
				if(specRPS < 91)		//RPMS rise slower under boost threshold, might add this later, for time being the increase is linear
					specRPS = specRPS + 2;
				//if(specRPS < 91 && specRPS > 34)		//When boost threshold (now set around 2k rpm, the RPM will rise faster as the turbo make it possible to inject more fuel without the stoichiometric ratio falling
                //{						//In real life the rise is very different but in this showing purpose, the acceleration is merely doubled
				//	specRPS = specRPS + 2;
                //}
				Thread.Sleep(20);
            }
        }

		void bg_increaseRPM_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

		void bg_increaseRPM_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
		        
		void Button_ignitionClick(object sender, EventArgs e)
		{
			if (!bg_crankshaft.IsBusy)
				bg_crankshaft.RunWorkerAsync();
			else
				bg_crankshaft.CancelAsync();

			if (!bg_physicsEngine.IsBusy)
				bg_physicsEngine.RunWorkerAsync();
			else
				bg_physicsEngine.CancelAsync();
			
		}


        private void button_test_Click(object sender, EventArgs e)
        {
			MediaPlayer cyl1 = new MediaPlayer();
			MediaPlayer cyl2 = new MediaPlayer();
			MediaPlayer cyl3 = new MediaPlayer();
			MediaPlayer cyl4 = new MediaPlayer();
			cyl1.Open(new System.Uri(@"C:\Users\leevi\Documents\SharpDevelop Projects\moottoriSimulaattori\moottoriSimulaattori\Sound\tdishort.wav"));
			cyl2.Open(new System.Uri(@"C:\Users\leevi\Documents\SharpDevelop Projects\moottoriSimulaattori\moottoriSimulaattori\Sound\tdishort.wav"));
			cyl3.Open(new System.Uri(@"C:\Users\leevi\Documents\SharpDevelop Projects\moottoriSimulaattori\moottoriSimulaattori\Sound\tdishort.wav"));
			cyl4.Open(new System.Uri(@"C:\Users\leevi\Documents\SharpDevelop Projects\moottoriSimulaattori\moottoriSimulaattori\Sound\tdishort.wav"));


			for (int n = 0; n < 10; n++)
			{
				cyl1.Play();
				Thread.Sleep(38);
				cyl1.Stop();
				cyl2.Play();
				Thread.Sleep(38);
				cyl2.Stop();
				cyl3.Play();
				Thread.Sleep(38);
				cyl3.Stop();
				cyl4.Play();
				Thread.Sleep(38);
				cyl4.Stop();
			}

			for (int n = 0; n < 60; n++)
			{
				cyl1.Play();
				Thread.Sleep(5);
				cyl1.Stop();
				cyl2.Play();
				Thread.Sleep(5);
				cyl2.Stop();
				cyl3.Play();
				Thread.Sleep(5);
				cyl3.Stop();
				cyl4.Play();
				Thread.Sleep(5);
				cyl4.Stop();
			}

			/*var player1 = new System.Windows.Media.MediaPlayer();
			player1.Open(new System.Uri(@"C:\Users\leevi\Documents\SharpDevelop Projects\moottoriSimulaattori\moottoriSimulaattori\Sound\tdishort.wav"));

			var player2 = new System.Windows.Media.MediaPlayer();
			player2.Open(new System.Uri(@"C:\Users\leevi\Documents\SharpDevelop Projects\moottoriSimulaattori\moottoriSimulaattori\Sound\tdishort.wav"));

			int i = 0;
			Thread.Sleep(100);

			while (i < 100)
			{
				player1.Play();
				Thread.Sleep(100);
				player2.Play();
				Thread.Sleep(100);
				i++;
			}
			*/

		}


        private void button_accPedal_MouseUp(object sender, MouseEventArgs e)
        {
			accPedalPress = false;
        }

        private void button_accPedal_MouseDown(object sender, MouseEventArgs e)
        {
			accPedalPress = true;
			bg_increaseRPM.RunWorkerAsync();
		}
    }
}


/*	while(!bg_crankshaft.CancellationPending)
{
System.Timers.Timer ignitionEvent = new System.Timers.Timer((1000 / specRPM));
ignitionEvent.Elapsed += new ElapsedEventHandler(ignitionEventHandler);
ignitionEvent.Enabled = true;

}
*/