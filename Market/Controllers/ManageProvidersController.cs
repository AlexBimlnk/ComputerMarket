using General.Storage;

using Market.Logic;
using Market.Logic.Models;
using Market.Models;

using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

/// <summary xml:lang = "ru">
/// Контроллер для управления поставщиками.
/// </summary>
public sealed class ManageProvidersController : Controller
{
    private readonly ILogger<ManageProvidersController> _logger;
    private readonly IKeyableRepository<Provider, ID> _providerRepository;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="ManageProvidersController"/>.
    /// </summary>
    /// <param name="logger" xml:lang = "ru">
    /// Логгер.
    /// </param>
    /// <param name="providerRepository" xml:lang = "ru">
    /// Репозиторий провайдеров.
    /// </param>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если любой из входных параметров оказался <see langword="null"/>.
    /// </exception>
    public ManageProvidersController(
        ILogger<ManageProvidersController> logger,
        IKeyableRepository<Provider, ID> providerRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));
    }

    /// <summary xml:lang = "ru">
    /// Возвращает список провайдеров.
    /// </summary>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    public IActionResult ListAsync() => View(_providerRepository.GetEntities());

    /// <summary xml:lang = "ru">
    /// Возвращает форму для редактирования информации о поставщиках.
    /// </summary>
    /// <returns> <see cref="ActionResult"/>. </returns>
    public ActionResult Edit(long id) => View(_providerRepository.GetByKey(new(id)));

    /// <summary xml:lang = "ru">
    /// Запрос на изменение провайдера.
    /// </summary>
    /// <param name="link" xml:lang = "ru"> 
    /// Провайдер с новыми данными. 
    /// </param>
    /// <returns> <see cref="Task{TResult}"/>. </returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditAsync(ManageProviderViewModel provider)
    {
        if (ModelState.IsValid)
        {
            var domainProvider = _providerRepository.GetByKey(new(provider.Key))!;

            var updatedProvider = new Provider(
                domainProvider.Key,
                domainProvider.Name,
                new Margin(provider.Margin),
                domainProvider.PaymentTransactionsInformation);

            // ToDo: update provider info
            _providerRepository.Delete(domainProvider);
            await _providerRepository.AddAsync(updatedProvider);
            _providerRepository.Save();

            return RedirectToAction("List");
        }

        return View();
    }
}
