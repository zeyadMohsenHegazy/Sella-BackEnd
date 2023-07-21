namespace Sella_API.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailtoken)
        {
            return $@"<html>
                    <head>
                    </head>
                    <body>
                        <div style=""border: 3px solid #026d02;width: fit-content;"">
                     <h1 style=""margin-left: 3.5em;"">Reset Your Password</h1>
                    <h3 style=""padding-left: 1em;padding-right: 1em;"">You are recieving this email as you asked for a password reset</h3> 
                    <br>
                    <a href=""http://localhost:4200/reset?email={email}&code={emailtoken}"" target=""_blank"" style=""padding: 20px;border: 5px solid #15c535;background-color: #15c535;color: #fff; font-weight: bold; text-decoration: none; font-size: larger; margin-left: 8.5em;"">Reset Password</a>
                    <br>
                    <br>
                    <h4 style=""margin-left: 1em;"">With Regards Sella Technical Team..</h4>
                    </div>
                    </body>
                    </html>";
        }
    }
}
