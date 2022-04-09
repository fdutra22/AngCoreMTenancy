using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace MeuCarro.Localization
{
    public static class MeuCarroLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(MeuCarroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(MeuCarroLocalizationConfigurer).GetAssembly(),
                        "MeuCarro.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
