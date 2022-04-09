using MeuCarro.Debugging;

namespace MeuCarro
{
    public class MeuCarroConsts
    {
        public const string LocalizationSourceName = "MeuCarro";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "e9cbb84999884a2f9bc7f6081e33e238";
    }
}
