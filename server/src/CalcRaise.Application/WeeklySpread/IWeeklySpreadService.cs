namespace CalcRaise.Application.WeeklySpread;

/// <summary>
/// "Haftalık yayılım" heat-map verisi: kas grubu x gün, hangi egzersizlerin planlandığı,
/// o hafta girilen set sayısı/hacim ve aynı kas grubunun 48 saatten az arayla tekrarlanıp
/// tekrarlanmadığı (bkz. warehouse/01-analiz-ve-degerlendirme.md §4).
/// </summary>
public interface IWeeklySpreadService
{
    Task<WeeklySpreadReportDto> GetReportAsync(int workoutWeekId, CancellationToken ct = default);
}
