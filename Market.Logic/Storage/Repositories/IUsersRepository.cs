using General.Storage;

using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;

public interface IUsersRepository : IKeyableRepository<User, InternalID>
{
    /// <summary xml:lang = "ru">
    /// Метод возвращающий пользователя с соответсвующим адресом электронной почты.
    /// </summary>
    /// <param name="email" xml:lang = "ru">Адрес электрноой почты пользователя.</param>
    /// <returns xml:lang = "ru">Пользователь с указанной почтой или <see langword="null"/>, если такого пользователя нет.</returns>
    public User? GetByEmail(string email);

    /// <summary xml:lang = "ru">
    /// Проверяет можно ли аунтефицировать пользователя по указанным учетным данным.
    /// </summary>
    /// <param name="email" xml:lang = "ru">Почта пользователя.</param>
    /// <param name="password" xml:lang = "ru">Пароль пользователя.</param>
    /// <param name="user">Пользователь с указанными учетныйми данными.</param>
    /// <returns xml:lang = "ru"><see langword="true"/> - если есть пользователь с такими учетными данными, иначе - <see langword="false"/>.</returns>
    public bool IsCanAuthenticate(string email, string password, out User user);
}
