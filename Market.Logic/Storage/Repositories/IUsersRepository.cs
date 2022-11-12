using General.Storage;

using Market.Logic.Models;

namespace Market.Logic.Storage.Repositories;

public interface IUsersRepository : IKeyableRepository<User, InternalID>
{
    /// <summary xml:lang = "ru">
    /// Метод возвращающий пользователя с соответсвующим адресом электронной почты.
    /// </summary>
    /// <param name="email" xml:lang = "ru">
    /// Адрес электрноой почты пользователя.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Пользователь с указанной почтой или <see langword="null"/>, если такого пользователя нет.
    /// </returns>
    public User? GetByEmail(string email);

    /// <summary xml:lang = "ru">
    /// Проверяет можно ли аунтефицировать пользователя по указанным учетным данным.
    /// </summary>
    /// <param name="data" xml:lang = "ru">
    ///  Данные для аутенфикации пользователя.
    /// </param>
    /// <param name="user">
    /// Пользователь с указанными учетныйми данными.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see langword="true"/> - если есть пользователь с такими учетными данными, иначе - <see langword="false"/>.
    /// </returns>
    public bool IsCanAuthenticate(AuthenticationData data, out User user);

    /// <summary>
    /// Проверяет соответвует ли введенный пароль, паролю пользователя в репозитории.
    /// </summary>
    /// <param name="id">Индетификатор пользователя.</param>
    /// <param name="password">Введенный пароль.</param>
    /// <returns>
    ///  <see langword="false"/> - если пользователя с таким <paramref name="id"/> - нет, 
    ///  введенный пароль не совпадает с пользовательским, 
    ///  а иначе - <see langword="true"/>.
    /// </returns>
    public bool IsPasswordMatch(InternalID id, Password password);
}
