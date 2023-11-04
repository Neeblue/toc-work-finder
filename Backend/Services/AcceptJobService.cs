using OpenQA.Selenium;
using Backend.Interfaces;

namespace Backend.Services;

public class AcceptJobService : IAcceptJobService
{
    private readonly ISelenium _selenium;

    public AcceptJobService(ISelenium selenium)
    {
        _selenium = selenium;
    }

    public async Task<bool> AcceptJob(string jobId)
    {
        return await _selenium.AcceptJob(jobId);
    }
}