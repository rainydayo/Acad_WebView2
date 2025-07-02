using Microsoft.VisualBasic;
using Microsoft.Web.WebView2.Core;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

// Import AutoCAD namespaces
#if ZWCAD

using ZwSoft.ZwCAD.ApplicationServices;
using ZwSoft.ZwCAD.DatabaseServices;
using OpenMode = ZwSoft.ZwCAD.DatabaseServices.OpenMode;
using cad = ZwSoft.ZwCAD;

#else
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using OpenMode = Autodesk.AutoCAD.DatabaseServices.OpenMode;
using cad = Autodesk.AutoCAD;
#endif


namespace hehe_autocad_addin
{
    // --- NEW: Class to represent a single bookmark for serialization ---
    public class Bookmark
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public partial class Form1 : Form
    {
        // HttpClient is intended to be instantiated once and re-used throughout the life of an application.
        private static readonly HttpClient client = new HttpClient();
        private string _localHtmlPath;
        private string _commandToExecute;
        private readonly string _initialUrl;
        // --- NEW: Path for the bookmarks JSON file ---
        private readonly string _bookmarksFilePath;


        public Form1()
        {
            InitializeComponent();

            // --- NEW: Define the path for saving bookmarks ---
            string userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "hehe_autocad_addin");
            _bookmarksFilePath = Path.Combine(userDataFolder, "bookmarks.json");


            this.Load += Form1_Load;

            // Wire up event handlers for the controls
            this.btnBack.Click += BtnBack_Click;
            this.btnForward.Click += BtnForward_Click;
            this.txtUrl.KeyDown += TxtUrl_KeyDown;
            this.btnSend.Click += BtnSend_Click;
            this.txtPrompt.KeyDown += TxtPrompt_KeyDown;
            this.btnAddToBookmark.Click += BtnAddToBookmark_Click;

            // Add context menu for opening in a new tab
            var contextMenu = new ContextMenuStrip();
            var menuItem = new ToolStripMenuItem("Open in new tab");
            menuItem.Click += TxtUrl_OpenInNewTab_Click;
            contextMenu.Items.Add(menuItem);
            txtUrl.ContextMenuStrip = contextMenu;
        }

        // New constructor for opening with a specific URL, calls the default constructor
        public Form1(string initialUrl) : this()
        {
            _initialUrl = initialUrl;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Store the path to index.html for later use
            _localHtmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "index.html");
            if (!File.Exists(_localHtmlPath))
            {
                MessageBox.Show("index.html not found. The data prompt feature will not work.");
            }

