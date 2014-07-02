namespace tmpMotionRecognitionUsingKeyFrame
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_loadclips = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_loadclips
            // 
            this.button_loadclips.Location = new System.Drawing.Point(13, 13);
            this.button_loadclips.Name = "button_loadclips";
            this.button_loadclips.Size = new System.Drawing.Size(75, 23);
            this.button_loadclips.TabIndex = 0;
            this.button_loadclips.Text = "Load Clips";
            this.button_loadclips.UseVisualStyleBackColor = true;
            this.button_loadclips.Click += new System.EventHandler(this.button_loadclips_Click);
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(188, 94);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 1;
            this.button_start.Text = "開始";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.button_loadclips);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_loadclips;
        private System.Windows.Forms.Button button_start;
    }
}

