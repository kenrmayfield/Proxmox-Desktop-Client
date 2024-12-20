using System;
using System.Diagnostics; // Required for Process.Start
using System.Windows.Forms;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proxmox_Desktop_Client.Panels;

public partial class About : Form
{
    public About()
    {
        InitializeComponent();
        
        // Add MouseHover and MouseLeave event handlers
        pictureBox_github.MouseHover += PictureBox_github_MouseHover;
        pictureBox_github.MouseLeave += PictureBox_github_MouseLeave;
        label_current.Text += " " + Program.AppVersion;

        // Call the asynchronous method to update the release label
        UpdateReleaseLabelAsync();
        CenterToParent();
    }
    private async void UpdateReleaseLabelAsync()
    {
        string latestReleaseName = await GetLatestReleaseNameAsync();
        label_release.Text += " " + latestReleaseName;
    }
    
    private void PictureBox_Click(object sender, EventArgs e)
    {
        string url = "https://github.com/sakakun/Proxmox-Desktop-Client"; // Replace with your URL

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Ensures the default web browser is used
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open the URL. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void PictureBox_github_MouseHover(object sender, EventArgs e)
    {
        PictureBox pictureBox = sender as PictureBox;
        if (pictureBox != null)
        {
            pictureBox.Cursor = Cursors.Hand;
        }
    }
    private void PictureBox_github_MouseLeave(object sender, EventArgs e)
    {
        PictureBox pictureBox = sender as PictureBox;
        if (pictureBox != null)
        {
            pictureBox.Cursor = Cursors.Default;
        }
    }
    
    public static async Task<string> GetLatestReleaseNameAsync()
    {
        string url = $"https://api.github.com/repos/sakakun/Proxmox-Desktop-Client/releases/latest";

        using HttpClient client = new HttpClient();

        // Set the User-Agent header (required by GitHub API)
        client.DefaultRequestHeaders.Add("User-Agent", "C# App");

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                using JsonDocument doc = JsonDocument.Parse(json);
                string releaseName = doc.RootElement.GetProperty("name").GetString();

                return releaseName;
            }
            else
            {
                throw new Exception($"GitHub API error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Program.DebugPoint($"An error occurred while fetching the latest release: {ex.Message}");
            return "GitHub 404";
        }
    }
}