﻿namespace tmpFeaturesTest
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
            this.button_catch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_catch
            // 
            this.button_catch.Location = new System.Drawing.Point(13, 13);
            this.button_catch.Name = "button_catch";
            this.button_catch.Size = new System.Drawing.Size(75, 23);
            this.button_catch.TabIndex = 0;
            this.button_catch.Text = "Catch";
            this.button_catch.UseVisualStyleBackColor = true;
            this.button_catch.Click += new System.EventHandler(this.button_catch_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button_catch);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_catch;
    }
}

