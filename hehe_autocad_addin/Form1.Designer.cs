namespace hehe_autocad_addin
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelTop = new Panel();
            btnAddToBookmark = new Button();
            txtUrl = new TextBox();
            flowLayoutPanelNav = new FlowLayoutPanel();
            btnBack = new Button();
            btnForward = new Button();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelBottom = new Panel();
            label1 = new Label();
            btnSend = new Button();
            txtPrompt = new TextBox();
            panelBookmarks = new FlowLayoutPanel();
            panelTop.SuspendLayout();
            flowLayoutPanelNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(btnAddToBookmark);
            panelTop.Controls.Add(txtUrl);
            panelTop.Controls.Add(flowLayoutPanelNav);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Padding = new Padding(5);
            panelTop.Size = new Size(896, 45);
            panelTop.TabIndex = 1;
            // 
            // btnAddToBookmark
            // 
            btnAddToBookmark.Dock = DockStyle.Right;
            btnAddToBookmark.Location = new Point(850, 5);
            btnAddToBookmark.Name = "btnAddToBookmark";
            btnAddToBookmark.Size = new Size(41, 35);
            btnAddToBookmark.TabIndex = 3;
            btnAddToBookmark.Text = "⭐";
            btnAddToBookmark.UseVisualStyleBackColor = true;
            // 
            // txtUrl
            // 
            txtUrl.Dock = DockStyle.Fill;
            txtUrl.Location = new Point(101, 5);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(790, 27);
            txtUrl.TabIndex = 0;
            // 
            // flowLayoutPanelNav
            // 
            flowLayoutPanelNav.Controls.Add(btnBack);
            flowLayoutPanelNav.Controls.Add(btnForward);
            flowLayoutPanelNav.Dock = DockStyle.Left;
            flowLayoutPanelNav.Location = new Point(5, 5);
            flowLayoutPanelNav.Margin = new Padding(0);
            flowLayoutPanelNav.Name = "flowLayoutPanelNav";
            flowLayoutPanelNav.Size = new Size(96, 35);
            flowLayoutPanelNav.TabIndex = 2;
            // 
            // btnBack
            // 
            btnBack.Enabled = false;
            btnBack.Location = new Point(3, 3);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(40, 29);
            btnBack.TabIndex = 0;
            btnBack.Text = "<";
            btnBack.UseVisualStyleBackColor = true;
            // 
            // btnForward
            // 
            btnForward.Enabled = false;
            btnForward.Location = new Point(49, 3);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(40, 29);
            btnForward.TabIndex = 1;
            btnForward.Text = ">";
            btnForward.UseVisualStyleBackColor = true;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(0, 96);
            webView21.Name = "webView21";
            webView21.Size = new Size(896, 410);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(label1);
            panelBottom.Controls.Add(btnSend);
            panelBottom.Controls.Add(txtPrompt);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 506);
            panelBottom.Margin = new Padding(3, 4, 3, 4);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(5);
            panelBottom.Size = new Size(896, 53);
            panelBottom.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 16);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 2;
            label1.Text = "Smart Prompt";
            // 
            // btnSend
            // 
            btnSend.Dock = DockStyle.Right;
            btnSend.Location = new Point(805, 5);
            btnSend.Margin = new Padding(3, 4, 3, 4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(86, 43);
            btnSend.TabIndex = 1;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            // 
            // txtPrompt
            // 
            txtPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPrompt.Location = new Point(115, 12);
            txtPrompt.Margin = new Padding(3, 4, 3, 4);
            txtPrompt.Name = "txtPrompt";
            txtPrompt.Size = new Size(683, 27);
            txtPrompt.TabIndex = 0;
            // 
            // panelBookmarks
            // 
            panelBookmarks.AutoScroll = true;
            panelBookmarks.Dock = DockStyle.Top;
            panelBookmarks.Location = new Point(0, 45);
            panelBookmarks.Name = "panelBookmarks";
            panelBookmarks.Padding = new Padding(5);
            panelBookmarks.Size = new Size(896, 51);
            panelBookmarks.TabIndex = 3;
            panelBookmarks.WrapContents = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(896, 559);
            Controls.Add(webView21);
            Controls.Add(panelBookmarks);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Name = "Form1";
            Text = "hehe Gemini Add-in";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            flowLayoutPanelNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelNav;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddToBookmark;
        private System.Windows.Forms.FlowLayoutPanel panelBookmarks;
    }
}
