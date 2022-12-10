using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Logic.Reports;

/// <summary>
/// Описывает создателя отчетов.
/// </summary>
public interface IReportBuilder
{
    /// <summary>
    /// Устанавливает идентификатор поставщика по которому будет
    /// создаваться отчет.
    /// </summary>
    /// <param name="providerId"> Идентификатор поставщика. </param>
    public void SetProviderId(ID providerId);

    /// <summary>
    /// Устанавливает начало периода, по которому будет идти отчет.
    /// </summary>
    /// <param name="date"> Дата начала периода. </param>
    public void SetStartPeriod(DateOnly date);

    /// <summary>
    /// Устанавливает конец периода, по которому будет идти отчет.
    /// </summary>
    /// <param name="date"> Дата конца периода. </param>
    public void SetEndPeriod(DateOnly date);

    /// <summary>
    /// Создает отчет.
    /// </summary>
    /// <returns> Отчет по периоду типа <see cref="Report"/>. </returns>
    /// <exception cref="InvalidOperationException">
    /// Когда не установлены данные, для создания отчета.
    /// </exception>
    public Report CreateReport();
}
