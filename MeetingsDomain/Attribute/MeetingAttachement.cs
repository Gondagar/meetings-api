using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MeetingsDomain.Attribute
{
    class MeetingAttachement : ValidationAttribute
    {
        private string _unsafeFiles = @"^.+\.(bat|sh|cmd|jar|exe)$";

        public override bool IsValid(object value)
        {
            IFormFile file = (IFormFile)value;

            return !Regex.IsMatch(file.FileName, _unsafeFiles, RegexOptions.IgnoreCase);
        }
    }
}
