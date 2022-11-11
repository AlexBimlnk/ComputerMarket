using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Market.Logic.Models;
public sealed class AuthenticationData
{
    public const string EMAIL_PATTERN = @"^\w+@\w+\.\w+$";
    public const string ONLY_LETTERS_AND_NUMBERS_PATTERN = @"^[a-zA-Z0-9_.-]*$";
    public const int LOGIN_MAX_LENGTH = 20;
    public const int LOGIN_MIN_LENGTH = 6;
    public const int EMAIL_MAX_LENGTH = 256;
    public const int EMAIL_MIN_LENGTH = 3;

    public AuthenticationData(string login, string email, Password password)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException($"Login can't be null or empty or contains only whitespaces", nameof(login));

        if (login.Length < LOGIN_MIN_LENGTH || login.Length > LOGIN_MAX_LENGTH)
            throw new ArgumentException($"Login must have length between {LOGIN_MIN_LENGTH} and {LOGIN_MAX_LENGTH}", nameof(login));

        if (!Regex.IsMatch(login, ONLY_LETTERS_AND_NUMBERS_PATTERN))
            throw new ArgumentException($"Login must contains only letters and numbers", nameof(login));

        Login = login;

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"Email can't be null or empty or contains only whitespaces", nameof(email));
        if (email.Length < EMAIL_MIN_LENGTH || email.Length > EMAIL_MAX_LENGTH)
            throw new ArgumentException($"Login must have length between {EMAIL_MIN_LENGTH} and {EMAIL_MAX_LENGTH}", nameof(email));
        if (!Regex.IsMatch(email, EMAIL_PATTERN))
            throw new ArgumentException($"Given email is not match with email pattern", nameof(email));
        Email = email;

        Password = password ?? throw new ArgumentNullException(nameof(login));
    }

    /// <summary xml:lang = "ru">
    /// Логин.
    /// </summary>
    public string Login { get; }

    /// <summary xml:lang = "ru">
    /// Пароль.
    /// </summary>
    public Password Password { get; }

    /// <summary xml:lang = "ru">
    /// Email.
    /// </summary>
    public string Email { get; }
}
