using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using Api.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase, IJobsController
{
    private readonly IJobService _jobService;
    private readonly IJobFilterService _jobFilterService;
    private readonly IAcceptJobService _acceptJobService;
    private readonly IEnumerable<INotifyService> _notifyServices;

    public JobsController(IJobService jobService, IJobFilterService jobFilterService, IAcceptJobService acceptJobService, IEnumerable<INotifyService> notifyServices)
    {
        _jobService = jobService;
        _jobFilterService = jobFilterService;
        _acceptJobService = acceptJobService;
        _notifyServices = notifyServices; //TODO: Move this code (and the delay) outside of the controller to a separate frontend.
    }

    [HttpGet(Name = "GetJobs")]
    public async Task<IActionResult> Get()
    {
        List<Job> jobs = await _jobService.GetJobs();
        return Ok(jobs);
    }

    [HttpPost("{jobId}", Name = "AcceptJob")]
    public async Task<IActionResult> Accept(string jobId)
    {
        Regex regex = new Regex(@"^\d{7}$");
        if (string.IsNullOrEmpty(jobId) || !regex.IsMatch(jobId))
        {
            return BadRequest();
        }

        bool result = await _acceptJobService.AcceptJob(jobId);
        return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
    }
}