using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Backend.Configurations;
using Backend.Interfaces;
using System.Text.Json;
using Backend.Models;

namespace Backend.Scraping;

/// <summary>
/// This class mocks the Selenium class. 
/// It will return static data instead of scraping the web.
/// </summary>
public class MockSelenium : ISelenium
{
    public async Task<List<Job>> GetJobs()
    {        
        string json = await File.ReadAllTextAsync("../Backend/StaticData.json");
        List<Job>? jobs = JsonSerializer.Deserialize<List<Job>>(json);
        return jobs ?? new List<Job>();
    }

    public Task<bool> AcceptJob(string jobId)
    {
        return Task.FromResult(true);
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
}