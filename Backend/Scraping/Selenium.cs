using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Backend.Configurations;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Scraping;

/// <summary>
/// This class is used to scrape the TTOC website using Selenium. 
/// Called using JobService.
/// </summary>
public class Selenium : ISelenium
{
    private readonly IWebDriver _driver;
    private List<string> oldIds = new List<string>(); //To skip jobs that have already been scraped

    public Selenium()
    {
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("blink-settings=imagesEnabled=false");
        //options.AddArgument("headless");
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        _driver = new ChromeDriver(service, options);
    }

    /// <summary>
    /// This method will scrape the TTOC website and return a list of jobs.
    /// </summary>
    /// <returns>
    /// A List containing all of the jobs that were scraped.
    /// </returns>
    public async Task<List<Job>> GetJobs()
    {
        List<Job> jobs = new List<Job>();

        await Task.Run(async () =>
        {
            if (!await GoToJobPage()) return;
            var tableRows = _driver.FindElements(By.XPath(Elements.XPath.JobsInTable));

            //Added for loop instead of foreach row in tableRows to prevent throwing stale exception
            for (int x = 0; x < tableRows.Count; x++)
            {
                tableRows = _driver.FindElements(By.XPath(Elements.XPath.JobsInTable));

                if (oldIds.Contains(tableRows[x].FindElement(By.XPath(".//a[@title='Click here to view the dispatch']")).Text)) 
                    continue;

                if (!await OpenJob(tableRows[x]))
                {
                    Console.WriteLine("Failed to open job, or job was filled");
                    _driver.Navigate().Back();
                    if (_driver.FindElements(By.XPath(Elements.XPath.JobsTable)).Count > 0) continue;
                }

                Job job = await GetJobFromJobPage();
                if (job.Id != null)
                    jobs.Add(job);

                //Go back to the table of jobs
                _driver.Navigate().Back();
            }
            oldIds = GetIds();
        });

        return jobs;
    }
    public async Task<bool> AcceptJob(string jobId)
    {
        bool result = await Task.Run(async () =>
        {
            if (!await GoToJobPage()) return false;

            var btnList = _driver.FindElements(By.PartialLinkText(jobId.ToString()));
            if (btnList.Count() < 1)
                return false;
            
            var btn = btnList[0];
            btn.Click();

            //Code here to accept the button and get reply text
            var btnAccept = _driver.FindElements(By.XPath(Elements.XPath.AcceptJobButton));

            if (btnAccept.Count() > 0)
            {
                btnAccept[0].Click();
                var confirmation = _driver.FindElements(By.XPath(Elements.XPath.AcceptConfirmation));
                _driver.Navigate().Back();
                if (confirmation.Count() > 0)
                    return true;
            }

            _driver.Navigate().Back();
            

            return false;
         });        

        return result;
    }

    private async Task<bool> Login()
    {
        var result = await Task.Run(() =>
        {
            _driver.Url = Links.LoginPage;

            var usernameElements = _driver.FindElements(By.Id(Elements.Id.UserName));
            if (usernameElements.Count < 1) return false;
            usernameElements[0].SendKeys(Secrets.Username);

            var passwordElements = _driver.FindElements(By.Id(Elements.Id.Password));
            if (passwordElements.Count < 1) return false;
            passwordElements[0].SendKeys(Secrets.Password);

            var buttonElements = _driver.FindElements(By.Id(Elements.Id.LoginButton));
            if (buttonElements.Count < 1) return false;
            buttonElements[0].Click();

            var mainPageExistsElements = _driver.FindElements(By.Id(Elements.Id.MainPageExists));
            if (mainPageExistsElements.Count < 1) return false;

            return true;
        });

        return result;
    }
    private async Task<bool> GoToJobPage()
    {
        var result = await Task.Run(async () =>
        {
            _driver.Url = Links.JobsPage;

            //If the table is not found then double check that the client is logged in
            var jobsTableElements = _driver.FindElements(By.XPath(Elements.XPath.JobsTable));
            if (jobsTableElements.Count < 1)
            {
                if (!await Login()) return false;
                _driver.Url = Links.JobsPage;
            }

            jobsTableElements = _driver.FindElements(By.XPath(Elements.XPath.JobsTable));
            if (jobsTableElements.Count < 1) return false;

            return true;
        });

        return result;

    }
    private List<string> GetIds()
    {
        List<string> ids = new List<string>();

        var tableRows = _driver.FindElements(By.XPath(Elements.XPath.JobsInTable));
        foreach (var row in tableRows)
        {
            var idButton = row.FindElements(By.XPath(Elements.XPath.JobIdATag));
            if (idButton.Count < 1) continue;
            ids.Add(idButton[0].Text);
        }

        return ids;
    }
    private async Task<bool> OpenJob(IWebElement jobRow)
    {
        bool result = false;
        try
        {
            result = await Task.Run(() =>
            {
                var idButton = jobRow.FindElements(By.XPath(Elements.XPath.JobIdATag));
                if (idButton.Count < 1) return false;

                //Scroll the element into view for long tables
                var js = (IJavaScriptExecutor)_driver;
                if (idButton[0].Location.Y > 200) js.ExecuteScript($"window.scrollTo({0}, {idButton[0].Location.Y - 200})");
                idButton[0].Click();

                //Verify that the page loaded successfully
                if (_driver.FindElements(By.XPath(Elements.XPath.VerifyJobOpened)).Count < 1) return false;
                //Verify that the job has not been filled
                if (_driver.FindElements(By.PartialLinkText("This job has been filled.")).Count > 0) return false;

                return true;
            });
        }
        catch
        {
            Console.WriteLine("Error opening job!");
            return false;
        }

        return result;
    }
    private async Task<Job> GetJobFromJobPage()
    {
        Job job = new();
        job = await Task.Run(() =>
        {
            try
            {
                // Return if job info is not available
                if (_driver.FindElements(By.XPath(Elements.XPath.Teacher)).Count < 1)
                    return job;

                job.Id = _driver.FindElement(By.XPath(".//input[@name='VOICEID']")).GetAttribute("value");
                job.Teacher = _driver.FindElement(By.XPath(Elements.XPath.Teacher)).Text;
                job.SubjectsAndLevels = _driver.FindElement(By.XPath(Elements.XPath.SubjectAndLevels)).Text;
                job.Message = _driver.FindElement(By.XPath(Elements.XPath.Message)).Text;

                var tableRows = _driver.FindElements(By.XPath(Elements.XPath.JobTable));

                job.StartDate = DateTime.Parse(tableRows[0].FindElement(By.XPath("./*[1]")).Text);
                job.EndDate = DateTime.Parse(tableRows[tableRows.Count - 1].FindElement(By.XPath("./*[1]")).Text);
                job.Position = tableRows[0].FindElement(By.XPath("./*[2]")).Text;
                job.Location = tableRows[0].FindElement(By.XPath("./*[3]")).Text;
                job.StartTime = DateTime.Parse(tableRows[0].FindElement(By.XPath("./*[4]")).Text);
                job.EndTime = DateTime.Parse(tableRows[0].FindElement(By.XPath("./*[5]")).Text);
            }
            catch
            {
                Console.WriteLine("Error getting job information!");
            }

            return job;
        });

        return job;
    }

    public void Dispose()
    {
        _driver.Dispose();
    }
}