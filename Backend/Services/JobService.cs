using System.Text.Json;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services;

public class JobService : IJobService
{
    private readonly ISelenium _selenium;

    public JobService(ISelenium selenium)
    {
        _selenium = selenium;
    }

    /// <summary>
    /// Gets a list of jobs from the TTOC website. Uses Selenium to scrape the website.
    /// Converts the returned results into a list of Job objects.
    /// Recommend calling JobFilterService.FilterJobs() after fetching jobs.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Job>> GetJobs()
    {
        var jobs = await _selenium.GetJobs();
        return jobs;
    }

    public void Dispose()
    {
        _selenium.Dispose();
    }
}