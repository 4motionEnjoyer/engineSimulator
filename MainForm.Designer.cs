/*
 * Created by SharpDevelop.
 * User: leevi
 * Date: 7.2.2022
 * Time: 18.28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace moottoriSimulaattori
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.radioButton_cylinder4 = new System.Windows.Forms.RadioButton();
			this.radioButton_cylinder3 = new System.Windows.Forms.RadioButton();
			this.radioButton_cylinder2 = new System.Windows.Forms.RadioButton();
			this.radioButton_cylinder1 = new System.Windows.Forms.RadioButton();
			this.progressBar_RPM = new System.Windows.Forms.ProgressBar();
			this.label_RPM = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.button_accPedal = new System.Windows.Forms.Button();
			this.button_ignition = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// radioButton_cylinder4
			// 
			this.radioButton_cylinder4.Location = new System.Drawing.Point(429, 33);
			this.radioButton_cylinder4.Name = "radioButton_cylinder4";
			this.radioButton_cylinder4.Size = new System.Drawing.Size(104, 24);
			this.radioButton_cylinder4.TabIndex = 0;
			this.radioButton_cylinder4.TabStop = true;
			this.radioButton_cylinder4.Text = "Cylinder 4";
			this.radioButton_cylinder4.UseVisualStyleBackColor = true;
			// 
			// radioButton_cylinder3
			// 
			this.radioButton_cylinder3.Location = new System.Drawing.Point(429, 63);
			this.radioButton_cylinder3.Name = "radioButton_cylinder3";
			this.radioButton_cylinder3.Size = new System.Drawing.Size(104, 24);
			this.radioButton_cylinder3.TabIndex = 1;
			this.radioButton_cylinder3.TabStop = true;
			this.radioButton_cylinder3.Text = "Cylinder 3";
			this.radioButton_cylinder3.UseVisualStyleBackColor = true;
			// 
			// radioButton_cylinder2
			// 
			this.radioButton_cylinder2.Location = new System.Drawing.Point(429, 94);
			this.radioButton_cylinder2.Name = "radioButton_cylinder2";
			this.radioButton_cylinder2.Size = new System.Drawing.Size(104, 24);
			this.radioButton_cylinder2.TabIndex = 2;
			this.radioButton_cylinder2.TabStop = true;
			this.radioButton_cylinder2.Text = "Cylinder 2";
			this.radioButton_cylinder2.UseVisualStyleBackColor = true;
			// 
			// radioButton_cylinder1
			// 
			this.radioButton_cylinder1.Location = new System.Drawing.Point(429, 125);
			this.radioButton_cylinder1.Name = "radioButton_cylinder1";
			this.radioButton_cylinder1.Size = new System.Drawing.Size(104, 24);
			this.radioButton_cylinder1.TabIndex = 3;
			this.radioButton_cylinder1.TabStop = true;
			this.radioButton_cylinder1.Text = "Cylinder 1";
			this.radioButton_cylinder1.UseVisualStyleBackColor = true;
			// 
			// progressBar_RPM
			// 
			this.progressBar_RPM.Location = new System.Drawing.Point(31, 34);
			this.progressBar_RPM.Name = "progressBar_RPM";
			this.progressBar_RPM.Size = new System.Drawing.Size(287, 23);
			this.progressBar_RPM.TabIndex = 4;
			// 
			// label_RPM
			// 
			this.label_RPM.Location = new System.Drawing.Point(31, 5);
			this.label_RPM.Name = "label_RPM";
			this.label_RPM.Size = new System.Drawing.Size(100, 23);
			this.label_RPM.TabIndex = 5;
			this.label_RPM.Text = "RPM";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(31, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(287, 23);
			this.label1.TabIndex = 6;
			this.label1.Text = "0 . . . 1k . . . 2k . . . 3k . . . 4k . . . 5k . . . 6k";
			// 
			// button_accPedal
			// 
			this.button_accPedal.Location = new System.Drawing.Point(31, 138);
			this.button_accPedal.Name = "button_accPedal";
			this.button_accPedal.Size = new System.Drawing.Size(100, 41);
			this.button_accPedal.TabIndex = 7;
			this.button_accPedal.Text = "Acc. pedal";
			this.button_accPedal.UseVisualStyleBackColor = true;
			// 
			// button_ignition
			// 
			this.button_ignition.Location = new System.Drawing.Point(171, 138);
			this.button_ignition.Name = "button_ignition";
			this.button_ignition.Size = new System.Drawing.Size(101, 41);
			this.button_ignition.TabIndex = 8;
			this.button_ignition.Text = "Ignition";
			this.button_ignition.UseVisualStyleBackColor = true;
			this.button_ignition.Click += new System.EventHandler(this.Button_ignitionClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1067, 504);
			this.Controls.Add(this.button_ignition);
			this.Controls.Add(this.button_accPedal);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label_RPM);
			this.Controls.Add(this.progressBar_RPM);
			this.Controls.Add(this.radioButton_cylinder1);
			this.Controls.Add(this.radioButton_cylinder2);
			this.Controls.Add(this.radioButton_cylinder3);
			this.Controls.Add(this.radioButton_cylinder4);
			this.Name = "MainForm";
			this.Text = "moottoriSimulaattori";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button button_ignition;
		private System.Windows.Forms.Button button_accPedal;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label_RPM;
		private System.Windows.Forms.ProgressBar progressBar_RPM;
		private System.Windows.Forms.RadioButton radioButton_cylinder1;
		private System.Windows.Forms.RadioButton radioButton_cylinder2;
		private System.Windows.Forms.RadioButton radioButton_cylinder3;
		private System.Windows.Forms.RadioButton radioButton_cylinder4;
	}
}
