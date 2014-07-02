namespace tmpMotionRecognition
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
            this.button_loadA = new System.Windows.Forms.Button();
            this.button_loadB = new System.Windows.Forms.Button();
            this.button_similarity = new System.Windows.Forms.Button();
            this.button_keyframe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_loadA
            // 
            this.button_loadA.Location = new System.Drawing.Point(13, 13);
            this.button_loadA.Name = "button_loadA";
            this.button_loadA.Size = new System.Drawing.Size(75, 23);
            this.button_loadA.TabIndex = 0;
            this.button_loadA.Text = "Load A";
            this.button_loadA.UseVisualStyleBackColor = true;
            this.button_loadA.Click += new System.EventHandler(this.button_loadA_Click);
            // 
            // button_loadB
            // 
            this.button_loadB.Location = new System.Drawing.Point(94, 13);
            this.button_loadB.Name = "button_loadB";
            this.button_loadB.Size = new System.Drawing.Size(75, 23);
            this.button_loadB.TabIndex = 0;
            this.button_loadB.Text = "Load B";
            this.button_loadB.UseVisualStyleBackColor = true;
            this.button_loadB.Click += new System.EventHandler(this.button_loadB_Click);
            // 
            // button_similarity
            // 
            this.button_similarity.Location = new System.Drawing.Point(176, 13);
            this.button_similarity.Name = "button_similarity";
            this.button_similarity.Size = new System.Drawing.Size(75, 23);
            this.button_similarity.TabIndex = 1;
            this.button_similarity.Text = "Similarity";
            this.button_similarity.UseVisualStyleBackColor = true;
            this.button_similarity.Click += new System.EventHandler(this.button_similarity_Click);
            // 
            // button_keyframe
            // 
            this.button_keyframe.Location = new System.Drawing.Point(13, 43);
            this.button_keyframe.Name = "button_keyframe";
            this.button_keyframe.Size = new System.Drawing.Size(75, 23);
            this.button_keyframe.TabIndex = 2;
            this.button_keyframe.Text = "KeyFrame";
            this.button_keyframe.UseVisualStyleBackColor = true;
            this.button_keyframe.Click += new System.EventHandler(this.button_keyframe_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button_keyframe);
            this.Controls.Add(this.button_similarity);
            this.Controls.Add(this.button_loadB);
            this.Controls.Add(this.button_loadA);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_loadA;
        private System.Windows.Forms.Button button_loadB;
        private System.Windows.Forms.Button button_similarity;
        private System.Windows.Forms.Button button_keyframe;
    }
}

