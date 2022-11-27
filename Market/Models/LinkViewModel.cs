using System.ComponentModel.DataAnnotations;

namespace Market.Models;

public sealed class LinkViewModel
{
    [Required(ErrorMessage = "Не указан внутренний идентификатор продукта.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор не может быть отрицательным.")]
    public long InternalId { get; set; }

    [Required(ErrorMessage = "Не указан внутренний идентификатор продукта.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор не может быть отрицательным.")]
    public long ProviderId { get; set; }

    [Required(ErrorMessage = "Не указан внутренний идентификатор продукта.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор не может быть отрицательным.")]
    public long ExternalId { get; set; }
}
