using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;


namespace Files {
    public class RegexValidationRule : ValidationRule {
        private string _pattern;
        private Regex _regex;

        public string Pattern {
            get { return _pattern; }
            set {
                _pattern = value;
                _regex = new Regex (_pattern, RegexOptions.IgnoreCase);
            }
        }


        public RegexValidationRule () {
            Pattern = @"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:\"";|/]+$";
        }


        public override ValidationResult Validate (object value, CultureInfo cultureInfo) {
            if (value == null || value.ToString () == "" || !_regex.Match (value.ToString ()).Success) {
                return new ValidationResult (false, "The value is not a valid file name");
            } else {
                return new ValidationResult (true, null);
            }
        }
    }
}
