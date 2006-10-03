namespace ClearCanvas.Ris.Client.Adt.View.WinForms
{
    partial class JscriptComponentControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._script = new System.Windows.Forms.TextBox();
            this._result = new System.Windows.Forms.TextBox();
            this._runButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _script
            // 
            this._script.AcceptsReturn = true;
            this._script.Location = new System.Drawing.Point(20, 26);
            this._script.Multiline = true;
            this._script.Name = "_script";
            this._script.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._script.Size = new System.Drawing.Size(329, 144);
            this._script.TabIndex = 0;
            // 
            // _result
            // 
            this._result.Location = new System.Drawing.Point(20, 206);
            this._result.Multiline = true;
            this._result.Name = "_result";
            this._result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._result.Size = new System.Drawing.Size(329, 144);
            this._result.TabIndex = 1;
            // 
            // _runButton
            // 
            this._runButton.Location = new System.Drawing.Point(274, 177);
            this._runButton.Name = "_runButton";
            this._runButton.Size = new System.Drawing.Size(75, 23);
            this._runButton.TabIndex = 2;
            this._runButton.Text = "Run";
            this._runButton.UseVisualStyleBackColor = true;
            this._runButton.Click += new System.EventHandler(this._runButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Script";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Result";
            // 
            // JscriptComponentControl
            // 
            this.AcceptButton = this._runButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._runButton);
            this.Controls.Add(this._result);
            this.Controls.Add(this._script);
            this.Name = "JscriptComponentControl";
            this.Size = new System.Drawing.Size(370, 369);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _script;
        private System.Windows.Forms.TextBox _result;
        private System.Windows.Forms.Button _runButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
