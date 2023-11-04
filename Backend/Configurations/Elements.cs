namespace Backend.Configurations
{
    public class Elements
    {
        public static class Id
        {
            public static string UserName = "Username";
            public static string Password = "Password";
            public static string LoginButton = "SsoLogin_Btn";
            public static string MainPageExists = "dashboardTabsTable";
            public static string MessageBox = "MessageContainer";

        }
        public static class XPath
        {
            public static string JobsTable = "//*[@id='ctl00_brokerDiv']/form/div/table";
            public static string JobIdATag = ".//a[@title='Click here to view the dispatch']";
            public static string JobsInTable = "//*[@id='ctl00_brokerDiv']/form/div/table/tbody/tr"; //tr[1] is the first job
            public static string VerifyJobOpened = ".//*[contains(text(),'Workboard - Dispatch Details')]";
            public static string Teacher = "//*[@id='ctl00_brokerDiv']/form/div/table[1]/tbody/tr[1]/td[2]/label";
            public static string SubjectAndLevels = "//*[@id='ctl00_brokerDiv']/form/div/table[1]/tbody/tr[2]/td[2]/label";
            public static string Message = "//*[@id='MessageContainer']/textarea";
            public static string JobTable = "//*[@id='ctl00_brokerDiv']/form/div/table[2]/tbody/tr";
            public static string AcceptJobButton = "//*[@id='ctl00_brokerDiv']/form/div/div[2]/div/button[3]";
            public static string AcceptConfirmation = "//*[@id='ctl00_brokerDiv']/form/div/div[1]/div";


        }
        public static class Title
        {
            public static string IdTitle = "Click here to view the dispatch";
        }
    }
}