# AutoCAD 2026 WebView2 Add‑In

A simple AutoCAD 2026 add‑in that embeds a WebView2 browser window directly within AutoCAD—no more switching between tabs!

---

## Features

* Adds a new **`hehe`** panel under the **Add‑ins** ribbon tab in AutoCAD 2026.
* Click **Open WebView** to launch a modeless browser window docked on top.
* Keeps the window always in front (`TopMost`) so you can browse without losing focus in AutoCAD.

---

## Prerequisites

1. **AutoCAD 2026**
2. **.NET 8.0 SDK** installed
3. **Microsoft Edge WebView2 Runtime** (Evergreen) installed

   * Download: [https://developer.microsoft.com/microsoft-edge/webview2/#download-section](https://developer.microsoft.com/microsoft-edge/webview2/#download-section)
4. **Microsoft.Web.WebView2** NuGet package referenced in your project

---

## Installation

1. **Launch AutoCAD 2026** and run the **NETLOAD** command:

   ```text
   NETLOAD "C:\path\to\<project>\hehe_autocad_addin\bin\Debug\net8.0-windows\hehe_autocad_addin.dll"
   ```

2. You should see a new **Add‑ins** tab.
   Under **Add‑ins ▶ hehe**, click **Open WebView** to launch the browser window.

---

## Usage

1. With AutoCAD open, switch to the **Add‑ins** ribbon tab.
2. Click **Open WebView** to open the embedded browser.
3. The WebView window will remain on top—navigate freely without losing focus in AutoCAD.

---

## Troubleshooting

* If you see **`unable to load WebView2Loader.dll`**, ensure:

  * The Edge WebView2 runtime is installed.
  * `WebView2Loader.dll` is copied to your output folder (set **Copy to Output Directory** = **Always**).

* If the **Add‑ins** tab is missing, run:

  ```text
  NETLOAD "...\hehe_autocad_addin.dll"
  RESTART
  ```

* Make sure you target **x64** in your project settings to match AutoCAD’s process.

---

## License

No License

