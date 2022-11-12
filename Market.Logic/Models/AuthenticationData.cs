using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Market.Logic.Models;
public sealed class AuthenticationData
{
    public const string EMAIL_PATTERN = @"^(?=.{3,256}$)\w+@\w+\.\w+$";
    public const string LOGIN_PATTERN = @"^[a-zA-Z0-9_.-]{6,20}$";
    
    private string? _login;
    
    public AuthenticationData(string email, Password password, string? login = null)
    {
        if (login is not null)
        {
            Login = login;
        }

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"Email can't be null or empty or contains only whitespaces", nameof(email));
        if (!Regex.IsMatch(email, EMAIL_PATTERN))
            throw new ArgumentException($"Given email is not match with email pattern", nameof(email));
        
        Email = email;

        Password = password ?? throw new ArgumentNullException(nameof(password));
    }

    /// <summary xml:lang = "ru">
    /// Логин.
    /// </summary>
    public string Login 
    {
        get => _login ?? throw new InvalidOperationException("");
            
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Login can't be null or empty or contains only whitespaces", nameof(value));

            if (!Regex.IsMatch(value, LOGIN_PATTERN))
                throw new ArgumentException($"Login must contains only letters and numbers", nameof(value));

            _login = value;
        }
    
    }

    /// <summary xml:lang = "ru">
    /// Email.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Пароль.
    /// </summary>
    public Password Password { get; }
}
