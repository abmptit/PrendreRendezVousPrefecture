namespace PrendreRendezVousPrefecture.Helpers
{
    public static class SmtpConfig
    {
        public static string Address => ConfigHelper.GetStringValue("Smtp.Server.Address");
        public static int Port => ConfigHelper.GetIntValue("Smtp.Server.Port");
        public static string Login => ConfigHelper.GetStringValue("Smtp.Server.Login");
        public static string Password => ConfigHelper.GetStringValue("Smtp.Server.Password");

    }
}
