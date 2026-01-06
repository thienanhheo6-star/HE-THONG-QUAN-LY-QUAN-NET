using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;

namespace QLQuanNet
{
    public class RevenueRecord
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class ReportService
    {
        public List<RevenueRecord> GetRevenueByDay(DateTime from, DateTime to)
        {
            using (var db = new Model1())
            {
                // Revenue calculated as: NapTien (top-ups) + SuDungDichVu (service charges)
                // Exclude SuDungMay (play time) fees from revenue.

                // 1) Top-ups grouped by date
                var naptiens = db.NapTiens
                    .Where(n => n.ThoiGian >= from && n.ThoiGian <= to)
                    .ToList()
                    .GroupBy(n => n.ThoiGian.Date)
                    .Select(g => new { Date = g.Key, Amount = g.Sum(x => x.SoTien) })
                    .ToList();

                // 2) Service charges: link SuDungDichVu -> SuDungMay to get date
                var usages = db.SuDungMays
                    .Where(s => s.ThoiGianBatDau >= from && s.ThoiGianBatDau <= to)
                    .ToList();

                var usageIds = usages.Select(u => u.MaSD).Where(id => id != 0).ToList();
                var sdvList = db.SuDungDichVus.Where(sdv => usageIds.Contains(sdv.MaSD)).ToList();

                var svcByDate = (from sdv in sdvList
                                 join sd in usages on sdv.MaSD equals sd.MaSD
                                 select new { Date = sd.ThoiGianBatDau.Date, Amount = sdv.ThanhTien ?? 0m })
                                .GroupBy(x => x.Date)
                                .Select(g => new { Date = g.Key, Amount = g.Sum(x => x.Amount) })
                                .ToList();

                // Merge top-ups and services into daily totals
                var daily = new List<RevenueRecord>();

                foreach (var n in naptiens)
                {
                    daily.Add(new RevenueRecord { Date = n.Date, Amount = n.Amount });
                }

                foreach (var s in svcByDate)
                {
                    var day = daily.FirstOrDefault(d => d.Date == s.Date);
                    if (day != null) day.Amount += s.Amount;
                    else daily.Add(new RevenueRecord { Date = s.Date, Amount = s.Amount });
                }

                return daily.OrderBy(r => r.Date).ToList();
            }
        }
    }
}
