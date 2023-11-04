using Microsoft.AspNetCore.Mvc;

namespace Api.Interfaces;

public interface IJobsController
{
    Task<IActionResult> Get();
    Task<IActionResult> Accept(string jobId);    
}