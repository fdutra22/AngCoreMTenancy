using System.ComponentModel.DataAnnotations;

namespace MeuCarro.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}