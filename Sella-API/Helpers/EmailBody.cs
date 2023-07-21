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
                     <h1>Reset Your Password</h1>
                    <br>
                    <p>You are recieving this email as you asked for a password reset</p>
                    <br>
                    <a href=""http://localhost:4200/reset?email={email}&code={emailtoken}"" target=""_blank"">Reset Password</a>
                    <br>
                    <br>
                    <p>With Regards Sella Team</p>
                    </body>
                    </html>";
        }
    }
}
