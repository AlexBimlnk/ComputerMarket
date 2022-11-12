﻿using System.ComponentModel.DataAnnotations;

using Market.Logic.Models;

using PasswordClass = Market.Logic.Models.Password;

namespace Market.Models.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Не указано имя пользователя")]
    [RegularExpression(AuthenticationData.LOGIN_PATTERN,
         ErrorMessage = "Имя пользователя должно содержать только цифры и латинские символы, и иметь длину от 6 до 20 символов.")]
    [Display(Name = "Имя пользователя")]
    public string Login { get; set; } = default!;

    [Required(ErrorMessage = "Не указан адрес электронной почты")]
    [RegularExpression(AuthenticationData.EMAIL_PATTERN,
         ErrorMessage = "Адрес электронной не соотвествует формату.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    [RegularExpression(PasswordClass.PASSWORD_PATTERN,
         ErrorMessage = "Пароль может содержать только цифры и латинские символы, и иметь длинну от 8 до 20 символов.")]
    public string Password { get; set; } = default!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = default!;
}