            try
            {
                // --- MODIFIED: Use the same folder path defined in the constructor ---
                var env = await CoreWebView2Environment.CreateAsync(null, Path.GetDirectoryName(_bookmarksFilePath));

                await webView21.EnsureCoreWebView2Async(env);

                // Add event handlers that require CoreWebView2 to be initialized
                webView21.CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;
                webView21.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
                webView21.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

                // --- MODIFIED: Load bookmarks from file or use defaults ---
                LoadBookmarks();

                // Navigate to initial URL or default
                string urlToNavigate = !string.IsNullOrEmpty(_initialUrl) ? _initialUrl : "https://www.google.com";
                NavigateToUrl(urlToNavigate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"WebView2 failed to initialize: {ex.Message}");
            }
        }

        // --- New Tab Context Menu Handler ---

        private void TxtUrl_OpenInNewTab_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text;
            if (!string.IsNullOrWhiteSpace(url))
            {
                // Use the new constructor to open a new window with the specified URL
                var newForm = new Form1(url)
                {
                    TopMost = true
                };
                newForm.Shown += (s, args) => newForm.BringToFront();
                newForm.Show();
            }
        }

        // --- Navigation and UI Event Handlers ---

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (webView21?.CoreWebView2 != null && webView21.CoreWebView2.CanGoBack)
            {
                webView21.CoreWebView2.GoBack();
            }
        }

        private void BtnForward_Click(object sender, EventArgs e)
        {
            if (webView21?.CoreWebView2 != null && webView21.CoreWebView2.CanGoForward)
            {
                webView21.CoreWebView2.GoForward();
            }
        }

        private void TxtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                NavigateToUrl(txtUrl.Text);
            }
        }

        private void CoreWebView2_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            txtUrl.Text = webView21.CoreWebView2.Source;
        }

        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            btnBack.Enabled = webView21.CoreWebView2.CanGoBack;
            btnForward.Enabled = webView21.CoreWebView2.CanGoForward;
            txtUrl.Text = webView21.CoreWebView2.Source;

            // If we have a pending command and we just loaded the local html, execute it
            if (!string.IsNullOrEmpty(_commandToExecute) && webView21.CoreWebView2.Source.StartsWith("file://"))
            {
                FetchAndDisplayData(_commandToExecute);
                _commandToExecute = null; // Clear the command after executing
            }
        }

        void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            // Future use
        }

        private void NavigateToUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && webView21?.CoreWebView2 != null)
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("file://"))
                {
                    url = "https://" + url;
                }
                try
                {
                    webView21.CoreWebView2.Navigate(url);
                }
                catch (UriFormatException)
                {
                    MessageBox.Show("The URL format is invalid.");
                }
            }
        }

        // --- Bookmark Management (MODIFIED) ---

        private void InitializeDefaultBookmarks()
        {
            AddBookmarkToBar("Google", "https://www.google.com");
            AddBookmarkToBar("YouTube", "https://www.youtube.com");
            SaveBookmarks(); // Save the defaults so they are there next time
        }

        private void BtnAddToBookmark_Click(object sender, EventArgs e)
        {
            string currentUrl = txtUrl.Text;
            if (string.IsNullOrWhiteSpace(currentUrl) || currentUrl.Equals("about:blank", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Cannot bookmark an empty page.");
                return;
            }

            bool originalTopMost = this.TopMost;
            try
            {
                this.TopMost = false;
                string name = Interaction.InputBox("Enter a name for this bookmark:", "Add Bookmark", "New Bookmark");

                if (!string.IsNullOrWhiteSpace(name))
                {
                    AddBookmarkToBar(name, currentUrl);
                    // --- MODIFIED: Save bookmarks after adding a new one ---
                    SaveBookmarks();
                }
            }
            finally
            {
                this.TopMost = originalTopMost;
            }
        }

        private void AddBookmarkToBar(string name, string url)
        {
            var bookmarkButton = new Button
            {
                Text = name,
                Tag = url,
                AutoSize = true,
                FlatStyle = FlatStyle.System,
                Cursor = Cursors.Hand
            };
            bookmarkButton.Click += (s, args) => NavigateToUrl((s as Button).Tag.ToString());

            var deleteButton = new Button
            {
                Text = "x",
                ForeColor = Color.Red,
                Font = new System.Drawing.Font("Arial", 6, FontStyle.Bold),
                Width = 18,
                Height = 18,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            deleteButton.FlatAppearance.BorderSize = 0;

            var container = new FlowLayoutPanel
            {
                AutoSize = true,
                Margin = new Padding(2, 0, 2, 0),
                BorderStyle = BorderStyle.FixedSingle
            };

            deleteButton.Click += (s, args) =>
            {
                panelBookmarks.Controls.Remove(container);
                container.Dispose();
                // --- MODIFIED: Save bookmarks after deleting one ---
                SaveBookmarks();
            };

            container.Controls.Add(bookmarkButton);
            container.Controls.Add(deleteButton);

            panelBookmarks.Controls.Add(container);
        }

        // --- NEW: Methods to save and load bookmarks ---

        private void SaveBookmarks()
        {
            var bookmarks = new List<Bookmark>();
            foreach (FlowLayoutPanel container in panelBookmarks.Controls)
            {
                if (container.Controls.Count > 0 && container.Controls[0] is Button bookmarkButton)
                {
                    bookmarks.Add(new Bookmark
                    {
                        Name = bookmarkButton.Text,
                        Url = bookmarkButton.Tag.ToString()
                    });
                }
            }

            try
            {
                string json = JsonSerializer.Serialize(bookmarks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_bookmarksFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving bookmarks: {ex.Message}");
            }
        }

        private void LoadBookmarks()
        {
            if (File.Exists(_bookmarksFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_bookmarksFilePath);
                    // Prevent crash on empty or corrupted file
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        InitializeDefaultBookmarks();
                        return;
                    }

                    var bookmarks = JsonSerializer.Deserialize<List<Bookmark>>(json);
                    if (bookmarks != null && bookmarks.Any())
                    {
                        foreach (var bookmark in bookmarks)
                        {
                            AddBookmarkToBar(bookmark.Name, bookmark.Url);
                        }
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading bookmarks: {ex.Message}");
                }
            }
            // If file doesn't exist or is empty/corrupt, load defaults
            InitializeDefaultBookmarks();
        }


        // --- Gemini API and AutoCAD Data Interaction (Unchanged) ---

        private void TxtPrompt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSend_Click(sender, e);
            }
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            string userPrompt = txtPrompt.Text.Trim();
            if (string.IsNullOrEmpty(userPrompt))
            {
                MessageBox.Show("Please enter a prompt.");
                return;
            }
            if (string.IsNullOrEmpty(_localHtmlPath))
            {
                MessageBox.Show("index.html path not found. Cannot display data.");
                return;
            }

            try
            {
                SetLoadingState(true);

                string command = await GetCommandFromGemini(userPrompt);

                if (string.IsNullOrEmpty(command))
                {
                    MessageBox.Show("Could not determine the command from the prompt. Please try again.");
                    return;
                }

                _commandToExecute = command;
                webView21.CoreWebView2.Navigate(new Uri(_localHtmlPath).AbsoluteUri);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task<string> GetCommandFromGemini(string prompt)
        {
            string apiKey = env.gemini_key;
            if (apiKey == "YOUR_GEMINI_API_KEY")
            {
                MessageBox.Show("Please replace 'YOUR_GEMINI_API_KEY' in Form1.cs with your actual Gemini API key.");
                return null;
            }

            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            var systemInstruction = new
            {
                role = "system",
                parts = new[] { new { text = "You are an AutoCAD data assistant. Your task is to interpret the user's request and identify what kind of data they want to retrieve from the drawing. Based on the user's prompt, determine the type of data required. Your response must be a single JSON object with one key: 'command'. The value for this key should be one of the following lowercase strings: 'linetypes', 'layers', 'blocks', 'textstyles', or 'dimstyles'. For example, if the user asks 'list all linetypes', you should return {\"command\": \"linetypes\"}." } }
            };

            var payload = new
            {
                contents = new[]
                {
                    new { role = "user", parts = new[] { new { text = prompt } } }
                },
                system_instruction = systemInstruction
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            using (JsonDocument doc = JsonDocument.Parse(responseBody))
            {
                var candidates = doc.RootElement.GetProperty("candidates");
                var firstCandidate = candidates[0];
                var contentPart = firstCandidate.GetProperty("content").GetProperty("parts")[0];
                var rawText = contentPart.GetProperty("text").GetString();

                using (JsonDocument commandDoc = JsonDocument.Parse(rawText))
                {
                    return commandDoc.RootElement.GetProperty("command").GetString();
                }
            }

            return null;
        }

        private void FetchAndDisplayData(string command)
        {
            var doc = cad.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var db = doc.Database;

            object resultData = null;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                switch (command.ToLower())
                {
                    case "linetypes":
                        var linetypeTable = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead);
                        resultData = new { title = "All Linetypes", data = linetypeTable.Cast<ObjectId>().Select(id => ((LinetypeTableRecord)tr.GetObject(id, OpenMode.ForRead)).Name).ToList() };
                        break;
                    case "layers":
                        var layerTable = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                        resultData = new { title = "All Layers", data = layerTable.Cast<ObjectId>().Select(id => ((LayerTableRecord)tr.GetObject(id, OpenMode.ForRead)).Name).ToList() };
                        break;
                    case "blocks":
                        var blockTable = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                        resultData = new { title = "All Block Definitions", data = blockTable.Cast<ObjectId>().Select(id => ((BlockTableRecord)tr.GetObject(id, OpenMode.ForRead)).Name).Where(name => !name.StartsWith("*")).ToList() };
                        break;
                    case "textstyles":
                        var textStyleTable = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                        resultData = new { title = "All Text Styles", data = textStyleTable.Cast<ObjectId>().Select(id => ((TextStyleTableRecord)tr.GetObject(id, OpenMode.ForRead)).Name).ToList() };
                        break;
                    case "dimstyles":
                        var dimStyleTable = (DimStyleTable)tr.GetObject(db.DimStyleTableId, OpenMode.ForRead);
                        resultData = new { title = "All Dimension Styles", data = dimStyleTable.Cast<ObjectId>().Select(id => ((DimStyleTableRecord)tr.GetObject(id, OpenMode.ForRead)).Name).ToList() };
                        break;
                    default:
                        MessageBox.Show($"Unknown command: '{command}'");
                        return;
                }
                tr.Commit();
            }

            string jsonResult = JsonSerializer.Serialize(resultData);
            string script = $"window.receiveData({jsonResult});";
            webView21.CoreWebView2.ExecuteScriptAsync(script);
        }

        private void SetLoadingState(bool isLoading)
        {
            this.txtPrompt.Enabled = !isLoading;
            this.btnSend.Enabled = !isLoading;
            this.btnSend.Text = isLoading ? "..." : "Send";
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }
    }
}